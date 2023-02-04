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
        idle, run, attack
    }
    
    private AIState currState = AIState.idle;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        character = GetComponent<Character>(); 
        if (target == null)
            Debug.LogError("Something wrong");
    }

    void FixedUpdate()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        distanceToTarget = Mathf.Abs(((Vector2) target.position - (Vector2) transform.position).x);
        Debug.Log(distanceToTarget);    
        Debug.Log(currState);    
        
        switch (currState)
        {
            case AIState.idle:
                if (distanceToTarget > attackRange) 
                    currState = AIState.run;
                else {
                    currState = AIState.attack;
                }
                break;
            case AIState.run:
                Vector2 direction = (Vector2) target.position - (Vector2) transform.position;
                character.Move(Mathf.Clamp(direction.x, -0.1f, 0.1f));

                if (distanceToTarget <= attackRange)
                    currState = AIState.attack;
                break;
            case AIState.attack:
                character.Move(0);
                character.StartAttack();
                currState = AIState.idle;
                break;
        };
    }
}
