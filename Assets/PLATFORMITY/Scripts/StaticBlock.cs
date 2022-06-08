using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class StaticBlock : MonoBehaviour
{
    BoxCollider2D[] colliders;
    SpriteRenderer sprite;

    [Header("")]
    public bool repeatTexture = false;
    [Range(1, 100)]
    public int width = 1;
    [Range(1, 100)]
    public int height = 1;

    Vector3 lastPos, lastScale;
    int lastWidth, lastHeight;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.isPlaying)
            Destroy(this);

        //Get components
        colliders = GetComponents<BoxCollider2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();

        //Setup sprite
        sprite.drawMode = SpriteDrawMode.Tiled;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliders.Length == 0) //Get colliders if null
        {
            colliders = GetComponents<BoxCollider2D>();
        }

        if (sprite == null) //Get sprite if null
        {
            sprite = GetComponentInChildren<SpriteRenderer>();
            sprite.drawMode = SpriteDrawMode.Tiled;
        }            

        if (repeatTexture) //Texture repeating
        {
            if (lastPos == Vector3.zero || lastScale == Vector3.zero)
                return;

            if (transform.position != lastPos || width != lastWidth || height != lastHeight)
            {
                //Force scale
                transform.localScale = Vector3.one;

                //Update sprite repeating
                sprite.size = new Vector2(width, height);

                //Update colliders size
                foreach (BoxCollider2D collider in colliders)
                    collider.size = new Vector2(width, height);

                //Check offset for position
                float xOffset = (width % 2 == 0) ? 1f : 0.5f;
                float yOffset = (height % 2 == 0) ? 1f : 0.5f;

                //Snap position
                Vector3 position = transform.position;
                position.x = Mathf.RoundToInt(position.x) + xOffset;
                position.y = Mathf.RoundToInt(position.y) - yOffset;
                position.z = 0;

                transform.position = position;
            }
        }
        else //No repeat, affect scale
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
    }

    void LateUpdate()
    {
        lastPos = transform.position;
        lastScale = transform.localScale;

        lastWidth = width;
        lastHeight = height;
    }
}
