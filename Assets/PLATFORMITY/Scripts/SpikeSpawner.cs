using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public float spawnInterval = 1;
    public GameObject fallingSpike; 
    public enum direction {up,down,left,right};
    public direction shootDirection;
    public bool Moving;

    float timer;
    Quaternion rot;
    Vector3 downPos;
    Vector3 upPos;
    Vector3 leftPos;
    Vector3 rightPos;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval;

        downPos = new Vector3(transform.localPosition.x, transform.localPosition.y -1.5f, transform.localPosition.z);
        upPos = new Vector3(transform.localPosition.x, transform.localPosition.y + 1.5f, transform.localPosition.z);
        leftPos = new Vector3(transform.localPosition.x - 1.5f, transform.localPosition.y, transform.localPosition.z);
        rightPos = new Vector3(transform.localPosition.x + 1.5f, transform.localPosition.y, transform.localPosition.z);

        //Rotate to look towards shoot direction
        if (shootDirection == direction.up) //Upwards
            transform.localRotation = Quaternion.Euler(0, 0, 180f);
        else if (shootDirection == direction.right) //Right
            transform.localRotation = Quaternion.Euler(0, 0, 90f);
        else if (shootDirection == direction.left) //Left
            transform.localRotation = Quaternion.Euler(0, 0, 270f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            downPos = new Vector3(transform.localPosition.x, transform.localPosition.y - 1.5f, transform.localPosition.z);
            upPos = new Vector3(transform.localPosition.x, transform.localPosition.y + 1.5f, transform.localPosition.z);
            leftPos = new Vector3(transform.localPosition.x - 1.5f, transform.localPosition.y, transform.localPosition.z);
            rightPos = new Vector3(transform.localPosition.x + 1.5f, transform.localPosition.y, transform.localPosition.z);
        }

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
