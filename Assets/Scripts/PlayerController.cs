using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float jumpForce;

    private Rigidbody2D rb;

    private float horizontalInput;
    private bool jumpPress;
    private bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CaputureInput();
    }

    void CaputureInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpPress = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W);
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Attack();
    }

    void Move()
    {
        rb.velocity = new Vector2(horizontalInput * walkSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (jumpPress && onGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPress = false;
        }
    }

    void Attack()
    {
        
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
}
