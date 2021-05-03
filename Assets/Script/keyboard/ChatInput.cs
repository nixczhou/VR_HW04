using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class ChatInput : MonoBehaviour//NetworkBehaviour
{
    [SerializeField] private GameObject keyboard;
    [SerializeField] private TMP_InputField input_field;

    private int state;
    private Button[] buttons1;
    //private string str;
    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        buttons1 = keyboard.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons1.Length; i++)
        {
            if (buttons1[i].name == "DEL")
            {
                buttons1[i].onClick.AddListener(delegate { InputDeletion(input_field); });
            }
            else if (buttons1[i].name == "Shift")
            {
                buttons1[i].onClick.AddListener(delegate { InputShift(buttons1, 1); });
            }
            else if (buttons1[i].name == "<-")
            {

            }
            else if (buttons1[i].name == "->")
            {

            }
            else
            {
                string str1 = buttons1[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                buttons1[i].onClick.AddListener(delegate { InputOnClick(str1, input_field); });
            }
        }
        //keyboard1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InputOnClick(string message, TMP_InputField field)
    {
        if (state == 1)
            field.text = field.text + message.ToUpper();
        else
            field.text = field.text + message;
    }

    void InputDeletion(TMP_InputField field)
    {
        string str = field.text.Remove(field.text.Length - 1, 1);
        field.text = str;
    }

    void InputShift(Button[] buttons, int n)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (Regex.IsMatch(buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, @"[a-z]") && buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length == 1)
            {
                string str = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToUpper();
                buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
                state = 1;
            }
            else if (Regex.IsMatch(buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text, @"[A-Z]") && buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.Length == 1)
            {
                string str = buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text.ToLower();
                buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
                state = 0;
            }
        }
    }
}
