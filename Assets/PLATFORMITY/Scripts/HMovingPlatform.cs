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
        rb = GetComponent<Rigidbody2D>(); 
        direction = StartDirection;
    }

    void FixedUpdate()
    {
        player = FindObjectOfType<PlayerMovement>();
        if(player != null) playerRB = player.gameObject.GetComponent<Rigidbody2D>();

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
        if(collision.tag == "Player") playerRB.AddForce(rb.velocity * platformMultiplier);
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