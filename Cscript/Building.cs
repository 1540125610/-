using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private bool isBuilt;     //是否建造成功
    private Vector3 currentPosition;     //跟随鼠标位置
    public GameObject judgeArea;     //判定区域
    private BuildingJudge buildingJudge;
    private ResourceSystem resourceManager;    //矿石数
    public int cost = 50;     //建造消耗矿石
    void Start()
    {
        isBuilt = false;
        buildingJudge = judgeArea.GetComponent<BuildingJudge>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceSystem>();
    }


    void Update()
    {
        if (!isBuilt)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            //建组必在4个网格中间
            float posX = (((int)hit.point.x) / 5) * 5 ;
            float posY = -1;
            float posZ = (((int)hit.point.z) / 5) * 5 ;
            
            currentPosition = new Vector3(posX, posY, posZ);
            transform.position = currentPosition;
        }
        if (Input.GetMouseButtonDown(0) && buildingJudge.canBuild && !isBuilt)
        {
            isBuilt = true;
            buildingJudge.outBuilding();
            judgeArea.GetComponent<Renderer>().enabled = false;
            resourceManager.minerals -= cost;
        }
        if (Input.GetMouseButtonDown(1) && !isBuilt)
        {
            buildingJudge.outBuilding();
            Destroy(gameObject);
        }
    }
}
