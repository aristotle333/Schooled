using UnityEngine;
using System.Collections;

public class explosionAOE : MonoBehaviour
{

    public int framesToExpand = 40;          // This should be the same as the frames it takes the AOE projectile collider to expand
    public float minSize = 0.1f;
    public float maxSize = 0.8f;

    private float minSizeCamera;
    private float maxSizeCamera;

    private int frameCounter = 0;
    private Vector3 vecIncrement;

    void Start()
    {
        if (CameraMovement.S.Zoomed)
        {
            float newMinSize = CameraMovement.S.ExplosionMinSize;
            float newMaxSize = CameraMovement.S.ExplosionMaxSize;
            this.transform.localScale = new Vector3(newMinSize, newMinSize, newMinSize);
            float incrementTemp = (newMaxSize - newMinSize) / (float)framesToExpand;
            vecIncrement = new Vector3(incrementTemp, incrementTemp, incrementTemp);
        }
        else
        {
            float incrementTemp = (maxSize - minSize) / (float)framesToExpand;
            vecIncrement = new Vector3(incrementTemp, incrementTemp, incrementTemp);
        }
    }

    void Update()
    {
        ++frameCounter;
        this.transform.localScale += vecIncrement;
        if (frameCounter >= framesToExpand)
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayExplosionSound()
    {
        this.GetComponent<AudioSource>().Play();
    }



}