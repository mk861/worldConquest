using System.Collections.Generic;
using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager Instance { get; private set; }

    //which player owns which territory
    private Dictionary<Territory, Player> territoryOwnership = new Dictionary<Territory, Player>();

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
    }

    public void AssignTerritory(Territory territory, Player player)
    {
        if (territoryOwnership.ContainsKey(territory))
        {
            //handle reassignment here?
        }

        territoryOwnership[territory] = player;
        //moRE logic to initialize the territory for the player (idk if we need it) 
    }

    //remove a territory from a player
    public void RemoveTerritory(Territory territory)
    {
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
            return owner;
        }
        return null; //handle unclaimed territories somehow
    }

    // Possibly more methods to handle gameplay mechanics related to territories
}