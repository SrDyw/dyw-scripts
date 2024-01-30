using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth2D : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float precision = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = (int)(-transform.position.y * precision);

        for(int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>() == null) continue;
                transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y * precision) + (i + 1);
        }
    }
}
