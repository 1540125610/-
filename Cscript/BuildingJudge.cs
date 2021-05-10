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
    void Start()
    {

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

    }
    private void OnTriggerStay(Collider other)        //碰到障碍物不能建造
    {

        if (other.tag == "Military Units" || other.tag == "Build Units")
        {
            canBuild = false;
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
    }
    public void outBuilding()        //离开建造状态
    {
        playerControl.onBuilding = false;
    }
}
