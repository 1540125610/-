using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float height;
    private float width;
    private Camera c;
    private float mouseWheel;
    private int maxView = 30;
    private int minView = 3;
    private float slideSpeed = 30;   //镜头缩放速度
    private float moveSpeed = 30;    //镜头移动速度
    void Start()
    {
        height = UnityEngine.Screen.height;
        width = UnityEngine.Screen.width;
        c = GetComponent<Camera>();
    }

    
    void Update()
    {
        if(Input.mousePosition.x<=0)
        {
            transform.Translate(Vector3.left*Time.deltaTime*moveSpeed);
        }
        if (Input.mousePosition.x >= width-1)
        {
            transform.Translate(Vector3.right * Time.deltaTime*moveSpeed);
        }
        if (Input.mousePosition.y <= 0)
        {
            transform.Translate(Vector3.down * Time.deltaTime*moveSpeed);
        }
        if (Input.mousePosition.y >= height)
        {
            transform.Translate(Vector3.up * Time.deltaTime*moveSpeed);
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
