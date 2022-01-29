using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMovingPlatform : MonoBehaviour
{

    public float maxY;
    public float minY;
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

        platformPosition = rb.transform.position.y;

        if (platformPosition >= maxY)
            direction = 0;
        if (platformPosition <= minY)
            direction = 1;


        if (direction == 1)
            MoveUp();
        else
            MoveDown();

    }

    void MoveUp()
    {
        rb.velocity = Vector2.up * speed;
    }

    void MoveDown()
    {
        rb.velocity = Vector2.down * speed;
    }
}