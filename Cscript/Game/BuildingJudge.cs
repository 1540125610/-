using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingJudge : MonoBehaviour
{
    public Material green;
    public Material red;
    public bool canBuild = true;   //是否可以建造
    private Renderer rd;
    public Building building;
    private ResourceSystem resourceManager;    //矿石数
    private PlayerControl playerControl;

    List<GridScript> grids;                 //当前碰撞的网格
    List<Collider> gridsCollider;           //保存网格碰撞器
    void Start()
    {
        gridsCollider = new List<Collider>();

        rd = GetComponent<Renderer>();
        resourceManager = GameObject.Find("ResourceManager").GetComponent<ResourceSystem>();
        if (resourceManager.minerals < building.cost)
        {
            canBuild = false;
        }
        playerControl = GameObject.Find("Player Control").GetComponent<PlayerControl>();
        playerControl.onBuilding = true;
    }
    void Update()
    {
        if (canBuild)
        {
            rd.material = green;
        }
        else
        {
            rd.material = red;
        }

        Debug.Log(gridsCollider.Count);
    }
    private void OnTriggerStay(Collider other)        //碰到障碍物不能建造
    {
        if (other.tag == "Military Units" || other.tag == "Build Units")
        {
            canBuild = false;
        }

        if (other.gameObject.layer == 10)           //当其为导航基点时
        {
            if (!gridsCollider.Contains(other))
            {
                gridsCollider.Add(other);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Military Units" || other.tag == "Build Units")
        {
            if (resourceManager.minerals >= building.cost)
            {
                canBuild = true;
            }
        }

        if (other.gameObject.layer == 10)           //当其为导航基点时
        {
            if (gridsCollider.Contains(other))
            {
                gridsCollider.Remove(other);
            }
        }
    }
    public void outBuilding()        //离开建造状态
    {
        playerControl.onBuilding = false;
    }
}
