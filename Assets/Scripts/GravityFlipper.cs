using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityFlipper : MonoBehaviour
{
    public float gravityScale = 3;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        rb = collision.attachedRigidbody;
    
        Debug.Log(collision.attachedRigidbody.gravityScale);
        if (collision.gameObject.tag == "Player")
        {
            rb.gravityScale = -gravityScale;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.attachedRigidbody.gravityScale = gravityScale;
        }
    }
}
