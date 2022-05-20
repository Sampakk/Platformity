using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;

    [Header("Movement")]
    public float moveSpeed = 4f;
    public float iceMultiplier = 4f;
    public float jumpHeight = 3f;
    public BoxCollider2D groundCheck;
    public LayerMask groundLayer;
    public float groundDistance = 0.45f;
    public float verticalClamp = 20f;

    [Header("Footsteps")]
    public float footstepDelay = 0.5f;
    float lastFootstepAt;

    [Header("Effects")]
    public GameObject deadEffectPrefab;

    bool canTakeInput = true;
    float gravityScale;
    float moveX;
    float yVelocity;
    bool isOnIce;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        Footsteps();

        if (yVelocity > rb.velocity.y)
        {
            yVelocity = rb.velocity.y / 2;
            if (Input.GetKey(KeyCode.Space))
            {
                yVelocity = rb.velocity.y;
            }
        }
    }   

    void FixedUpdate()
    {
        Vector2 moveDir = transform.right * moveX + transform.forward;
        moveDir = Vector2.ClampMagnitude(moveDir, 1f);

        //Clamp movement
        float verticalVelocity = Mathf.Clamp(rb.velocity.y, float.MinValue, verticalClamp);

        if (isOnIce)
        {
            rb.AddForce(new Vector2( moveX * iceMultiplier, 0),ForceMode2D.Force);
        }
        else if (!isOnIce)
        {
            //Add force
            rb.AddForce(moveDir, ForceMode2D.Impulse);
        
             Vector2 horizontalVelocity = new Vector2(moveDir.x * moveSpeed, verticalVelocity);
            rb.velocity = horizontalVelocity;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            Die();
        }
        else if (collision.gameObject.tag == "Trampoline")
        {
            rb.AddForce(transform.up * -yVelocity, ForceMode2D.Impulse);
        }
        else if (collision.gameObject.tag != "Ice" && collision.gameObject.tag != "Bounds")
        {
            isOnIce = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Goal")
        {
            //Play audio
            AudioManager.audioMan.PlayCompleteSound();

            LevelAnimation(false);
        }
        else if (collision.gameObject.tag == "Ice")
        {
            isOnIce = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Gravity")
        {
            if (rb.gravityScale == gravityScale)
                rb.gravityScale = -gravityScale;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Gravity")
        {
            rb.gravityScale = gravityScale;
        }
        else if (collision.gameObject.tag == "Ice" && rb.velocity.y == 0) 
        {
            isOnIce = false;
        }
    }
    void GetInput()
    {
        //Horizontal movement
        if (canTakeInput)
        {
            moveX = 0;
            if (Input.GetKey(KeyCode.D)) moveX += 1;
            if (Input.GetKey(KeyCode.A)) moveX -= 1;
        }

        if (IsGrounded())
        {
            yVelocity = 0;

            //Jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

                //Play audio
                AudioManager.audioMan.PlayJumpSound();

                //Jump animation
                anim.SetTrigger("Jump");
            }
        }
    }

    void Footsteps()
    {
        if (IsGrounded() && moveX != 0)
        {
            if (Time.time >= lastFootstepAt + footstepDelay)
            {
                lastFootstepAt = Time.time;

                AudioManager.audioMan.PlayFootstepSound(0.5f);
            }
        }
    }

    void Die()
    {
        //Instantiate effect
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        GameObject deadEffect = Instantiate(deadEffectPrefab, spawnPos, Quaternion.identity);
        Destroy(deadEffect, 1f);

        //Play audio
        AudioManager.audioMan.PlayDeathSound();

        //Reload scene
        FindObjectOfType<GameManager>().LoadLevel(1f, true);

        //Destroy player & mouselook
        PlatformMaker platformMaker = GetComponent<PlatformMaker>();
        if (platformMaker != null) platformMaker.DestroyTarget();
        Destroy(gameObject);
    }

    public void LevelAnimation(bool moveIn)
    {
        StartCoroutine(MoveInOrOut(moveIn));
    }

    IEnumerator MoveInOrOut(bool moveIn)
    {
        //Disable input & physics
        canTakeInput = false;
        rb.simulated = false;

        //Setup position when moving in to level
        if (moveIn)
        {
            Vector3 moveInPos = GameManager.game.spawnPosition;
            moveInPos.x -= 3f;

            transform.position = moveInPos;

            //Move to start position
            while (transform.position != GameManager.game.spawnPosition)
            {
                moveX = 1f; //For animations
                transform.position = Vector3.MoveTowards(transform.position, GameManager.game.spawnPosition, moveSpeed * Time.deltaTime);

                yield return null;
            }
        }
        else
        {
            //Move to end position
            while (transform.position != GameManager.game.endPosition)
            {
                moveX = 1f; //For animations
                transform.position = Vector3.MoveTowards(transform.position, GameManager.game.endPosition, moveSpeed * Time.deltaTime);

                yield return null;
            }
        }
        
        //Finished moving
        if (moveIn)
        {
            rb.simulated = true;
            canTakeInput = true;
        }
        else //Load next level
        {
            FindObjectOfType<GameManager>().LoadLevel(0, false);
        }
    }

    public bool IsGrounded()
    {
        //Make custom filter
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(groundLayer | LayerMask.GetMask("FadeBlock"));
        contactFilter.useTriggers = false;

        List<Collider2D> results = new List<Collider2D>();

        //If it collides anything return true
        int collisionCount = Physics2D.OverlapBox(groundCheck.transform.position, groundCheck.size, 0, contactFilter, results);
        if (collisionCount > 0)
            return true;

        return false;
    }

    public float GetMoveX()
    {
        return moveX;
    }
}   
