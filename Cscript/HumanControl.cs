using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    //选中与非选中
    public void OnSelected()
    {
        isSelect = true;
        canvas.gameObject.SetActive(true);
    }

    public void OffSelected()
    {
        isSelect = false;
        canvas.gameObject.SetActive(false);
    }
}
