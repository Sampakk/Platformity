using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBlocker : MonoBehaviour
{
    float gravScale = 3;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        
    }

}
