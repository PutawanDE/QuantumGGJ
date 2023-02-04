using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float walkSpeed = 10.0f;
    [SerializeField] protected float attackRatePerSec = 1.0f;
    [SerializeField] protected float attackRange = 1.0f;
    [SerializeField] protected float attackDamage = 1.0f;
    [SerializeField] protected float hp = 100.0f;
    [SerializeField] protected float maxHp = 100.0f;

    [SerializeField] protected float raycastOffset;

    private bool isAttacking = false;

    // TODO: Call after instantiation
    public void Initialize(string tag)
    {
        if (tag == "Enemy")
        {
            // Add enemy controller
        }
        else if (tag == "Player")
        {
            // Add player controller
            gameObject.AddComponent<PlayerController>();
        }
    }

    public virtual void StartAttack()
    {
        if (!isAttacking) StartCoroutine(AttackCooldown());
    }


    private IEnumerator AttackCooldown()
    {
        isAttacking = true;
        Debug.Log("Start attacking");
        Attack();
        yield return new WaitForSeconds(1f / attackRatePerSec);
        isAttacking = false;
    }

    private float Attack()
    {
        float dmgDealt = 0f;

        Ray2D ray = CreateRay();
        Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction * attackRange);

        if (hit && hit.collider.tag != gameObject.tag)
        {
            if (hit.collider.tag == "Enemy" || hit.collider.tag == "Player")
            {
                // Calculate the distance from the surface and the "error" relative
                // to the floating height.
                float distance = Mathf.Abs(hit.point.x - transform.position.x);
                Debug.Log(distance);
                if (distance < attackRange)
                {
                    Debug.Log("Attack");
                    Character opponent = hit.collider.gameObject.GetComponent<Character>();
                    dmgDealt = opponent.TakeDmg(attackDamage);
                    Debug.Log("dmg: " + dmgDealt);
                }
            }
        }

        return dmgDealt;
    }

    public virtual float TakeDmg(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
            return hp;
        }
        return damage;
    }

    public virtual void Die()
    {
        // send dead event bla bla

        Destroy(gameObject);
    }

    private Ray2D CreateRay()
    {
        if (gameObject.tag == "Enemy")
        {
            Vector2 raycastPos = new Vector2(
            transform.position.x + raycastOffset,
            transform.position.y);

            return new Ray2D(raycastPos, Vector2.right);
        }
        else if (gameObject.tag == "Player")
        {
            Vector2 raycastPos = new Vector2(
            transform.position.x - raycastOffset,
            transform.position.y);

            return new Ray2D(raycastPos, Vector2.left);
        }

        Debug.LogWarning("Incorrect Character Tag. It must be either 'Enemy', or 'Player'.");
        return default(Ray2D);
    }
}
