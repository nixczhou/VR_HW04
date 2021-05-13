using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SensorOpenDoor : MonoBehaviour
{
    public GameObject door1;
    // public Transform door2;

    bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            // open door
            if(door1.transform.position.y < 9.0f)
            {
                Vector3 dy = Vector3.up * 4.0f * Time.deltaTime;
                door1.transform.position = door1.transform.position + dy;
                // door2.position = door2.position + dy;
            }
        }
        else
        {
            // close door
            if(door1.transform.position.y > 0.0f)
            {
                Vector3 dy = Vector3.up * -4.0f * Time.deltaTime;
                door1.transform.position = door1.transform.position + dy;
                // door2.position = door2.position + dy;
            }
        }
    }

    // void FixedUpdate()
    // {
    //     RaycastHit hit;
    //     // Does the ray intersect any objects excluding the player layer
    //     if (Physics.Raycast(transform.position, transform.up, out hit))
    //     {
    //         // Debug.Log("Hit");
    //         if (hit.transform.gameObject.tag != "Player")
    //         {
    //             // open door
    //             if(door1.position.y < 9.0f)
    //             {
    //                 Vector3 dy = Vector3.up * 4.0f * Time.deltaTime;
    //                 door1.position = door1.position + dy;
    //                 door2.position = door2.position + dy;
    //             }
    //         }
    //     }
    //     else
    //     {
    //         // close door
    //         if(door1.position.y > 0.0f)
    //         {
    //             Vector3 dy = Vector3.up * -4.0f * Time.deltaTime;
    //             door1.position = door1.position + dy;
    //             door2.position = door2.position + dy;
    //         }
    //     }
    // }

    void OnTriggerEnter(Collider collider)
    {
        // if(collision.gameObject.tag != "Player")
        // {
            door1.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            Debug.Log("open door");
            open = true;
        // }
    }

    void OnTriggerExit(Collider collider)
    {
        // if(collision.gameObject.tag != "Player")
        // {
            door1.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            Debug.Log("close door");
            open = false;
        // }
    }
}
