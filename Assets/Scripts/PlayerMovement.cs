using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 4f;
    public float jumpHeight = 3f;
    float moveX;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundDistance = 0.45f;
    public bool doubleJumpEnabled = false;
    bool hasDoubleJumped = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis("Horizontal");

        if (isGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }
            hasDoubleJumped = false;
        }
        if (doubleJumpEnabled == true)
        {
            if (!isGrounded() && hasDoubleJumped == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                    hasDoubleJumped = true;
                }
            }
        }
        Debug.Log(hasDoubleJumped);
    }   
    private void FixedUpdate()
    {
        Vector2 moveDir = transform.right * moveX + transform.forward;
        moveDir = Vector2.ClampMagnitude(moveDir, 1f);

        //Add force
        rb.AddForce(moveDir, ForceMode2D.Impulse);

        //Clamp movement
        float verticalVelocity = Mathf.Clamp(rb.velocity.y, float.MinValue, jumpHeight);
        Vector2 horizontalVelocity = new Vector2(moveDir.x * moveSpeed, verticalVelocity);
        rb.velocity = horizontalVelocity;
    }
    bool isGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundLayer)) return true; 
        return false;
    }
}   
