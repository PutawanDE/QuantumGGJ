using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Vector2 facingDirection;

    [SerializeField] protected float walkSpeed = 10.0f;
    [SerializeField] protected float jumpForce = 5f;
    [SerializeField] protected float attackRatePerSec = 1.0f;
    [SerializeField] protected float attackRange = 1.0f;
    [SerializeField] protected float attackDamage = 1.0f;
    [SerializeField] protected float hp = 100.0f;
    [SerializeField] protected float maxHp = 100.0f;

    [SerializeField] protected float raycastOffset;

    private Rigidbody2D rb;

    private bool isAttacking = false;
    private bool onGround = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Initialize(gameObject.tag);
    }

    // TODO: Call after instantiation
    public void Initialize(string tag)
    {
        if (tag == "Enemy")
        {
            facingDirection = Vector2.right;
            // TODO: Toggle Enemy Controller
        }
        else if (tag == "Player")
        {
            facingDirection = Vector2.left;
            // TODO: Toggle Player Controller
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

    private Ray2D CreateRay()
    {
        if (facingDirection == Vector2.right)
        {
            Vector2 raycastPos = new Vector2(
            transform.position.x + raycastOffset,
            transform.position.y);

            return new Ray2D(raycastPos, Vector2.right);
        }
        else if (facingDirection == Vector2.left)
        {
            Vector2 raycastPos = new Vector2(
            transform.position.x - raycastOffset,
            transform.position.y);

            return new Ray2D(raycastPos, Vector2.left);
        }

        return default(Ray2D);
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

    public virtual void Move(float horizontalInput)
    {
        rb.velocity = new Vector2(horizontalInput * walkSpeed, rb.velocity.y);
        if (horizontalInput < 0f)
        {
            facingDirection = Vector2.left;
        }
        else if (horizontalInput > 0f)
        {
            facingDirection = Vector2.right;
        }

    }

    public virtual void Jump()
    {
        if (onGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            onGround = false;
        }
    }

    public float GetHP()
    {
        return hp;
    }

    public float GetMaxHP()
    {
        return maxHp;
    }

}
