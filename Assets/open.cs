using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class open : MonoBehaviour
{
    public GameObject stick;
    public GameObject keyboard;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        stick.SetActive(false);
        keyboard.SetActive(false);
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame) {
            if (stick.activeSelf) { 
                stick.SetActive(false);
                keyboard.SetActive(false);
                canvas.SetActive(false);
            }
            else {
                stick.SetActive(true);
                keyboard.SetActive(true);
                canvas.SetActive(true);
            }
        }
    }
}
