using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManger : MonoBehaviour
{
    public Transform teamOneTransform;
    public Transform teamTwoTransform;

    private GameObject localPlayer = null;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
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

        Transform tempTransform = localPlayer.transform;
        mainCamera.transform.parent = tempTransform;
        mainCamera.transform.localPosition = new Vector3(0, 0, 0);
        mainCamera.transform.localRotation = Quaternion.identity;
    }
}
