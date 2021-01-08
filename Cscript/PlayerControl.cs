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



    List<GameObject> chosenObj = new List<GameObject>();            //当前选中的单位队列
    GameObject chosenOtherPlayerObj = null;                         //选到了其他玩家的单位

    

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


        //鼠标操作
        if (Input.GetMouseButtonDown(0))        //按下左键
        {
            RayDetection();
            


            //框选(按下左键)
            startPos = Input.mousePosition;
            selectBox.SetActive(true);     //开启选择框
        }
        if (Input.GetMouseButton(0))            //长按左键框选
        {
            



            //框选
            float x = Input.mousePosition.x - startPos.x;           //框的宽度
            float y = Input.mousePosition.y - startPos.y;           //框的长度

            //框的中心点：起点+框大小的一半-屏幕中心  注:input得到的屏幕坐标是以屏幕左下为原点，但selectBox的坐标是以屏幕中心为原点
            selectBox.transform.localPosition = new Vector3(x /2 + startPos.x - UnityEngine.Screen.width/2 , y /2 + startPos.y - UnityEngine.Screen.height/2, 0);
            
            selectBox.transform.localScale = new Vector3(x, y, 1);                                        //框的大小
        }
        if (Input.GetMouseButtonUp(0))          //抬起左键
        {




            //框选
            selectBox.SetActive(false);         //关闭选框
            endPos = Input.mousePosition;

            //如果是框选则开启，不是则启动单点模式
            if (endPos == startPos)                     //单点模式 
            {

                if (hit.collider.gameObject.layer == 9)   //第九层是地面
                {
                    ChosenObjClaer();
                }else if(hit.collider.gameObject.layer == 8)   //第八层是单位
                {
                    if (playerInfo.MilitaryUnits.Contains(hit.collider.gameObject) || playerInfo.BulidUnits.Contains(hit.collider.gameObject))        //是否是玩家人物单位
                    {
                        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))   //按下shift(添加单位)
                        {
                            if (!chosenObj.Contains(hit.collider.gameObject))          //不是已选择的目标
                            {
                                AddChosenObj(hit.collider.gameObject);
                            }  
                        }
                        else if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))  // 按下ctrl(剔除单位)
                        {
                            if (chosenObj.Contains(hit.collider.gameObject))          //是已选择的目标
                            {
                                DeleteChosenObj(hit.collider.gameObject);
                            }
                        }
                        else
                        {
                            ChosenObjClaer();           //清理已选择列表
                            AddChosenObj(hit.collider.gameObject);
                        }
                    }
                    else                                        //选中其他玩家的单位
                    {
                        if (!(Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))  // 非按下shift和ctrl键的情况下
                        {

                            ChosenObjClaer();           //清理已选择列表       


                            chosenOtherPlayerObj = hit.collider.gameObject;
                            chosenOtherPlayerObj.GetComponent<HumanControl>().OnSelected(Color.red);       //开启其选择框

                        }
                    }
                }
            }
            else                //框选模式
            {
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))   //按下shift(添加单位)
                {
                    Checkbox(true);          //调用框选函数
                }
                else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))  // 按下ctrl(剔除单位)
                {
                    Checkbox(false);          //调用框选函数
                }
                else
                {
                    ChosenObjClaer();        //清理已选择列表
                    Checkbox(true);          //调用框选函数
                }
            }
        }



        int i = Random.Range(0, 5);
        //Debug.Log(i);
    }

    //框选函数
    private void Checkbox(bool isAdd)
    {
        Vector3 lowerLeftPos = new Vector3(Mathf.Min(startPos.x, endPos.x), Mathf.Min(startPos.y, endPos.y), 0);         //框左下角点
        Vector3 upperRightPos = new Vector3(Mathf.Max(startPos.x, endPos.x), Mathf.Max(startPos.y, endPos.y), 0);        //框右上角点


        foreach (GameObject unit in playerInfo.MilitaryUnits)
        {
            Vector3 unitScreenPos = Camera.main.WorldToScreenPoint(unit.transform.position);            //作战单位的世界坐标转屏幕坐标
            if (unitScreenPos.x > lowerLeftPos.x && unitScreenPos.y > lowerLeftPos.y && unitScreenPos.x< upperRightPos.x && unitScreenPos.y < upperRightPos.y)          //是否在框选范围内
            {

                if (isAdd)
                {
                    if (!chosenObj.Contains(unit))          //不是已选择的目标
                    {
                        AddChosenObj(unit);                //添加符合条件的单位
                    }
                }
                else
                {
                    if (chosenObj.Contains(unit))          //是已选择的目标
                    {
                        DeleteChosenObj(unit);                //删除符合条件的单位
                    }
                }
            }
        } 
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

    
    //清理所有选中
    private void ChosenObjClaer()
    {
        if(chosenObj.Count != 0)            //选中的是自己单位
        {
            foreach (GameObject a in chosenObj)
            {
                a.GetComponent<HumanControl>().OffSelected();       //关闭选择框
            }
            chosenObj.Clear();              //清理数组
        }
        

        if (chosenOtherPlayerObj)           //如果是选中敌人单位
        {
            chosenOtherPlayerObj.GetComponent<HumanControl>().OffSelected();
            chosenOtherPlayerObj = null;
        }
    }

    //选择时添加单位
    private void AddChosenObj(GameObject addObj)
    {
        chosenObj.Add(addObj);
        addObj.GetComponent<HumanControl>().OnSelected(Color.green);       //开启选择框
    }

    //选择时剔除单位
    private void DeleteChosenObj(GameObject addObj)
    {
        addObj.GetComponent<HumanControl>().OffSelected();       //关闭选择框
        chosenObj.Remove(addObj);
    }
}
