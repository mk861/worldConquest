using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectorUI : MonoBehaviour
{
    public class OnPlayersConfirmedEventArgs : System.EventArgs
    {
        public List<Player> players;
    }
    private OnPlayersConfirmedEventArgs onPlayersConfirmedEventArgs = new OnPlayersConfirmedEventArgs();
    public static event System.EventHandler<OnPlayersConfirmedEventArgs> OnPlayersConfirmed;

    public List<PlayerSelectorData> playersData = new List<PlayerSelectorData>();
    public List<PlayerSelectorData> players = new List<PlayerSelectorData>();
    // PlayerSelectorData player;
    public int maxPlayers = 6;
    public int minPlayers = 2;

    /// <summary>
    /// Starts Initialise as soon as script is loaded
    /// </summary>
    private void Start()
    {
        Initialise();
    }

    /// <summary>
    /// Initialises players 
    /// </summary>
    [Button]
    public void Initialise()
    {
        if (players.Count < minPlayers)
        {
            players.Clear();
            for (int i = 0; i < minPlayers; i++)
            {
                AddPlayer();
            }
        }
    }

    /// <summary>
    /// Adds a player and assigns all the player data 
    /// </summary>
    [Button]
    public void AddPlayer()
    {
        if (players.Count >= maxPlayers)
            return;

        var playerData = new PlayerSelectorData();
        int index = players.Count;
        playerData.playerName = playersData[index].playerName;
        playerData.playerPrefab = playersData[index].playerPrefab;
        playerData.playerPosition = playersData[index].playerPosition;
        playerData.playerColor = playersData[index].playerColor;
        players.Add(playerData);
        GameObject playerObj = Instantiate(playerData.playerPrefab, playerData.playerPosition, Quaternion.identity);
        playerObj.SetActive(true);
        Player player = playerObj.GetComponent<Player>();
        player.PlayerName = playerData.playerName;
        player.PlayerColor = playerData.playerColor;
        selectedPlayers.Add(player);
        //   PlayerSelectorData player = new PlayerSelectorData();
        //   player.playerName = Random.Range(0, 9999).ToString();
        //  player.playerPrefab.transform.position = player.playerPosition;
        //  players.Add(player); MoveTroopsStage
    }

    /// <summary>
    /// Removes a player and destroys the game object 
    /// </summary>
    [Button]
    public void RemovePlayer()
    {
        if (players.Count <= minPlayers)
            return;

        players.RemoveAt(players.Count - 1);
        Destroy(selectedPlayers[selectedPlayers.Count - 1].gameObject);
        selectedPlayers.RemoveAt(selectedPlayers.Count - 1);
    }

    /// <summary>
    /// Confirms players 
    /// </summary>
    private List<Player> selectedPlayers = new List<Player>();
    [Button]
    public void ConfirmPlayers()
    {
        PlayerManager.Instance.SetPlayers(selectedPlayers);

        onPlayersConfirmedEventArgs.players = selectedPlayers;
        OnPlayersConfirmed?.Invoke(this, onPlayersConfirmedEventArgs);
    }
}
