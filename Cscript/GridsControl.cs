using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridsControl : MonoBehaviour
{
    public GameObject grid;             //网格实体
    public GameObject Grids;            //网格汇总的父物体
    public GridScript[] gridScripts;    //所有网格的脚本

    private void Awake()
    {
        gridScripts = new GridScript[2500];     //2500个格子

        int x, y;                               //x：行，y列 左下为原点
        for (int i = 0; i < 2500; i++)
        {
            x = i%50;   
            y = i/50;
            Vector3 pos = new Vector3(-49 + x*2, -1, -49 + y*2);  //从左下开始创建 格子大小为2*2

            GameObject newGrid = GameObject.Instantiate(grid, pos, Quaternion.identity);

            newGrid.name = "Grid" + y +"-"+ x;      //名字
            newGrid.transform.parent = Grids.transform;     //移动到总网格下
                
            gridScripts[i] = newGrid.GetComponent<GridScript>();        //将其脚本列入脚本汇总
        }
    }

    void Start()
    {
        
    }
}
