using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;

public class LoginPanelController : MonoBehaviourPunCallbacks
{
    [Header("[ Panel ]")]
    public GameObject loginPanel;   //the login Panel
    public GameObject lobbyPanel;   //the lobby Panel
    public GameObject roomPanel;    //the room Panel

    [Header("[ Button ]")]
    public Button backButton;       //the back button

    [Header("[ Show the Message ]")]
    public GameObject userMessage;  //the username mess
	public Text username;           //the name of user
	public Text connectionState;    //the connection state

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    private string gameVersion = "2.0";

    /// <summary>
    /// Init the connection according to the client connection state
    /// </summary>
    /// <param></param>
    /// <returns></returns>
	private void Start ()
    {
        //if connection not connect to Photon Network
        if (!PhotonNetwork.IsConnected)
        {
            SetLoginPanelActive();  //enable Login Panel
            username.text = PlayerPrefs.GetString("Username");  //if user info. provide in local host => get the username
        }
        else
        {
            SetLobbyPanelActive();  //enable Lobby Panel
        }

		connectionState.text = "";  //init the mess of the connection state
    }

#if(UNITY_EDITOR)
    /// <summary>
    /// Network State Debug
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private void Update()
    {
        connectionState.text = PhotonNetwork.NetworkClientState.ToString(); //Show the details on the bottom left-hand corner
    }
#endif

    /// <summary>
    /// Enable Login Panel and close another UI components
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void SetLoginPanelActive()
    {
		loginPanel.SetActive(true);
		userMessage.SetActive(false);
		backButton.gameObject.SetActive(false);
		lobbyPanel.SetActive(false);
		if(roomPanel != null)
			roomPanel.SetActive(false);
	}

    /// <summary>
    /// Enable Lobby Panel and close another UI components
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void SetLobbyPanelActive()
    {				
		loginPanel.SetActive(false);
		userMessage.SetActive(true);
		backButton.gameObject.SetActive(true);
		lobbyPanel.SetActive(true);
	}

    /// <summary>
    /// Do some process after click the login button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickLogInButton()
    {
		SetLobbyPanelActive();

        if (!PhotonNetwork.IsConnected)
        {
            //Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
#if(UNITY_EDITOR)
            Debug.Log("Join the Name Server");
#endif
        }

        //if username == null, itin username by random
        if (username.text == "")
			username.text = "user" + UnityEngine.Random.Range (1, 9999);
        PhotonNetwork.LocalPlayer.NickName = username.text;
		PlayerPrefs.SetString ("Username", username.text);  //save the user name in the local host
    }

    /// <summary>
    /// Rewrite the callback function => Join Default Lobby after successfully connect the Master Server
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /// <summary>
    /// Rewrite the callback function => Show the username after the user successfully enters a lobby on the Master Server from the Name Server
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnJoinedLobby()
    {
        userMessage.GetComponentInChildren<Text>().text = "Welcome, " + PhotonNetwork.LocalPlayer.NickName;
        lobbyPanel.GetComponent<LobbyPanelController>().lobbyLoadingLabel.SetActive(false);
#if (UNITY_EDITOR)
        Debug.Log("Join the Join Lobby");
#endif
    }

