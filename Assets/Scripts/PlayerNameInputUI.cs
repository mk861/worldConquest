using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerNameInputUI : MonoBehaviour
{
    public TMP_InputField nameField;
    public TMP_Text title;
    public string playerName
    {
        get { return playerName; }
        set { Debug.Log("You can't set the player name like that"); }
    }

    public void SubmitName()
    {
        if (string.IsNullOrEmpty(nameField.text) == false)
        {
            playerName = nameField.text;
        }
    }

    public void SetTitle(string titleName)
    {
        title.text = titleName;
    }
}





