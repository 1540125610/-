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

    Ray ray;            //射线
    RaycastHit hit;     //射线检测结果

    Vector3 startPos;   //鼠标左键按下位置
    Vector3 endPos;     //鼠标左键抬起位置


    string playerName;   //玩家名字
    GameManage gameManage;              //总信息类
    GameInfo playerInfo;                //玩家信息
    int playerNum;                      //玩家编号

    public GameObject selectBox;        //选框



    List<GameObject> chosenObj = new List<GameObject>();     //被选中的单位队列

    

    List<GameObject> selectObjList;
    void Start()
    {
        
        playerName = "Player1";

        gameManage = transform.parent.gameObject.GetComponent<GameManage>();    //获取总信息类


        //查找玩家在总玩家里的编号
        int num =0;
        foreach (GameInfo a in gameManage.playersInfos)
        {
            if(a.Name == playerName)
            {
                playerNum = num;
                break;                  //找到了就跳出
            }
            num++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AutoRefresh();      //一直刷新玩家信息

        //框选
        if (Input.GetMouseButtonDown(0))        //按下左键
        {
            startPos = Input.mousePosition;
            
            selectBox.SetActive(true);
        }
        if (Input.GetMouseButton(0))            //长按左键框选
        {
            
            float x = Input.mousePosition.x - startPos.x;
            float y = Input.mousePosition.y - startPos.y;
            selectBox.transform.localPosition = new Vector3(x/2, y/2, 0);
            selectBox.transform.localScale = new Vector3(x, y, 1);
        }
        if (Input.GetMouseButtonUp(0))          //抬起左键
        {
            selectBox.SetActive(false);
            endPos = Input.mousePosition;
            Checkbox();
        }
        
    }

    //框选函数
    private void Checkbox()
    {
        Vector3 lowerLeftPos = new Vector3(Mathf.Min(startPos.x, endPos.x), Mathf.Min(startPos.y, endPos.y), 0);        //框左下角点
        Vector3 upperRightPos = new Vector3(Mathf.Max(startPos.x, endPos.x), Mathf.Max(startPos.y, endPos.y), 0);        //框右上角点

        ChosenObjClaer();              //先清理掉已经选着的东西
        foreach (GameObject unit in playerInfo.MilitaryUnits)
        {
            Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);            //作战单位的世界坐标转屏幕坐标
            if (unitScreenPos.x > lowerLeftPos.x && unitScreenPos.y > lowerLeftPos.y && unitScreenPos.x< upperRightPos.x && unitScreenPos.y < upperRightPos.y)          //是否在框选范围内
            {
                chosenObj.Add(unit);
            }
        }
        OnChosenObj();                  //开启选择框
    }

    //射线检测
    public void RayDetection()      
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);        //摄像机发出射线
        Physics.Raycast(ray, out hit);
    }

    //刷新玩家信息
    private void AutoRefresh()
    {
        playerInfo = gameManage.playersInfos[playerNum];
    }

    
    private void ChosenObjClaer()
    {
        foreach(GameObject a in chosenObj)
        {
            a.GetComponent<HumanControl>().OffSelected();       //关闭选择框
        }
        chosenObj.Clear();
    }


    //框选单位添加完毕
    private void OnChosenObj()
    {
        foreach (GameObject a in chosenObj)
        {
            a.GetComponent<HumanControl>().OnSelected();       //开启选择框
        }
    }
}
