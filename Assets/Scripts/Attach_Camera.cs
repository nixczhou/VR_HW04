using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Attach_Camera : MonoBehaviour
{
    [SerializeField] private string obj;
    // Update is called once per frame
    void Update()
    {
        if(obj == "Player")
            transform.GetComponent<Canvas>().worldCamera = GameObject.Find(obj).transform.GetChild(0).GetComponent<Camera>() ;
        else if(obj == "Main Camera")
            transform.GetComponent<Canvas>().worldCamera = GameObject.Find(obj).GetComponent<Camera>();
    }
}
