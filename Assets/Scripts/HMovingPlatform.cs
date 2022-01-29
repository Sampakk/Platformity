using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMovingPlatform : MonoBehaviour
{

    public float maxX;
    public float minX;
    private float platformPosition;
    private int direction = 1;
    public float speed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    void MoveRight()
    {
        rb.velocity = Vector2.right * speed;
    }

    void MoveLeft()
    {
        rb.velocity = Vector2.left * speed;
    }
}