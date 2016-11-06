using UnityEngine;
using System.Collections;
using System;

public class IsometricView : MonoBehaviour
{
    private new SpriteRenderer renderer;

    void Awake()
    {
        this.renderer = GetComponent<SpriteRenderer>();
        if (this.renderer == null)
            throw new NullReferenceException("IsometricView could not find the sprite renderer");

        if (this.renderer.sortingLayerName != "Isometric")
            throw new InvalidOperationException("IsometricView only works on sprites in the Isometric layer");
    }

    void Update()
    {
        int ypos = Mathf.FloorToInt(this.transform.position.y);
        this.renderer.sortingOrder = -ypos;
    }
}
