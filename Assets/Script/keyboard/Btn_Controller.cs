using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Btn_Controller : MonoBehaviour
{
    //public string str;
    public bool isClick;
    public string str;
    private float count_down;
    private void Start()
    {
        isClick = false;
        count_down = 0.03f;
    }

    public void Update()
    {
        if(isClick)
        {
            count_down -= Time.deltaTime;
        }
        if(count_down <= 0f)
        {
            count_down = 0.03f;
            isClick = false;
        }
    }
    public void OnClick()
    {
        isClick = true;
        //Debug.Log(transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
        str = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
