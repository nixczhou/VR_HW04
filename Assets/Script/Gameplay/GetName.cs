using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetName : MonoBehaviour
{
    [SerializeField] private TMP_Text player_name;
    // Start is called before the first frame update
    void Start()
    {
        player_name.text = transform.parent.GetComponent<PlayerController>().playerName;
    }
}
