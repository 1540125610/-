using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public bool isAppropriat;           //是否被占用
    public GridScript[] grids;          //周围脚本
    public bool[] move;                 //是否可以向周围移动(下标表示上方的周围脚本)
    public GridsControl gridsControl;               //格子汇总信息

    public int hinderNum =1;            //阻碍系数

    //地图数据
    public struct mapGrid
    {
        public int direction;                  //方向
        public int allHinderNum;               //总阻碍系数
    }
    public mapGrid[] maps;                     //地图数据

    // Start is called before the first frame update
    void Start()
    {
        isAppropriat = false;           //初始未被占用

        grids = new GridScript[10];     //周围脚本(0不使用，1为左下，9为右上)
        move = new bool[10];

        gridsControl = GameObject.Find("Grids Control").GetComponent<GridsControl>();
        GetOtherGrid();

        maps = new mapGrid[1000];       //预留1k个地图
}

    // Update is called once per frame
    void Update()
    {

        
    }

    private void GetOtherGrid()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = transform.position.z;
        foreach (GridScript a in gridsControl.gridScripts)
        {
            Vector3 aPos = a.transform.position;

            if(Mathf.Abs(aPos.x-posX)>2 || Mathf.Abs(aPos.z - posZ) > 2)    //不是周围的直接跳过
            {
                continue;
            }


            //左下 1
            if(aPos.x == posX-1*2 && aPos.y == posY && aPos.z ==posZ-1*2)
            {
                grids[1] = a;
                move[1] = true;
            }
            //下  2
            else if (aPos.x == posX && aPos.y == posY && aPos.z == posZ - 1*2)
            {
                grids[2] = a;
                move[2] = true;
            }
            //右下 3
            else if (aPos.x == posX + 1*2 && aPos.y == posY && aPos.z == posZ - 1*2)
            {
                grids[3] = a;
                move[3] = true;
            }
            //左  4
            else if (aPos.x == posX - 1*2 && aPos.y == posY && aPos.z == posZ)
            {
                grids[4] = a;
                move[4] = true;
            }
            //自己 5
            else if (aPos.x == posX && aPos.y == posY && aPos.z == posZ)
            {
                grids[5] = a;
                move[5] = true;
            }
            //右  6
            else if (aPos.x == posX + 1*2 && aPos.y == posY && aPos.z == posZ)
            {
                grids[6] = a;
                move[6] = true;
            }
            //左上 7
            else if (aPos.x == posX - 1*2 && aPos.y == posY && aPos.z == posZ + 1*2)
            {
                grids[7] = a;
                move[7] = true;
            }
            //上 8
            else if (aPos.x == posX && aPos.y == posY && aPos.z == posZ + 1*2)
            {
                grids[8] = a;
                move[8] = true;
            }
            //右上 9
            else if (aPos.x == posX + 1*2 && aPos.y == posY && aPos.z == posZ + 1*2)
            {
                grids[9] = a;
                move[9] = true;
            }

        }
    }

    //被占用
    public void OnAppropriat()
    {
        isAppropriat = true;
        for(int a = 0; a < 10; a++)
        {
            if(grids[a] != null)
            {
                move[a] = false;
            }
        }
    }

    //未被占用
    public void OffOnAppropriat()
    {
        isAppropriat = false;
        for (int a = 0; a < 10; a++)
        {
            if (grids[a] != null)
            {
                move[a] = true;
            }
        }
    }

    //地图开始编写
    public void MapStart(int index)
    {
        maps[index].direction = 5;          //该方格，位置不动
        maps[index].allHinderNum = 0;       //该方格的总系数为0

        NextMap(5, maps[index].allHinderNum, index);
    }


    //传递给下一个网格
    public void NextMap(int dir,int nowHinderNum,int mapNum)
    {

        nowHinderNum += hinderNum;                              //更新总系数
        if(maps[mapNum].allHinderNum == 0)
        {

        }
        else if (nowHinderNum > maps[mapNum].allHinderNum)           //当本身总系数低于传递的数组，保留原系数
        {
            return;
        }


        //确定为最低路线时，更新信息
        maps[mapNum].allHinderNum = nowHinderNum;
        maps[mapNum].direction = 10 - dir;
        

        for (int i=1; i < 10; i++)
        {
            if(i == dir || i == 5)      //为自身或者来源方向时，跳过
            {
                continue;
            }
            if (move[i])            //可以移动的方向
            {
                grids[i].NextMap(i, maps[mapNum].allHinderNum, mapNum);
            }
        }
    }
}
