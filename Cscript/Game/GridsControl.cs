using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GridsControl : MonoBehaviour
{
    public GameObject grid;             //网格实体
    public GameObject Grids;            //网格汇总的父物体
    public GridScript[] gridScripts;    //所有网格的脚本

    public struct objGo 
    {
        public bool used;               //该位置是否被使用
        public List<GameObject> Objects;        //应该移动的物体
        public GridScript aimGrid;              //目标网格
    }
    objGo[] objsGo = new objGo[100];           //默认100个导航地图

    private void Awake()
    {

        gridScripts = new GridScript[400];     //2500个格子

        int x, y;                               //x：行，y列 左下为原点
        for (int i = 0; i < 400; i++)
        {
            x = i % 20;
            y = i / 20;
            Vector3 pos = new Vector3(-47.5f + x * 5, -1, -47.5f + y * 5);  //从左下开始创建 格子大小为5*5

            GameObject newGrid = GameObject.Instantiate(grid, pos, Quaternion.identity);

            newGrid.name = "Grid" + y + "-" + x;      //名字
            newGrid.transform.parent = Grids.transform;     //移动到总网格下

            gridScripts[i] = newGrid.GetComponent<GridScript>();        //将其脚本列入脚本汇总
        }
    }

    void Start()
    {
        
    }

    //所有选中单位前往某个位置
    public void ToAim(Vector3 aimPoint,List<GameObject> chosenObj)
    {
        GridScript aim = PosToGrid(aimPoint);            //获得网格
        GridAI(aim, chosenObj, aimPoint);               //开始寻路
    }


    //获得地面点的网格
    private GridScript PosToGrid(Vector3 pos)
    {
        Collider[] collidedObj = Physics.OverlapSphere(pos, 0.01f, 1 << 10);      //创建球型碰撞(位置，大小，层数)
        GridScript aim = collidedObj[0].GetComponent<GridScript>();             //获得网格上的脚本
        return aim;                                                             //返回脚本
    }


    //寻路
    private void GridAI(GridScript newAimGrid, List<GameObject> chosenObj,Vector3 aimPoint)            //（网格，单位，目的点）
    {
        //先查找是否为存在同一地图
        foreach (objGo a in objsGo)
        {
            if(a.aimGrid == newAimGrid)        //存在以该点为终点的导航地图时
            {
                int index = Array.IndexOf(objsGo, a);           //获得当前位置下标

                foreach (GameObject obj in chosenObj)
                {
                    
                    if (a.Objects.Contains(obj) )        //该单位已经存在于当前存储中
                    {

                    }
                    else                                //如果不存在，则添加该单位进入存储
                    {
                        a.Objects.Add(obj);             //添加
                                                   //告知单位进行移动
                    }
                }
                return;                     //退出函数
            }
        }
        for (int i=0;i<objsGo.Length;i++ )                        //非同一张图
        {
            if (!objsGo[i].used)                                 //未被使用的位置
            {
                //保存数值
                objsGo[i].used = true;
                objsGo[i].Objects = chosenObj;
                objsGo[i].aimGrid = newAimGrid;

                //将路传递给目标方格
                objsGo[i].aimGrid.MapStart(i);

                

                //让每个单位开始移动
                for(int j = chosenObj.Count -1;j>=0;j--)
                {
                    int mapindex = chosenObj[j].GetComponent<HumanControl>().SetMove(aimPoint, i);     //获取原地址且改变单位地址
                    DeleteObj(chosenObj[j], mapindex);
                }


                return;
            }
        }
    }

    //将莫一个单位从地图导航中剔除
    public void DeleteObj(GameObject obj,int index)
    {
        if (index == -1)   //当非导航时，退出
        {
            return;
        }

        int i = objsGo[index].Objects.IndexOf(obj);
        objsGo[index].Objects.RemoveAt(i);          


        if (objsGo[index].Objects.Count == 0)       //当该地图没有物体在原地址使用时，清空该地图
        {
            objsGo[index].used = false;
            objsGo[index].aimGrid = null;
        }
    }
}
