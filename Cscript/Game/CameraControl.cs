using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float height;
    private float width;
    private Camera camera_;
    private float mouseWheel;
    private int maxView = 30;
    private int minView = 3;
    private float slideSpeed = 30;   //镜头缩放速度
    private float moveSpeed = 30;    //镜头移动速度
    private Vector3 originalPositon;   //摄像头初始位置
    void Start()
    {
        height = UnityEngine.Screen.height;
        width = UnityEngine.Screen.width;
        camera_ = GetComponent<Camera>();
        originalPositon = transform.position;

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
            if(camera_.fieldOfView<=maxView)
            {
                camera_.fieldOfView += slideSpeed * Time.deltaTime;
                if(camera_.fieldOfView>maxView)
                {
                    camera_.fieldOfView = maxView;
                }
            }

        }
        else if(mouseWheel>0)
        {
            if (camera_.fieldOfView >= minView)
            {
                camera_.fieldOfView -= slideSpeed * Time.deltaTime;
                if (camera_.fieldOfView < minView)
                {
                    camera_.fieldOfView = minView;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))     //点击空格键回到摄像头初始位置
        {
            transform.position = originalPositon;
        }
    }
}
