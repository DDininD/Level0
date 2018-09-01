using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Bird bird;
    
    [HideInInspector]
    public Vector3 o_pos;
    Camera myCamera;
    private Rigidbody cameraRigi;

    void Awake()
    {
        //bird = FindObjectOfType<Bird>();
        myCamera = GetComponent<Camera>();
        o_pos = transform.position;
        Debug.Log(myCamera.scaledPixelWidth);
        cameraRigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        bird = FindObjectOfType<Bird>();
        if (bird != null)
        {
            cameraRigi.velocity = bird.GetComponent<Rigidbody2D>().velocity;
        }
    }

    public void ResetCamera()
    {
        Invoke("ResetCameraO", 3.0f);
    }

    void ResetCameraO()
    {
        transform.position = o_pos;
        FindObjectOfType<FixCamera>().enabled = enabled;
        enabled = false;
    }

}

