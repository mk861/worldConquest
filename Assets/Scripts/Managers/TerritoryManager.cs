using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager Instance { get; private set; }

    //which player owns which territory
    private Dictionary<Territory, Player> territoryOwnership = new Dictionary<Territory, Player>();
    private TerritoryMonoBehaviour[] territoryMonoBehaviours;

    public List<Territory> Territories => territoryMonoBehaviours.Select(t => t.territory).ToList();

    /// <summary>
    /// calls this method when the instance of the script is loaded
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); //persist across scenes thinking?
        }

        territoryMonoBehaviours = FindObjectsOfType<TerritoryMonoBehaviour>();
    }

    /// <summary>
    /// Assigns the ownership of a territory to a specific player
    /// </summary>
    /// <param name="territory"></param>
    /// <param name="player"></param>
    public void AssignTerritory(Territory territory, Player player)
    {
        territoryOwnership[territory] = player;
        Debug.Log("Assigned " + territory + " to " + player);
    }


    /// <summary>
    /// Removes the ownership of a specific territory from a player
    /// </summary>
    /// <param name="territory"></param>
    public void RemoveTerritory(Territory territory)
    {
        if (!territoryOwnership.ContainsKey(territory))
            territoryOwnership.Remove(territory);
    }

/*    /// <summary>
    /// Changes the ownership o
    /// </summary>
    /// <param name="territory"></param>
    /// <param name="newOwner"></param>
    public void ChangeTerritoryOwnership(Territory territory, Player newOwner)
    {
        territoryOwnership[territory] = newOwner;
        //add logic
    }*/

    /// <summary>
    /// Returns the name of the player who owns a specific territory
    /// </summary>
    /// <param name="territory"></param>
    /// <returns> Player who owns territory, otherwise null </returns>
    public Player GetTerritoryOwner(Territory territory)
    {
        if (territoryOwnership.TryGetValue(territory, out Player owner))
        {
            if (territory.TroopsCount > 0)
                return owner;
        }
        Debug.Log("No owner for " + territory.TerritoryName);
        return null; //handle unclaimed territories somehow
    }

    /// <summary>
    /// Checks if the 2 territories are adjacent to each other 
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns> true if the territories are adjacent to each other, otherwise false</returns>
    public bool AreTerritoriesAdjacent(Territory t1, Territory t2)
    {
        return t1.TerritoryMonoBehaviour.IsAdjacentTo(t2);
    }

    /// <summary>
    /// Returns true if all territories are occupied
    /// </summary>
    /// <returns> true if all occupied, false otherwise </returns>
    public bool AreAllTerritoriesOccupied()
    {
        for (int i = 0; i < territoryMonoBehaviours.Length; i++)
        {
            if (GetTerritoryOwner(territoryMonoBehaviours[i].territory) == null)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Checks if a specific player owns all of the territories
    /// </summary>
    /// <param name="player"></param>
    /// <returns> true if player owns all, false otherwise </returns>
    public bool AreAllTerritoriesOwnedByPlayer(Player player)
    {
        for (int i = 0; i < territoryMonoBehaviours.Length; i++)
        {
            if (GetTerritoryOwner(territoryMonoBehaviours[i].territory) != player)
                return false;
        }
        return true;
    }

    /// <summary>
    /// Returns the player that won the game
    /// </summary>
    /// <returns> player who won the game, otherwise null </returns>
    public Player GetPlayerOwningAllTerritories()
    {
        List<Player> playerList = PlayerManager.Instance.playerList;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (AreAllTerritoriesOwnedByPlayer(playerList[i]))
                return playerList[i];
        }
        return null;
    }
}