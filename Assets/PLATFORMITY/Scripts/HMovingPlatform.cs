using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMovingPlatform : MonoBehaviour
{

    PlayerMovement player;
    Rigidbody2D playerRB;
    public int platformMultiplier;

    public float maxX;
    public float minX;
    private float platformPosition;
    private int direction;
    public int StartDirection;
    public float speed;
    Rigidbody2D rb;

    void Start()
    {

        player = FindObjectOfType<PlayerMovement>();
        playerRB = player.gameObject.GetComponent<Rigidbody2D>();

        rb = GetComponent<Rigidbody2D>(); 
        direction = StartDirection;
    }

    void FixedUpdate()
    {
        platformPosition = rb.transform.position.x;
        

        if (platformPosition >= maxX)
            direction = 0;
        if (platformPosition <= minX)
            direction = 1;


        if (direction == 1)
            MoveRight();
        else
            MoveLeft();

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        playerRB.AddForce(rb.velocity * platformMultiplier);
    }

    void MoveRight()
    {
        rb.velocity = Vector2.right * speed;
    }

    void MoveLeft()
    {
        rb.velocity = Vector2.left * speed;
    }
}