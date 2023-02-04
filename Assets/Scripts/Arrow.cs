using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed;
    private float attackDamage;
    private string owner;
    private Vector2 direction;

    private float dmgDealt;

    public void Initialize(string owner, float attackDamage, float speed, Vector2 direction)
    {
        this.owner = owner;
        this.attackDamage = attackDamage;
        this.speed = speed;
        this.direction = direction;
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
        {
            if (owner != other.gameObject.tag)
            {
                Character opponent = other.gameObject.GetComponent<Character>();
                dmgDealt = opponent.TakeDmg(attackDamage);
                Debug.Log("Dmg dealt: " + dmgDealt);
                Destroy(gameObject);
            }
        }
    }
}
