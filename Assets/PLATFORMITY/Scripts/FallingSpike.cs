using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    SpikeSpawner spawner;
    public float speed = 0.05f;

    void Start()
    {
        spawner = GetComponentInParent<SpikeSpawner>();
    }

    void Update()
    {
        if(spawner.shootDirection == SpikeSpawner.direction.up)
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        else if(spawner.shootDirection == SpikeSpawner.direction.down)
        {
            transform.position += new Vector3(0, -speed * Time.deltaTime, 0);
        }
        else if (spawner.shootDirection == SpikeSpawner.direction.left)
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0) ;
        }
        else if (spawner.shootDirection == SpikeSpawner.direction.right)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
