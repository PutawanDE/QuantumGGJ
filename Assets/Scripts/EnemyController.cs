using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{  
    private Transform target;
    private Character character;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        character = GetComponent<Character>(); 
        if (target == null)
            Debug.LogError("Something wrong");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2) target.position - (Vector2) transform.position;
        character.Move(Mathf.Clamp(direction.x, -1f, 1f));
    }
}
