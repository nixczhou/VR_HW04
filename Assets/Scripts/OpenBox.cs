using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class OpenBox : MonoBehaviour
{
    // public GameObject box;
    // public GameObject answer;
    bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        // disable_box();
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            if (gameObject.transform.position.y < 4.0f)
            {
                Vector3 dy = Vector3.up * 4.0f * Time.deltaTime;
                gameObject.transform.position = gameObject.transform.position + dy;
            }
        }
    }

    public void disable_box()
    {
        gameObject.SetActive(false);
    }

    public void open_box()
    {
        gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
        Debug.Log("open box");
        open = true;
    }
}
