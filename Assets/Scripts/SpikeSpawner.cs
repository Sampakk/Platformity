using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeSpawner : MonoBehaviour
{
    public float spawnInterval = 1;
    public GameObject fallingSpike;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        spawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0)
        {
            Instantiate(fallingSpike, new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 1.5f, gameObject.transform.localPosition.z), Quaternion.identity);
            spawnInterval = 2;

        }
    }
}
