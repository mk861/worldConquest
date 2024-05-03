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

    /// <summary>
    /// Takes the name that was submitted and assignes it to playerName
    /// </summary>
    public void SubmitName()
    {
        if (string.IsNullOrEmpty(nameField.text) == false)
        {
            playerName = nameField.text;
        }
    }

    /// <summary>
    /// Set's the title of the UI box to the desired title name (Enter "Blue's" name:)
    /// </summary>
    /// <param name="titleName"></param>
    public void SetTitle(string titleName)
    {
        title.text = titleName;
    }
}