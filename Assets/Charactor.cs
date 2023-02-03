using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Charactor : MonoBehaviour
{
    public float speed = 10.0f;
    public float attackSpeed = 1.0f;
    public float attackRange = 1.0f;
    public float attackDamage = 1.0f;
    public float hp = 100.0f;
    public float maxHp = 100.0f;

    public float attack(Enemy e)
    {
        e.attacked(attackDamage);
        return attackDamage;
    }

    public float attacked(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            die();
            return hp;
        }
        return damage;
    }

    public void die()
    {
        Destroy(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // if click will attack enemy
        // if enemy will respawn

        // Cast a ray straight down.

        Ray2D ray = new Ray2D(transform.position, Vector2.left);
        Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin,  ray.direction * attackRange);

        if (hit)
        {
            if (hit.collider.tag == "Enemy")
            {
                // Calculate the distance from the surface and the "error" relative
                // to the floating height.
                float distance = Mathf.Abs(hit.point.x - transform.position.x);
                Debug.Log(distance);
                if (distance < attackRange)
                {
                    Debug.Log("Attack");
                    Enemy e = hit.collider.gameObject.GetComponent<Enemy>();
                    Debug.Log("dmg: " + attack(e));
                }
            }
        }
    }
}
