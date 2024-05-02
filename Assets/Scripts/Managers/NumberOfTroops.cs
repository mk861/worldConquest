using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberOfTroops : MonoBehaviour
{

    private string _playerName;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _playerName = PlayerManager.Instance.playerList[PlayerManager.Instance.CurrentPlayerTurnIndex].PlayerName;
    }
}
