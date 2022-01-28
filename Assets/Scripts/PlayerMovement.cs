using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;

    public float moveSpeed = 4f;
    public float jumpHeight = 3f; 
    public BoxCollider2D groundCheck;
    public LayerMask groundLayer;
    public float groundDistance = 0.45f;
    public bool doubleJumpEnabled = false;
    bool hasDoubleJumped = false;

    float moveX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            }

            hasDoubleJumped = false;
        }

        if (doubleJumpEnabled == true)
        {
            if (!IsGrounded() && hasDoubleJumped == false)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                    hasDoubleJumped = true;
                }
            }
        }

        Animations();
    }   

    void FixedUpdate()
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

    bool IsGrounded()
    {
        if (Physics2D.OverlapBox(groundCheck.transform.position, groundCheck.size, 0, groundLayer))
            return true;

        return false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            //Die function
            Debug.Log("Dead");
        }
    }

    void Animations()
    {
        //Left and right turning
        float maxAngle = 5f;

        Vector3 spriteLocalEulers = sprite.transform.localEulerAngles;
        spriteLocalEulers.z = -(moveX * maxAngle);

        sprite.transform.localRotation = Quaternion.Euler(spriteLocalEulers);

        //Vertical, jumping
        if (IsGrounded())
        {
            Vector3 spriteLocalScale = sprite.transform.localScale;
            spriteLocalScale.y = Mathf.MoveTowards(spriteLocalScale.y, 2f, 5f * Time.deltaTime);

            sprite.transform.localScale = spriteLocalScale;
        }
        else
        {
            Vector3 spriteLocalScale = sprite.transform.localScale;
            spriteLocalScale.y = Mathf.MoveTowards(spriteLocalScale.y, 1.9f, 5f * Time.deltaTime);

            sprite.transform.localScale = spriteLocalScale;
        }
    }

    public float GetMoveX()
    {
        return moveX;
    }
}   
