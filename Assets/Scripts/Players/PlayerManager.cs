using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public class OnPlayerTurnStartedEventArgs : System.EventArgs
    {
        public Player lastPlayerInTurn;
        public Player playerInTurn;
    }
    private OnPlayerTurnStartedEventArgs onPlayerTurnStartedEventArgs = new OnPlayerTurnStartedEventArgs();
    public static event System.EventHandler<OnPlayerTurnStartedEventArgs> OnPlayerTurnStarted;

    [SerializeField]
    private List<TroopsByPlayerAmount> troopsByPlayerAmountsSettings;

    public List<Player> playerList = new List<Player>();

    public int CurrentPlayerTurnIndex { get; private set; }

    //added
    public Player CurrentPlayer { get { return playerList[CurrentPlayerTurnIndex]; } }

    public void SetPlayers(List<Player> players)
    {
        playerList = players;

        // Give all players the same amount of troops
        int troopsByPlayerAmount = GetTroopsByPlayerAmount(playerList.Count);
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[i].AddTroops(troopsByPlayerAmount);
        }
    }

    public void StartFirstTurn()
    {
        CurrentPlayerTurnIndex = 0;
        for (int i = 1; i < playerList.Count; i++)
        {
            playerList[i].IsTurn = false;
        }
        playerList[0].IsTurn = true;

        onPlayerTurnStartedEventArgs.lastPlayerInTurn = null;
        onPlayerTurnStartedEventArgs.playerInTurn = playerList[0];
        OnPlayerTurnStarted?.Invoke(this, onPlayerTurnStartedEventArgs);

        Debug.Log("Player " + playerList[0].PlayerName + " is now playing");
    }

    /// <summary>
    /// Go to the next turn
    /// </summary>
    public void GoNextTurn()
    {
        onPlayerTurnStartedEventArgs.lastPlayerInTurn = playerList[CurrentPlayerTurnIndex];
        for (int i = 0; i < playerList.Count; i++)
        {
            playerList[CurrentPlayerTurnIndex].IsTurn = false;
        }
        CurrentPlayerTurnIndex = (CurrentPlayerTurnIndex + 1) % playerList.Count;
        playerList[CurrentPlayerTurnIndex].IsTurn = true;

        onPlayerTurnStartedEventArgs.playerInTurn = playerList[CurrentPlayerTurnIndex];
        OnPlayerTurnStarted?.Invoke(this, onPlayerTurnStartedEventArgs);

        Debug.Log("Player " + playerList[CurrentPlayerTurnIndex].PlayerName + " is now playing");
    }

    /// <summary>
    /// Returns the amount of troops based on the amount of players
    /// </summary>
    /// <param name="playerAmount"></param>
    /// <returns></returns>
    private int GetTroopsByPlayerAmount(int playerAmount)
    {
        for (int i = troopsByPlayerAmountsSettings.Count - 1; i >= 0; i--)
        {
            if (playerAmount >= troopsByPlayerAmountsSettings[i].playerAmount)
                return troopsByPlayerAmountsSettings[i].troopsAmount;
        }
        return troopsByPlayerAmountsSettings[0].troopsAmount;
    }
}

[System.Serializable]
public class PlayerSelectorData
{
    public string playerName;
    public Color playerColor;
    public GameObject playerPrefab;
    public Vector3 playerPosition;

    [Button]
    public void CopyDataFromPrefab()
    {
        playerName = playerPrefab.name;
        playerPosition = playerPrefab.transform.position;
    }
}

[System.Serializable]
public class TroopsByPlayerAmount
{
    [FoldoutGroup("@playerAmount")]
    public int playerAmount;
    [FoldoutGroup("@playerAmount")]
    public int troopsAmount;
}