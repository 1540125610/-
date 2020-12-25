using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManage : MonoBehaviour
{
    //单例
    public GameManage _gameManage = null;

    private void Awake()
    {
        _gameManage = this;
    }


    public List<GameInfo> playersInfos = new List<GameInfo>();


    // Start is called before the first frame update
    void Start()
    {
        //添加初始单位到玩家单位里
        foreach (GameInfo a in playersInfos)
        {
            foreach(GameObject b in a.StartingUnits)
            {
                if(b.tag == "Military Units")           //作战单位
                {
                    a.MilitaryUnits.Add(b);
                }
                else if(b.tag == "Bulid Units")         //建筑单位
                {
                    a.BulidUnits.Add(b);
                }
                else                                    //异常
                {
                    Debug.Log("出现初始单位无法归类的错误，该单位名字为：" + b.name);
                }
            }
            a.StartingUnits.Clear();            //清理初始单位表
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public class GameInfo
{
    public string Name;  //玩家名字

    public Transform Location;  //起始位置
     
    public Color AccentColor;   //玩家标识颜色

    public List<GameObject> StartingUnits = new List<GameObject>();  //初始单位

    public List<GameObject> MilitaryUnits = new List<GameObject>();     //作战单位

    public List<GameObject> BulidUnits = new List<GameObject>();     //建筑单位

    public bool IsAi;   //是不是AI控制

    public float Credits;  //积分

}
