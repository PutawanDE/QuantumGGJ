using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;

    private Character character;
    private Rigidbody2D rb;

    private float horizontalInput;
    private bool jumpPress;
    private bool onGround;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    void Update()
    {
        CaptureInput();
    }

    void CaptureInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpPress = Input.GetButton("Jump");

        if (Input.GetButton("Fire1"))
        {
            character.StartAttack();

        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    void Move()
    {
        rb.velocity = new Vector2(horizontalInput * character.walkSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (jumpPress && onGround)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpPress = false;
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
}
