using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected bool facingLeft;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] protected float walkSpeed = 10.0f;
    [SerializeField] protected float jumpForce = 5f;
    [SerializeField] protected float attackRange = 1.0f;
    [SerializeField] protected float attackDamage = 1.0f;
    [SerializeField] protected float hp = 100.0f;
    [SerializeField] protected float maxHp = 100.0f;

    [SerializeField] protected float raycastOffset;

    private Rigidbody2D rb;
    private Animator animator;

    private GameController gameController;

    private bool isAttacking = false;
    private bool onGround = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // TODO: Call after instantiation
    public void Initialize(string tag)
    {
        gameObject.tag = tag;
        if (tag == "Enemy")
        {
            faceRight();
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController)
            {
                Destroy(playerController);
            }

            gameObject.AddComponent<EnemyController>();
        }
        else if (tag == "Player")
        {
            faceLeft();
            EnemyController enemyController = GetComponent<EnemyController>();
            if (enemyController)
            {
                Destroy(enemyController);
            }

            gameObject.AddComponent<PlayerController>();
        }
    }

    private void Update()
    {
        CheckGround();
        animator.SetBool("IsInAir?", !onGround);

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (!isAttacking)
            {
                isAttacking = true;
                Attack();
            }
        }
        else
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
                    dmgDealt = opponent.TakeDmg(attackDamage, this);
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

    public virtual float TakeDmg(float damage, Character attacker)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die(attacker);
            return hp;
        }
        return damage;
    }

    public virtual void Die(Character attacker)
    {
        if (gameObject.tag == "Player")
        {
            gameController.GameOver();
        }
        else if (gameObject.tag == "Enemy")
        {
            gameController.NextRound(this.gameObject);
        }

        Destroy(attacker.gameObject);
        Destroy(gameObject);
    }

    public virtual void Move(float horizontalInput)
    {
        rb.velocity = new Vector2(horizontalInput * walkSpeed, rb.velocity.y);
        if (horizontalInput < 0f)
        {
            faceLeft();
            animator.SetInteger("Direction", 1);
        }
        else if (horizontalInput > 0f)
        {
            faceRight();
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

    private void CheckGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

        if (hit)
        {
            onGround = true;
        }
        else
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

    private void faceLeft()
    {
        facingLeft = true;
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void faceRight()
    {
        facingLeft = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
