using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{  
    public LayerMask layermask;

    private Transform target;
    private Character character;
    private float distanceToTarget;
    
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

    private void MeleeBot() {
        const float attackRange = 0.2f;
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
                character.Move(Mathf.Clamp(direction.x, -0.5f, 0.5f));

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

    private void RangerBot() {
        const float attackRange = 2f;
        distanceToTarget = Vector2.Distance(target.position, transform.position);
        Debug.Log(currState);
    
        Vector2 direction = (Vector2) target.position - (Vector2) transform.position;

        switch (currState)
        {
            case AIState.idle:
            {
                if (distanceToTarget > attackRange) {
                    currState = AIState.run;
                }
                else {
                    currState = AIState.attack;
                }
                break;
            }
            case AIState.run:
            {
                character.Move(Mathf.Clamp(direction.x, -0.5f, 0.5f));

                if (distanceToTarget <= attackRange)
                    currState = AIState.attack;
                break;
            }
            case AIState.attack: 
            {
                if (direction.x < 0) character.faceLeft();
                else if (direction.y > 0) character.faceRight();

                character.Move(0);
                character.StartAttack();
                currState = AIState.idle;
                break;
            }
        };

    }

    void Update()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }       

        if (character.GetType() == typeof(Ranger)) {
            RangerBot();
        }
        else {
            MeleeBot();
        }
    }
}
