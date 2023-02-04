using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{  
    private Transform target;
    private Character character;
    private float distanceToTarget;
    private float attackRange = 0.2f;
    
    private enum AIState {
        idle, run, attack, backoff
    }
    private float backOffChance = 0.5f;
    private int counter = 0;
    
    private AIState currState = AIState.idle;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        character = GetComponent<Character>(); 
        if (target == null)
            Debug.LogError("Something wrong");
    }

    private bool ShouldBackOff() {
        return Random.value < backOffChance;
    }

    void Update()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }       

        distanceToTarget = Vector2.Distance(target.position, transform.position);
        
        Debug.Log(currState);
    
        switch (currState)
        {
            case AIState.idle:
            {
                if (distanceToTarget > attackRange) {
                    currState = AIState.run;
                }
                else if (Input.GetKeyDown("e") && ShouldBackOff()) {
                    currState = AIState.backoff;
                }
                else {
                    currState = AIState.attack;
                }
                break;
            }
            case AIState.run:
            {
                Vector2 direction = (Vector2) target.position - (Vector2) transform.position;
                character.Move(Mathf.Clamp(direction.x, -0.1f, 0.1f));

                if (distanceToTarget <= attackRange)
                    currState = AIState.attack;
                break;
            }
            case AIState.attack: 
            {
                character.Move(0);
                character.StartAttack();
                currState = AIState.idle;
                break;
            }
            case AIState.backoff:
            {
                // this guy not moving so fight him to dead
                if (counter >= 1000) {
                    currState = AIState.attack;
                    backOffChance = 0.0f;
                    counter = 0;
                }

                if (distanceToTarget > attackRange * 5){
                    character.Jump();
                    currState = AIState.idle;
                }

                Vector2 direction = (Vector2) target.position - (Vector2) transform.position;
                character.Move(Mathf.Clamp(-direction.x, -0.3f, 0.3f));
                counter += 1;
                break;
            }
        };
    }
}
