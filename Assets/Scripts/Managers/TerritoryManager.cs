using System.Collections.Generic;
using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager Instance { get; private set; }

    //which player owns which territory
    private Dictionary<Territory, Player> territoryOwnership = new Dictionary<Territory, Player>();
    private TerritoryMonoBehaviour[] territoryMonoBehaviours;

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

    public void AssignTerritory(Territory territory, Player player)
    {
        territoryOwnership[territory] = player;
        Debug.Log("Assigned " + territory + " to " + player);
    }

    //remove a territory from a player
    public void RemoveTerritory(Territory territory)
    {
        if (!territoryOwnership.ContainsKey(territory))
            territoryOwnership.Remove(territory);
        //add logic
    }

    //change ownership of a territory
    public void ChangeTerritoryOwnership(Territory territory, Player newOwner)
    {
        territoryOwnership[territory] = newOwner;
        //add logic
    }

    //which player owns a territory
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
    /// Are the 2 territories connected?
    /// </summary>
    /// <param name="t1"></param>
    /// <param name="t2"></param>
    /// <returns></returns>
    public bool AreTerritoriesAdjacent(Territory t1, Territory t2)
    {
        return t1.TerritoryMonoBehaviour.IsAdjacentTo(t2);
    }

    /// <summary>
    /// Return true if all territories are occupied
    /// </summary>
    /// <returns></returns>
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
    /// Return true if all territories are owned by the the player
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
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
    /// Return the player that owns all territories, otherwise return null (Basically return who won the game)
    /// </summary>
    /// <returns></returns>
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

    // Possibly more methods to handle gameplay mechanics related to territories
}