using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectorUI : MonoBehaviour
{

    public List<PlayerSelectorData> players = new List<PlayerSelectorData>();
   // PlayerSelectorData player;
    public int maxPlayers = 6;
    public int minPlayers = 2;

    private void Start()
    {
       // player = new PlayerSelectorData();
        Initialise();
    }

    [Button]
    public void Initialise()
    {

        for (int i = 0; i < minPlayers; i++)
        {
            AddPlayer();
        }
    }

    [Button]
    public void AddPlayer()
    {
        if (players.Count >= maxPlayers)
            return;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].playerName = Random.Range(0, 9999).ToString();
            players[i].playerPrefab.transform.position = players[i].playerPosition;
        }
     //   PlayerSelectorData player = new PlayerSelectorData();
     //   player.playerName = Random.Range(0, 9999).ToString();
      //  player.playerPrefab.transform.position = player.playerPosition;
      //  players.Add(player);
    }

    [Button]
    public void RemovePlayer()
    {
        if(players.Count <= minPlayers)
            return;
        players.RemoveAt(players.Count - 1);
    }

    [Button]
    public void ConfirmPlayers()
    {
        List<Player> _players = new List<Player>();
        players.ForEach(p =>
        {
            Player player = new Player(p.playerName, p.playerColor);
            _players.Add(player);
        });

        PlayerManager.Instance.SetPlayers(_players); 

    } 

}
