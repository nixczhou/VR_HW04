using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System;

public class GameManger : MonoBehaviour
{
    public Transform teamOneTransform;
    public Transform teamTwoTransform;

    private GameObject localPlayer = null;
    private Camera mainCamera;

    private List<GameObject> listOfChildren;

    private void GetChildRecursive(GameObject obj){
        if (null == obj)
            return;

        foreach (Transform child in obj.transform){
            if (null == child)
                continue;
            //child.gameobject contains the current child you can do whatever you want like add it to an array
            listOfChildren.Add(child.gameObject);
            GetChildRecursive(child.gameObject);
        }
    }

    void Start()
    {   
        mainCamera = Camera.main;
        listOfChildren = new List<GameObject>();
        InstantiatePlayer();
    }

    [PunRPC]
    void InstantiatePlayer()
    {
        ExitGames.Client.Photon.Hashtable playerCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

        if (playerCustomProperties["Team"].ToString().Equals("team1"))
        {
            localPlayer = PhotonNetwork.Instantiate("Player_Team1", teamOneTransform.position, Quaternion.identity, 0);
        }
        else if (PhotonNetwork.LocalPlayer.CustomProperties["Team"].ToString().Equals("team2"))
        {
            localPlayer = PhotonNetwork.Instantiate("Player_Team2", teamTwoTransform.position, Quaternion.identity, 0);
        }
        
        GameObject tempTransform = localPlayer;

        GetChildRecursive(tempTransform);
        GameObject parentObj = listOfChildren.Find(x => x.name == "CameraOffset");
        print("Name = " + parentObj.name);

        mainCamera.transform.parent = parentObj.transform;

        mainCamera.transform.localPosition = new Vector3(0.0f, 1.36144f, -21.438f);
		mainCamera.transform.localRotation = Quaternion.identity;

    }

}
