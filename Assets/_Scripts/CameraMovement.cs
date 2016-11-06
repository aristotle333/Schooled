using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    public static CameraMovement S;
    public float xAxisValue;
    public float yAxisValue;

    // Camera Boundaries
    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;

    public float speedLag = 0.2f;

    public float ExplosionMinSize;
    public float ExplosionMaxSize;

    public float ZoomCameraSize;
    public Vector3 ZoomPosition;

    public bool Zoomed;

    private Camera cameraRef;
    private float defaultSize = 10;
    private Vector3 lastPosition;

    void Awake()
    {
        S = this;
        cameraRef = this.GetComponent<Camera>();
        lastPosition = this.transform.position;
    }

    void Update()
    {
        xAxisValue = Input.GetAxis("Horizontal");
        yAxisValue = Input.GetAxis("Vertical");
        if (Camera.main != null && !Zoomed)
        {
            Vector3 OriginalLocation = Camera.main.transform.position;
            Camera.main.transform.Translate(new Vector3(xAxisValue * speedLag, yAxisValue * speedLag, 0.0f));

            Vector3 cameraPos = Camera.main.transform.position;
            if (cameraPos.x >= xMax || cameraPos.x <= xMin || cameraPos.y <= yMin || cameraPos.y >= yMax)
            {
                Camera.main.transform.position = OriginalLocation;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeCameraSize();
        }
        if (!Zoomed)
        {
            lastPosition = this.transform.position;
        }
    }

    void changeCameraSize()
    {
        if (Zoomed)
        {
            Zoomed = false;
            cameraRef.orthographicSize = defaultSize;
            this.transform.position = lastPosition;
        }
        else
        {
            Zoomed = true;
            cameraRef.orthographicSize = ZoomCameraSize;
            this.transform.position = ZoomPosition;
        }
    }

}