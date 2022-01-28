using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StaticBlock : MonoBehaviour
{
    Vector3 lastPos, lastScale;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
            Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (lastPos == Vector3.zero || lastScale == Vector3.zero)
            return;

        if (transform.localScale != lastScale || transform.position != lastPos)
        {
            //Snap scale
            Vector3 scale = transform.localScale;
            scale.x = Mathf.RoundToInt(scale.x);
            scale.y = Mathf.RoundToInt(scale.y);
            scale.z = 1;

            transform.localScale = scale;

            //Check offset for position
            float xOffset = (scale.x % 2 == 0) ? 1f : 0.5f;
            float yOffset = (scale.y % 2 == 0) ? 1f : 0.5f;

            //Snap position
            Vector3 position = transform.position;
            position.x = Mathf.RoundToInt(position.x) + xOffset;
            position.y = Mathf.RoundToInt(position.y) - yOffset;
            position.z = 0;

            transform.position = position;
        }
    }

    void LateUpdate()
    {
        lastPos = transform.position;
        lastScale = transform.localScale;
    }
}
