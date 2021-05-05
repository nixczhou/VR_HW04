using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mirror;
using TMPro;

public class LineRaycaster : NetworkBehaviour
{
    [SerializeField] private float distance = 20f;
    [SerializeField] private Transform rightHand;
    private LineRenderer lineRenderer;
    private GameObject wall1, wall2, wall3, wall4;//side of the box
    
    [SyncVar]
    private bool correctness = false;//id get correct answer, open the box
    [SyncVar]
    private GameObject moving_object;//set target object

    //set correctness and moving_object on server
    //[Server]
    //[Server]

    private void Start()
    {
        //get lineRenderer, wall1, wall2, wall3, wall4
    }
    
    [Client]
    private void Update()
    {
        //get player authority to call command, check Chat(Mirror Example) -> ChatWindow.cs -> OnSend()

        //if press key G, call command hold object, else if key up, call command release object

        Ray ray = new Ray(rightHand.position, rightHand.forward * distance + rightHand.position);
        lineRenderer.SetPosition(0, rightHand.position);
        lineRenderer.SetPosition(1, rightHand.forward * 5f + rightHand.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            lineRenderer.SetPosition(1, hit.point);
            //if collider object is interactable object, call command to set moving_object to collider gameobject
            //else if the object is those choices and get correct answer, call command to set correctness
        }

        if(correctness)//open the box
        {
            if (wall1.transform.localScale.z > 0.1f)
            {
                wall1.transform.localScale -= new Vector3(0f, 0f, 0.1f);
                wall1.transform.position += new Vector3(0f, 0.05f, 0f);

                wall2.transform.localScale -= new Vector3(0f, 0f, 0.1f);
                wall2.transform.position += new Vector3(0f, 0.05f, 0f);

                wall3.transform.localScale -= new Vector3(0f, 0f, 0.1f);
                wall3.transform.position += new Vector3(0f, 0.05f, 0f);

                wall4.transform.localScale -= new Vector3(0f, 0f, 0.1f);
                wall4.transform.position += new Vector3(0f, 0.05f, 0f);
            }
        }
    }

    [Command]//set gameobject's kinematic to true and call clientRpc to update gamobject position
    private void cmd_GetHold(GameObject collider)
    {
    }
    
    [Command]//set gameobject's kinematic to false and call command to set moving_object to null
    private void cmd_Release(GameObject collider)
    {
    }
    
    [Command]//call server function to change correctness value
    public void cmd_open()
    {
    }

    [Command]//call server to set moving_object
    public void cmd_set_target(GameObject gameObject)
    {
    }

    [ClientRpc]//Update moving_object's position
    private void Move_Object(Vector3 dest) { }
}
