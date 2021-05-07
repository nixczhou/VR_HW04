using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]
public class RoomPanelController : MonoBehaviourPunCallbacks
{
    [Header("[ Panel ]")]
    public GameObject lobbyPanel;   //the lobby Panel
    public GameObject roomPanel;    //the room Panel

    [Header("[ Button ]")]
    public Button backButton;       //the back button
    public Button readyButton;      //the ready button

    [Header("[ room info. ]")]
    public Text roomName;           //the room name
	public GameObject team1;        //the team 1 member
	public GameObject team2;        //the team 2 member

    [Header("[ Show the Message ]")]
    public Text warningMess;        //the warning mess

	private PhotonView pView;       //the photon component
    private Text[] texts;           //show the player info.
    private ExitGames.Client.Photon.Hashtable costomProperties; //save the player properties

    /// <summary>
    /// Init the Room Panel and give the user location
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private new void OnEnable()
    {
		pView = GetComponent<PhotonView>();
		if(!PhotonNetwork.IsConnected)
            return;

		roomName.text = "Name: " + PhotonNetwork.CurrentRoom.Name;
		warningMess.text = "";

		backButton.onClick.RemoveAllListeners();
		backButton.onClick.AddListener (delegate() 
        {
			PhotonNetwork.LeaveRoom();
            lobbyPanel.SetActive (true);
			roomPanel.SetActive (false);
		});
        
		DisableTeamPanel();
		UpdateTeamPanel(false);

        if (!team1.activeSelf)
        {
#if(UNITY_EDITOR)
            Debug.Log("You are team 1");
#endif
            team1.SetActive(true);
            texts = team1.GetComponentsInChildren<Text>();
            texts[0].text = PhotonNetwork.NickName;
            if (PhotonNetwork.IsMasterClient)
                texts[1].text = "Master Client";
            else
                texts[1].text = "Not ready";
            costomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                    { "Team","team1" },
                    { "isReady", false }
            };
            bool setBool = PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
            Debug.Log("setBool: " + setBool);
        }
        else if (!team2.activeSelf)
        {
#if (UNITY_EDITOR)
            Debug.Log("You are team 2");
#endif
            team2.SetActive(true);
            texts = team2.GetComponentsInChildren<Text>();
            texts[0].text = PhotonNetwork.NickName;
            if (PhotonNetwork.IsMasterClient)
                texts[1].text = "Master Client";
            else
                texts[1].text = "Not ready";
            costomProperties = new ExitGames.Client.Photon.Hashtable()
            {
                    { "Team", "team2" },
                    { "isReady", false }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(costomProperties);
        }
	}

    /// <summary>
    /// Disable the all team Panel
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void DisableTeamPanel()
    {
        team1.SetActive(false);
        team2.SetActive(false);
	}

    /// <summary>
    /// Show the player info. on the team panel
    /// </summary>
    /// <param>
    /// bool isUpdateSelf: show local player info. or not
    /// </param>
    /// <returns></returns>
    public void UpdateTeamPanel(bool isUpdateSelf)
    {
		foreach (Player p in PhotonNetwork.PlayerList)
        {
			if (!isUpdateSelf && p.IsLocal)
                continue;

			costomProperties = p.CustomProperties;
			if (costomProperties["Team"].Equals ("team1"))
            {
                team1.SetActive(true);

                texts = team1.GetComponentsInChildren<Text>();
                texts[0].text = p.NickName;
                if (p.IsMasterClient)
                    texts[1].text = "Master Client";
                else if ((bool)costomProperties["isReady"])
                    texts[1].text = "Ready";
                else
                    texts[1].text = "Not Ready";
            }
            else
            {
                team2.SetActive(true);

                texts = team2.GetComponentsInChildren<Text>();
                texts[0].text = p.NickName;
                if (p.IsMasterClient)
                    texts[1].text = "Master Client";
                else if ((bool)costomProperties["isReady"])
                    texts[1].text = "Ready";
                else
                    texts[1].text = "Not Ready";
            }
        }
    }

    /// <summary>
    /// Ready Button Setup
    /// </summary>
    /// <param></param>
    /// <returns></returns>
	public void ReadyButtonControl()
    {
		if(PhotonNetwork.IsMasterClient)
        {	
			readyButton.GetComponentInChildren<Text>().text = "Start";
			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(delegate()
            {
				ClickStartGameButton();
			});
		}
        else
        {
			if((bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"])
				readyButton.GetComponentInChildren<Text>().text = "Cancel ready";		
			else 
				readyButton.GetComponentInChildren<Text> ().text = "Ready";

			readyButton.onClick.RemoveAllListeners();
			readyButton.onClick.AddListener(delegate()
            {
				ClickReadyButton();
			});
		}
	}

    /// <summary>
    /// Do the process after user click the ready button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
	public void ClickReadyButton()
    {
		bool isReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isReady"];
		costomProperties = new ExitGames.Client.Photon.Hashtable (){ { "isReady",!isReady } };

		PhotonNetwork.LocalPlayer.SetCustomProperties (costomProperties);
		Text readyButtonText = readyButton.GetComponentInChildren<Text>();

		if(isReady)
            readyButtonText.text = "Ready";
		else
            readyButtonText.text = "Cancel ready";
	}

    /// <summary>
    /// Do the process after master client click the start button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickStartGameButton()
    {
        Player[] playerList = PhotonNetwork.PlayerList;

        if (playerList.Length == 1)
        {
            warningMess.text = "just one people => cannot start";
            return;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
			if(p.IsLocal)
                continue;

			if ((bool)p.CustomProperties["isReady"] == false)
            {
				warningMess.text = "Not all people ready => cannot start";
				return;
			}
		}
		warningMess.text = "";
        PhotonNetwork.CurrentRoom.IsOpen = false;
		pView.RPC ("LoadGameScene", Photon.Pun.RpcTarget.All, "World");
	}
    
    /// <summary>
    /// RPC Function, all player load the game scene
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    [PunRPC]
	public void LoadGameScene(string sceneName)
    {
        PhotonNetwork.LoadLevel (sceneName);
	}
}
