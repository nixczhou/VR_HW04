using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class ShowPlayerName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string nickname = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log(nickname);
        TextMeshPro mText = gameObject.GetComponent<TextMeshPro>();
        mText.text = nickname;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
