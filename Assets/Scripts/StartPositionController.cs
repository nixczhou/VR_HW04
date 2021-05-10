using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class StartPositionController : MonoBehaviour
{
    public GameObject Player;
    public Transform startpos1;
    public Transform startpos2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(PhotonNetwork.PlayerList.Length);
        Debug.Log(PhotonNetwork.IsMasterClient);
        if(PhotonNetwork.IsMasterClient)
        {
            // Instantiate(Player, startpos1.position, startpos1.rotation);
            Player.transform.position = startpos1.position;
        }
        else
        {
            // Instantiate(Player, startpos2.position, startpos2.rotation);
            Player.transform.position = startpos2.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
