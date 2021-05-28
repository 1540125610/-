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

    GameObject parent;              //父对象
    GridsControl gridsControl;      //网格总控制器


    List<GridScript> grids;                 //当前碰撞的网格

    void Start()
    {
        grids = new List<GridScript>();

        isBuilt = false;
        buildingJudge = judgeArea.GetComponent<BuildingJudge>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceSystem>();
        parent = GameObject.Find("Build Units/Player1");
        gridsControl = GameObject.Find("Grids Control").GetComponent<GridsControl>();
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

            transform.parent = parent.transform;      //修改自己的父对象

            GetGrids();     //获取被占据网格
            TellToGrids();  //通知被占据网格关闭自身

            gridsControl.StartRenovate();       //刷新地图
        }
        if (Input.GetMouseButtonDown(1) && !isBuilt)
        {
            buildingJudge.outBuilding();
            Destroy(gameObject);
        }
    }

    //获取占据网格
    private void GetGrids()
    {
        foreach(Collider a in judgeArea.GetComponent<BuildingJudge>().gridsCollider)
        {
            GridScript b = a.GetComponent<GridScript>();
            grids.Add(b);
        }
    }

    //通知网格关闭自己
    public void TellToGrids()
    {
        foreach(GridScript a in grids)
        {
            a.OnAppropriat();       //关闭该通道
        }
    }
}
