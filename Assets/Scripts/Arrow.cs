using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private float attackDamage;
    private Character owner;
    private Vector2 direction;

    private float dmgDealt;

    public void Initialize(Character owner, float attackDamage, float speed, Vector2 direction)
    {
        this.owner = owner;
        this.attackDamage = attackDamage;
        this.speed = speed;
        this.direction = direction;
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
        if (direction.x < 0) {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            if (owner.gameObject.tag != other.gameObject.tag)
            {
                Character opponent = other.gameObject.GetComponent<Character>();
                dmgDealt = opponent.TakeDmg(attackDamage, owner);
                Debug.Log("Dmg dealt: " + dmgDealt);
                Destroy(gameObject);
            }
        }
    }
}
