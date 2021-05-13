using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class TransferOwnership : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Transfer_Ownership()
    {
        gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
    }
    
}
