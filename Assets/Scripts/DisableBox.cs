using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBox : MonoBehaviour
{
    public GameObject box;
    // public GameObject answer;
    // Start is called before the first frame update
    void Start()
    {
        disable_box();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void disable_box()
    {
        box.SetActive(false);
    }
}
