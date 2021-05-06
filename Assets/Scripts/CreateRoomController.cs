using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public class CreateRoomController : MonoBehaviourPunCallbacks
{
    [Header("[ Panel ]")]
    public GameObject createRoomPanel;		        //the create room Panel
	public GameObject roomLoadingPanel;		        //the room loading Panel

    [Header("[ Other Setting ]")]
    public LobbyPanelController lobbyPanelScript;   //get the room list info. from LobbyPanelController script

    [Header("[ Show the Message ]")]
    public Text roomName;					        //the room name
	public Text warningMess;				        //the warning text

    /// <summary>
    /// Clear the warning text when create room Panel enable
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private new void OnEnable()
    {
		warningMess.text = "";
    }

    /// <summary>
    /// Do the process after the user click the Confirm button on the create room Panel
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickConfirmCreateRoomButton()
    {
		RoomOptions roomOptions = new RoomOptions();

        roomOptions.MaxPlayers = 2;

        bool isRoomNameRepeat = false;

        foreach (KeyValuePair<string, RoomInfo> entry in lobbyPanelScript.cachedRoomList)
        {
            if (roomName.text == entry.Value.Name)
            {
                isRoomNameRepeat = true;
                break;
            }
        }

		if(isRoomNameRepeat)
        {
			warningMess.text = "The room name is already exist!";
		}
		else
        {
            String ut = DateTime.UtcNow.ToString();
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                { "createTime", ut }
            };
            String[] s = { "createTime" };
            roomOptions.CustomRoomPropertiesForLobby = s;

            PhotonNetwork.CreateRoom(roomName.text, roomOptions, TypedLobby.Default);
#if(UNITY_EDITOR)
            Debug.Log("Create the room");
#endif
            createRoomPanel.SetActive(false);
			roomLoadingPanel.SetActive(true);
        }
	}

    /// <summary>
    /// Do the process after the user click the cancel button on the create room Panel
    /// </summary>
    /// <param></param>
    /// <returns></returns>
	public void ClickCancelCreateRoomButton()
    {
        createRoomPanel.SetActive(false);
		warningMess.text = "";
	}
}
