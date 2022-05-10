using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float hoverRange = 0.5f;
    public float hoverSpeed = 2f;

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * hoverSpeed) * (hoverRange / 2f);
        transform.position = new Vector3(transform.position.x, startPos.y + yOffset, transform.position.z);
    }
}
