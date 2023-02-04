using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Character character;

    private float horizontalInput = 0f;
    private bool jumpPress = false;

    void Awake()
    {
        character = GetComponent<Character>();
    }

    void Update()
    {
        CaptureInput();
    }

    void CaptureInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        jumpPress = Input.GetButton("Jump");

        if (Input.GetButton("Fire1"))
        {
            character.StartAttack();

        }
    }

    void FixedUpdate()
    {
        character.Move(horizontalInput);

        if (jumpPress)
        {
            character.Jump();
            jumpPress = false;
        }
    }
}
