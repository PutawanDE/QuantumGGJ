using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected bool facingLeft;

    [SerializeField] protected float walkSpeed = 10.0f;
    [SerializeField] protected float jumpForce = 5f;
    [SerializeField] protected float attackRatePerSec = 1.0f;
    [SerializeField] protected float attackRange = 1.0f;
    [SerializeField] protected float attackDamage = 1.0f;
    [SerializeField] protected float hp = 100.0f;
    [SerializeField] protected float maxHp = 100.0f;

    [SerializeField] protected float raycastOffset;

    private Rigidbody2D rb;
    private Animator animator;

    private bool isAttacking = false;
    private bool onGround = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Initialize(gameObject.tag);
    }

    // TODO: Call after instantiation
    public void Initialize(string tag)
    {
        if (tag == "Enemy")
        {
            facingLeft = false;
            // TODO: Toggle Enemy Controller
        }
        else if (tag == "Player")
        {
            facingLeft = true;
            // TODO: Toggle Player Controller
        }
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                Attack();
            }
        } else
        {
            isAttacking = false;
        }
    }

    public virtual void StartAttack()
    {
        if (onGround)
        {
            animator.SetTrigger("Attack");
        }
    }

    protected virtual void Attack()
    {
        float dmgDealt = 0f;

        Debug.Log("Attack");
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
                }
            }
        }

        animator.ResetTrigger("Attack");
        Debug.Log("Dmg dealt: " + dmgDealt);
    }

    private Ray2D CreateRay()
    {
        if (!facingLeft)
        {
            Vector2 raycastPos = new Vector2(
            transform.position.x + raycastOffset,
            transform.position.y);

            return new Ray2D(raycastPos, Vector2.right);
        }
        else
        {
            Vector2 raycastPos = new Vector2(
            transform.position.x - raycastOffset,
            transform.position.y);

            return new Ray2D(raycastPos, Vector2.left);
        }
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
            facingLeft = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetInteger("Direction", 1);
        }
        else if (horizontalInput > 0f)
        {
            facingLeft = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetInteger("Direction", 1);
        }
        else
        {
            animator.SetInteger("Direction", 0);
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
            animator.SetBool("IsInAir?", false);
            onGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
        {
            animator.SetBool("IsInAir?", true);
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
