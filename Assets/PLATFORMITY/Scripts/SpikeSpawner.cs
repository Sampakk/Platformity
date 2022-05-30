using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public float spawnInterval = 1;
    public GameObject fallingSpike;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Instantiate(fallingSpike, new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 1.5f, gameObject.transform.localPosition.z), Quaternion.identity);
            timer = spawnInterval;

        }
    }
}
