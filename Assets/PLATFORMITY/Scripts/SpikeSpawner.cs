using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public float spawnInterval = 1;
    public GameObject fallingSpike;
    float timer;
    public enum direction {up,down,left,right};
    public direction shootDirection;
    Quaternion rot;

    Vector3 downPos;
    Vector3 upPos;
    Vector3 leftPos;
    Vector3 rightPos;

    // Start is called before the first frame update
    void Start()
    {


        timer = spawnInterval;

        downPos = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y -1.5f, gameObject.transform.localPosition.z);
        upPos = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + 1.5f, gameObject.transform.localPosition.z);
        leftPos = new Vector3(gameObject.transform.localPosition.x - 1.5f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
        rightPos = new Vector3(gameObject.transform.localPosition.x + 1.5f, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if(shootDirection == direction.up)
            {
                rot.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z +180);
                Instantiate(fallingSpike, upPos, rot, transform);
            }
            else if(shootDirection == direction.down)
            {
                Instantiate(fallingSpike, downPos, Quaternion.identity, transform);
            }
            else if (shootDirection == direction.left)
            {
                rot.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 270);
                Instantiate(fallingSpike, leftPos, rot, transform);
            }
            else if (shootDirection == direction.right)
            {
                rot.eulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + 90);
                Instantiate(fallingSpike, rightPos, rot, transform);
            }

            timer = spawnInterval;
        }
    }
}
