using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected bool facingLeft;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius;

    [SerializeField] private float baseAttackDamage;
    [SerializeField] private float baseMaxHp;
    [SerializeField] private float baseWalkSpeed;

    [SerializeField] protected float attackRange;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float hp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected GameObject crown;
    [SerializeField] protected int deathCount = 0;

    [SerializeField] protected float raycastOffset;

    [Header("Blood Sprout Prefab")]
    public GameObject bloodSprout;

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
        UpdateStat();
    }

    public void Initialize(string tag)
    {
        gameObject.tag = tag;

        int layer = LayerMask.NameToLayer(tag);
        gameObject.layer = layer;

        UpdateStat();

        if (tag == "Enemy")
        {
            faceRight();
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController)
            {
                Destroy(playerController);
            }

            gameObject.AddComponent<EnemyController>();
            crown.SetActive(true);
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
            crown.SetActive(false);
        }
    }

    protected virtual void UpdateStat()
    {
        walkSpeed = baseWalkSpeed * (1 + deathCount * 0.1f);
        attackDamage = baseAttackDamage * (1 + deathCount * 0.1f);
        maxHp = baseMaxHp * (1 + deathCount * 0.1f);
        hp = maxHp;
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

    public Ray2D CreateRay()
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
        if (bloodSprout != null)
            Instantiate(bloodSprout, transform);

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
            gameController.NextRound();
        }

        Destroy(this.gameObject);
    }

    public virtual void Move(float horizontalInput)
    {
        if (isAttacking) return;

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

    public void faceLeft()
    {
        facingLeft = true;
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void faceRight()
    {
        facingLeft = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

}
