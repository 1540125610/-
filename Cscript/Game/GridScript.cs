using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridScript : MonoBehaviour
{
    public bool isAppropriat;           //是否被占用
    public GridScript[] grids;          //周围脚本
    public List<int> canMove;                 //可以移动的方向
    public GridsControl gridsControl;         //格子汇总信息

    public int hinderNum =1;            //阻碍系数

    public bool isSee = false;          //是否可见
    public GameObject See;      //可见物体 

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

        gridsControl = GameObject.Find("Grids Control").GetComponent<GridsControl>();
        GetOtherGrid();

        maps = new mapGrid[1000];       //预留1k个地图

        See = transform.Find("See").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSee)
        {
            See.SetActive(true);
            See.GetComponent<TextMesh>().text = maps[0].direction.ToString();
        }
        else
        {
            See.SetActive(false);
        }
    }

    //获取周围的方格 （可以优化为按照名字进行获取）
    private void GetOtherGrid()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;
        float posZ = transform.position.z;
        foreach (GridScript a in gridsControl.gridScripts)
        {
            Vector3 aPos = a.transform.position;

            if(Mathf.Abs(aPos.x-posX)>5 || Mathf.Abs(aPos.z - posZ) > 5)    //不是周围的直接跳过
            {
                continue;
            }


            //左下 1
            if(aPos.x == posX-1*5 && aPos.y == posY && aPos.z ==posZ-1*5)
            {
                grids[1] = a;
                canMove.Add(1);
            }
            //下  2
            else if (aPos.x == posX && aPos.y == posY && aPos.z == posZ - 1*5)
            {
                grids[2] = a;
                canMove.Add(2);
            }
            //右下 3
            else if (aPos.x == posX + 1*5 && aPos.y == posY && aPos.z == posZ - 1*5)
            {
                grids[3] = a;
                canMove.Add(3);
            }
            //左  4
            else if (aPos.x == posX - 1*5 && aPos.y == posY && aPos.z == posZ)
            {
                grids[4] = a;
                canMove.Add(4);
            }
            //自己 5
            else if (aPos.x == posX && aPos.y == posY && aPos.z == posZ)
            {
                grids[5] = a;
                canMove.Add(5);
            }
            //右  6
            else if (aPos.x == posX + 1*5 && aPos.y == posY && aPos.z == posZ)
            {
                grids[6] = a;
                canMove.Add(6);
            }
            //左上 7
            else if (aPos.x == posX - 1*5 && aPos.y == posY && aPos.z == posZ + 1*5)
            {
                grids[7] = a;
                canMove.Add(7);
            }
            //上 8
            else if (aPos.x == posX && aPos.y == posY && aPos.z == posZ + 1*5)
            {
                grids[8] = a;
                canMove.Add(8);
            }
            //右上 9
            else if (aPos.x == posX + 1*5 && aPos.y == posY && aPos.z == posZ + 1*5)
            {
                grids[9] = a;
                canMove.Add(9);
            }

        }
    }

    

    //地图开始编写
    public void MapStart(int index)
    {
        maps[index].direction = 5;          //该方格，位置不动
        maps[index].allHinderNum = 0;       //该方格的总系数为0

        NextMap(0, maps[index].allHinderNum, index);
    }


    //接受上一个网格的传递
    public void GetMap(int dir,int nowHinderNum,int mapNum)
    {
        if (maps[mapNum].direction == 5)     //如果本地方向为5时，意味着为终点，不接受任何传递
        {
            return;
        }

        if(nowHinderNum < maps[mapNum].allHinderNum || maps[mapNum].allHinderNum==0)    //传递过来的值小于当前值 或者当前值为0
        {
            maps[mapNum].allHinderNum = nowHinderNum;
            maps[mapNum].direction = 10-dir;

            NextMap(maps[mapNum].direction, maps[mapNum].allHinderNum, mapNum);            //继续向外传递

        }
        else if(nowHinderNum == maps[mapNum].allHinderNum)              //当传递过来的值相等时
        {
            if (dir % 2 == 1)                                       //优先斜向移动
            {
                maps[mapNum].allHinderNum = nowHinderNum;
                maps[mapNum].direction = 10-dir;

                NextMap(maps[mapNum].direction, maps[mapNum].allHinderNum, mapNum);    //继续向外传递
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
    //向下一个网格传递值
    private void NextMap(int dir, int nowHinderNum, int mapNum)
    {
        foreach(int i in canMove)         //遍历所有通行方向
        {
            if (i !=5 && i!=dir )            //当方向不为本身且不为接收方向
            {
                if (i % 2 == 1)             //传递方向为斜向时
                {
                    grids[i].GetMap(i, maps[mapNum].allHinderNum + 1 +grids[i].hinderNum, mapNum);          //斜向+1
                }
                else                        //传递方向不为斜向时
                {
                    grids[i].GetMap(i, maps[mapNum].allHinderNum + grids[i].hinderNum, mapNum);
                }
            }
        }
    }

    //被占用(关闭周围可通行)
    public void OnAppropriat()
    {
        isAppropriat = true;
        foreach (int a in canMove)
        {
            if(!grids[a].isAppropriat)      //该目标没有被占据
            {
                grids[a].canMove.Remove(10 - a);
            }
        }
        canMove.Clear();      //清理可以移动列表
    }

    //未被占用(开启周围通行)
    public void OffAppropriat()
    {
        isAppropriat = false;
        for(int i = 1; i < 10; i++)
        {
            if(grids[i]!=null && i != 5)        //当周边网格存在且不为本身时
            {
                if(grids[i].isAppropriat == false)          //该网格未被占用
                {
                    canMove.Add(i);
                }
            }
        }
    }
}
