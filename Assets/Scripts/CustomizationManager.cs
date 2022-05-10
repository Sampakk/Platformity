using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    PlayerMovement player;

    [Header("Hat")]
    public Transform hatRoot;
    public float moveRange = 0.2f;
    public float moveSpeed = 2f;

    Vector3 hatPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();

        hatPos = hatRoot.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Animations();
    }

    void Animations()
    {
        float moveX = player.GetMoveX();

        //Left and right turning
        float maxAngle = 5f;

        Vector3 localEulers = transform.localEulerAngles;
        localEulers.z = -(moveX * maxAngle);
        transform.localRotation = Quaternion.Euler(localEulers);

        //Animate hat
        if (player.GetMoveX() != 0) //Moving
        {
            float yOffset = Mathf.Sin(Time.time * moveSpeed) * (moveRange / 2f);
            hatRoot.localPosition = new Vector3(hatPos.x, hatPos.y + yOffset, hatPos.z);
        }
        else //Staying still
        {
            hatRoot.localPosition = hatPos;
        }
    }
}
