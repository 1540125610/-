using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 cameraPosition;
    private Camera c;
    private float mouseWheel;
    private int maxView = 30;
    private int minView = 3;
    private float slideSpeed = 30;
    void Start()
    {
        c = GetComponent<Camera>();
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            startPosition = Input.mousePosition;
            cameraPosition = transform.position;
        }
        if(Input.GetMouseButton(2))
        {
            float x = (Input.mousePosition.x - startPosition.x)/20;
            float z = (Input.mousePosition.y - startPosition.y)/20;
            transform.position = new Vector3(cameraPosition.x - x, cameraPosition.y, cameraPosition.z - z);
        }

        mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if(mouseWheel<0)
        {
            if(c.fieldOfView<=maxView)
            {
                c.fieldOfView += slideSpeed * Time.deltaTime;
                if(c.fieldOfView>maxView)
                {
                    c.fieldOfView = maxView;
                }
            }

        }
        else if(mouseWheel>0)
        {
            if (c.fieldOfView >= minView)
            {
                c.fieldOfView -= slideSpeed * Time.deltaTime;
                if (c.fieldOfView < minView)
                {
                    c.fieldOfView = minView;
                }
            }
        }
    }
}
