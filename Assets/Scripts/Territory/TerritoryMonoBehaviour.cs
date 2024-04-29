/*using System.Collections.Generic;
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
}*/
























/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is attached to each territory sprite which uses an instance to Territory class to manage game logic for each territory.
//this script handles unity-specific functionality like user input (I think???????)

public class TerritoryMonoBehaviour : MonoBehaviour
{

    public Territory territory;
    public TextMesh troopCountText; //reference to TextMesh displaying troop counts but will probably change this

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string id = name; //logic to derive ID?
        string territoryName = name;
        territory = new Territory(id, territoryName, spriteRenderer);
    }

    void OnMouseDown()
    {
        territory.OnClick();
    }

    //experimental...method will be called whenever update of visuals needed
    public void UpdateVisuals()
    {
        if (territory.Owner != null)
        {
            territory.TerritorySprite.color = territory.Owner.PlayerColor;
        }
        troopCountText.text = territory.TroopCount.ToString();
    }
}
*/