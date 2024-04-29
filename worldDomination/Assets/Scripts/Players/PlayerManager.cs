using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance { get; private set; }
    public List<Player> playerList = new List<Player>();

    private void Awake()
    {
        Instance = this; 
    }

    public void SetPlayers(List<Player> players)
    {
        playerList = players;
    }
}
[System.Serializable]
public class PlayerSelectorData
{
    public string playerName;
    public Color playerColor;
    public GameObject playerPrefab;
    public Vector3 playerPosition;
}