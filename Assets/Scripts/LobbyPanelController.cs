using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;

public class LobbyPanelController : MonoBehaviourPunCallbacks
{
    [Header("[ Panel ]")]
    public GameObject loginPanel;           //the login Panel
	public GameObject lobbyPanel;           //the lobby Panel
    public GameObject createRoomPanel;      //the create Panel
    public GameObject roomPanel;			//the room Panel
    public GameObject roomMessagePanel;		//the room mess Panel

    [Header("[ Show the Message ]")]
    public GameObject userMessage;          //the username mess
    public Text pageMessage;                //the page number mess

    [Header("[ Loading label ]")]
    public GameObject lobbyLoadingLabel;    //the lobby loading label
	public GameObject roomLoadingLabel;		//the room loading label
    
    [Header("[ Button ]")]
    public Button backButton;               //the back button
    public Button randomJoinButton;         //the random join button
    public GameObject previousButton;       //the previous button
    public GameObject nextButton;           //the next button
    
    /// <summary>
    /// save the room info. to cache on lobby
    /// </summary>
    [HideInInspector]
    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();

    /// <summary>
    /// convert the data type from Dictionary to List
    /// </summary>
    [HideInInspector]
    public List<KeyValuePair<string, RoomInfo>> myList = new List<KeyValuePair<string, RoomInfo>>();

    [HideInInspector]
    public int currentPageNumber;           //the current page number
    [HideInInspector]
    public int maxPageNumber;				//the max page number
    [HideInInspector]
    public int roomPerPage = 4;             //the total number of the room per page

    private GameObject[] roomMessage;       //room info. on the scene

    /// <summary>
    /// Init all mess on lobby when the lobby Panel is enable
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    private new void OnEnable()
    {
		currentPageNumber = 1;
		maxPageNumber = 1;
        lobbyLoadingLabel.SetActive(true);
		roomLoadingLabel.SetActive(false);

		if(createRoomPanel!=null)
			createRoomPanel.SetActive(false);

		RectTransform rectTransform = roomMessagePanel.GetComponent<RectTransform> ();
		roomPerPage = rectTransform.childCount;
        
		roomMessage = new GameObject[roomPerPage];
		for(int i = 0; i < roomPerPage; i++)
        {
			roomMessage[i] = rectTransform.GetChild(i).gameObject;
			roomMessage[i].SetActive(false);
		}

		backButton.onClick.RemoveAllListeners();
		backButton.onClick.AddListener(delegate()
            {
                PhotonNetwork.Disconnect();
                loginPanel.SetActive(true);
                lobbyPanel.SetActive(false);
                userMessage.SetActive(false);
                backButton.gameObject.SetActive(false);
                userMessage.GetComponentInChildren<Text>().text = "Welcome, ";
            });
		if(roomPanel!=null)
			roomPanel.SetActive(false);
        PhotonNetwork.JoinLobby(TypedLobby.Default);

    }

    /// <summary>
    /// Show the room mess
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ShowRoomMessage()
    {
        myList = cachedRoomList.ToList();
        myList.Sort((x, y) => Convert.ToDateTime(x.Value.CustomProperties["createTime"]).CompareTo(Convert.ToDateTime(y.Value.CustomProperties["createTime"])));

#if(UNITY_EDITOR)
        foreach (KeyValuePair<string, RoomInfo> item in myList)
            Debug.Log("Room Properties = " + (String)item.Value.CustomProperties["createTime"]);
#endif

        int start, end, i, j;
		start = (currentPageNumber - 1) * roomPerPage;
		if(currentPageNumber * roomPerPage < cachedRoomList.Count)
			end = currentPageNumber * roomPerPage;
		else
			end = cachedRoomList.Count;

		for(i = start, j = 0; i < end; i++, j++)
        {
			RectTransform rectTransform = roomMessage[j].GetComponent<RectTransform>();
            string roomName = myList[i].Value.Name;
            rectTransform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
			rectTransform.GetChild(1).GetComponent<Text>().text = roomName;
			rectTransform.GetChild(2).GetComponent<Text>().text = myList[i].Value.PlayerCount + "/" + myList[i].Value.MaxPlayers;
			Button button = rectTransform.GetChild(3).GetComponent<Button>();

			if(myList[i].Value.PlayerCount == myList[i].Value.MaxPlayers || myList[i].Value.IsOpen == false)
				button.gameObject.SetActive(false);
			else
            {
				button.gameObject.SetActive(true);
				button.onClick.RemoveAllListeners();
				button.onClick.AddListener(delegate()
                {
					ClickJoinRoomButton(roomName);
				});
			}
			roomMessage[j].SetActive(true);
		}

		while(j < 4)
        {
			roomMessage[j++].SetActive(false);
		}
	}

    /// <summary>
    /// The previous and next button control
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ButtonControl()
    {
		if (currentPageNumber == 1)
			previousButton.SetActive(false);
		else
			previousButton.SetActive(true);

		if (currentPageNumber == maxPageNumber)
			nextButton.SetActive(false);
		else
			nextButton.SetActive(true);
	}

    /// <summary>
    /// Do the process after the user click the create room button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickCreateRoomButton()
    {
		createRoomPanel.SetActive(true);
	}

    /// <summary>
    /// Do the process after the user click the random join room button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickRandomJoinButton()
    {
		PhotonNetwork.JoinRandomRoom();
		roomLoadingLabel.SetActive(true);
	}

    /// <summary>
    /// Do the process after the user click the previous button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickPreviousButton()
    {
		currentPageNumber--;
		pageMessage.text = currentPageNumber.ToString () + " / " + maxPageNumber.ToString();
		ButtonControl();
		ShowRoomMessage();
	}

    /// <summary>
    /// Do the process after the user click the previous button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickNextButton()
    {
		currentPageNumber++;
		pageMessage.text = currentPageNumber.ToString() + " / " + maxPageNumber.ToString();
		ButtonControl();
		ShowRoomMessage();
	}

    /// <summary>
    /// Do the process after the user click the join room button
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    public void ClickJoinRoomButton(string roomName)
    {
		PhotonNetwork.JoinRoom(roomName);
		roomLoadingLabel.SetActive(true);
	}
}
