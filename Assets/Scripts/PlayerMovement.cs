using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;

    public GameObject deadEffectPrefab;
    public float moveSpeed = 4f;
    public float jumpHeight = 3f; 
    public BoxCollider2D groundCheck;
    public LayerMask groundLayer;
    public float groundDistance = 0.45f;

    bool canTakeInput = true;
    float moveX;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            Die();
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
    }

    void GetInput()
    {
        if (canTakeInput)
        {
            moveX = Input.GetAxisRaw("Horizontal");

            if (IsGrounded())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);

                    //Play audio
                    AudioManager.audioMan.PlayJumpSound();
                }
            }
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

    bool IsGrounded()
    {
        //Make custom filter
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(groundLayer);
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
