using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class open : MonoBehaviour
{
    public GameObject stick;
    public GameObject keyboard;
    public GameObject canvas;
    private GameObject chatroom;

    // Start is called before the first frame update
    void Start()
    {
        chatroom = GameObject.FindWithTag("ChatRoom");
        stick.SetActive(false);
        keyboard.SetActive(false);
        canvas.SetActive(false);
        chatroom.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame) {
            if (stick.activeSelf) { 
                stick.SetActive(false);
                keyboard.SetActive(false);
                canvas.SetActive(false);
                chatroom.SetActive(false);
            }
            else {
                stick.SetActive(true);
                keyboard.SetActive(true);
                canvas.SetActive(true);
                chatroom.SetActive(true);
            }
        }
    }
}
