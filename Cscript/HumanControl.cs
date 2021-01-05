using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanControl : MonoBehaviour
{
    private bool isSelect = false;
    private Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        //拿到是否被选中的圈，并关闭它
        canvas = GetComponentInChildren<Canvas>();
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }



    //开启选框
    public void OnSelected(Color color)
    {
        isSelect = true;
        canvas.GetComponentInChildren<Image>().color = color;   //改变选框颜色
        canvas.gameObject.SetActive(true);
    }

    //关闭选框
    public void OffSelected()
    {
        isSelect = false;
        canvas.gameObject.SetActive(false);
    }
}