    /// <summary>
    /// Rewrite the callback function => Update the room list after join the lobby "cleanly"
    /// for more info., pls review:
    /// https://doc.photonengine.com/zh-tw/pun/current/lobby-and-matchmaking/matchmaking-and-lobby#default_lobby_type
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnRoomListUpdate(List<RoomInfo> _roomList)
    {
#if (UNITY_EDITOR)
        Debug.Log("Update the Room List");
#endif
        LobbyPanelController lobbyPanelScript = lobbyPanel.GetComponent<LobbyPanelController>();
        
        for (int i = 0; i < _roomList.Count; i++)
        {
            RoomInfo info = _roomList[i];
            if (info.RemovedFromList)
            {
                lobbyPanelScript.cachedRoomList.Remove(info.Name);
            }
            else
            {
                lobbyPanelScript.cachedRoomList[info.Name] = info;
            }
        }
        
#if(UNITY_EDITOR)
        Debug.Log("lobbyPanelScript.cachedRoomList Count = " + lobbyPanelScript.cachedRoomList.Count);
#endif
        lobbyPanelScript.maxPageNumber = (lobbyPanelScript.cachedRoomList.Count - 1) / lobbyPanelScript.roomPerPage + 1;
        if (lobbyPanelScript.currentPageNumber > lobbyPanelScript.maxPageNumber)
            lobbyPanelScript.currentPageNumber = lobbyPanelScript.maxPageNumber;
        lobbyPanelScript.pageMessage.text = lobbyPanelScript.currentPageNumber.ToString() + " / " + lobbyPanelScript.maxPageNumber.ToString();

        lobbyPanelScript.ButtonControl();

        lobbyPanelScript.ShowRoomMessage();

        if (lobbyPanelScript.cachedRoomList.Count == 0)
            lobbyPanelScript.randomJoinButton.interactable = false;
        else
            lobbyPanelScript.randomJoinButton.interactable = true;
    }

#if (UNITY_EDITOR)
    /// <summary>
    /// Rewrite the callback function => Just for debug, print mess when create room successfully
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnCreatedRoom()
    {
        Debug.Log("create room successfully and enter room now");
    }
#endif

#if (UNITY_EDITOR)
    /// <summary>
    /// Rewrite the callback function => Just for debug, print mess when create room unsuccessfully
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(String.Format("return code = {0}, return mess = {1}", returnCode, message));
    }
#endif

    /// <summary>
    /// Rewrite the callback function => Disable the lobby Panel and enable the room panel after the user successfully enters the room
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);

#if (UNITY_EDITOR)
        Debug.Log("Print Room Custom Properties: " + (String)PhotonNetwork.CurrentRoom.CustomProperties["createTime"]);
#endif
    }

    /// <summary>
    /// Rewrite the callback function => Update the player info. after player attr. changed
    /// For more info., pls review:
    /// https://doc-api.photonengine.com/en/pun/v2/class_photon_1_1_pun_1_1_mono_behaviour_pun_callbacks.html#afb96ff9ce687e592d74866b8775f1b32
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        roomPanel.GetComponent<RoomPanelController>().DisableTeamPanel();
        roomPanel.GetComponent<RoomPanelController>().UpdateTeamPanel(true);
        roomPanel.GetComponent<RoomPanelController>().ReadyButtonControl();

#if (UNITY_EDITOR)
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            Debug.Log("Print Team info.: ");
            Debug.Log("PhotonNetwork.LocalPlayer.CustomProperties[Team]: " + (String)PhotonNetwork.LocalPlayer.CustomProperties["Team"]);
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReady"))
        {
            Debug.Log("Print Team isReady or not: ");
            Debug.Log("PhotonNetwork.LocalPlayer.CustomProperties[isReady]: " + (bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"]);
        }
#endif
    }

    /// <summary>
    /// Rewrite the callback function => Change the master client after master client change
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomPanel.GetComponent<RoomPanelController>().ReadyButtonControl();
    }

    /// <summary>
    /// Rewrite the callback function => Update the player info. after player leave
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnPlayerLeftRoom(Player leftPlayer)
    {
        roomPanel.GetComponent<RoomPanelController>().DisableTeamPanel();
        roomPanel.GetComponent<RoomPanelController>().UpdateTeamPanel(true);
    }

    /// <summary>
    /// Rewrite the callback function => Show the username after the user successfully enters a lobby on the Master Server from the Name Server
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public override void OnDisconnected(DisconnectCause cause)
    {
        SetLoginPanelActive();
    }

    /// <summary>
    /// Do some process after click the exit button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickExitGameButton()
    {
        Application.Quit();
    }
}
