using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //单例
    public static PlayerControl _player;
   private void Awake()
    {
        _player = this;
    }
    // Start is called before the first frame update

    Ray ray;
    RaycastHit hit;

    Vector3 StartPos;
    Vector3 EndPos;

    List<GameObject> selectObjList;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RayDetection();
            StartPos = hit.point;

            if (Input.GetMouseButtonUp(0))
            {
                RayDetection();
                EndPos = hit.point;
            }
        }
        
    }




    public void RayDetection()      //射线检测
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //摄像机发出射线
        Physics.Raycast(ray, out hit);
    }
}
