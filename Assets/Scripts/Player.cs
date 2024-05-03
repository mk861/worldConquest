using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldDomination;

public class Player : MonoBehaviour
{
    public string PlayerName { get; set; }
    //do I need color?
    public Color PlayerColor { get; set; }
    public GameObject troopPrefab;
    public List<Territory> TerritoriesOwned { get; private set; } = new List<Territory>();
    public int TroopsCount { get; private set; }
    public int SpawnedTroops { get; private set; }
    public bool IsActive { get; set; } = true;
    public bool IsTurn { get; set; } = false;
    public bool HasWon { get; set; } = false;

    public Player(string name, Color color)
    {
        PlayerName = name;
        PlayerColor = color;
    }

    //added
    public List<Card> Cards { get; private set; } = new List<Card>();
    
    //added
    /// <summary>
    /// Creates a new list of cards when script instance is loaded
    /// </summary>
    private void Awake()
    {
        Cards = new List<Card>();
    }

    /// <summary>
    /// Creates territory monobahaviour when enabled
    /// </summary>
    private void OnEnable()
    {
        TerritoryMonoBehaviour.OnClick += TerritoryMonoBehaviour_OnClick;
    }

    /// <summary>
    /// Places starting or extra troops on territory, based on current game state, when a territory is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TerritoryMonoBehaviour_OnClick(object sender, TerritoryMonoBehaviour.OnClickEventArgs e)
    {
        if (!IsTurn)
            return;

        if (GameManager.Instance.GameState == GameManager.State.PlacingStartingTroops)
        {
            PlacingStartingUnits(e.clickedTerritory.territory);
            return;
        }

        if (GameManager.Instance.GameState == GameManager.State.PlacingExtraTroops)
        {
            PlacingExtraUnits(e.clickedTerritory.territory);
            return;
        }
    }

    /// <summary>
    /// If available troops and game conditions are met, places starting troops on uncontrolled territory
    /// then moves on to next turn
    /// </summary>
    /// <param name="t"></param>
    /// <param name="location"></param>
    public void PlacingStartingUnits(Territory t, Transform location = null)
    {
        if (SpawnedTroops >= TroopsCount)
            return;

        if (TroopsCount <= 0)
        {
            Debug.Log("Not enough troops");
            return;
        }

        if (TerritoryManager.Instance.GetTerritoryOwner(t) != null)
        {
            Debug.Log("This territory is already controlled by " + TerritoryManager.Instance.GetTerritoryOwner(t).PlayerName);
            return;
        }

        PlaceTroop(t, location);
        GameManager.Instance.GoNextTurn();
    }

    /// <summary>
    /// Places extra troops on territory controlled by player if there are enough troops
    /// </summary>
    /// <param name="t"></param>
    public void PlacingExtraUnits(Territory t)
    {
        if (SpawnedTroops >= TroopsCount)
            return;

        if (TroopsCount <= 0)
        {
            Debug.Log("Not enough troops");
            return;
        }

        if (!AmIOwner(t))
        {
            Debug.Log("This territory is already controlled by "); //+ TerritoryManager.Instance.GetTerritoryOwner(t).PlayerName)
            return;
        }

        PlaceTroop(t);
    }

    /// <summary>
    /// Spawns troop at specific location or on mouse position, then assigns it to the territory and updates control
    /// </summary>
    /// <param name="t"></param>
    /// <param name="location"></param>
    void PlaceTroop(Territory t, Transform location = null)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (location != null)
            mousePosition = location.transform.position;
        mousePosition.z = 0;


        GameObject spawnedTroop = Instantiate(troopPrefab, mousePosition, Quaternion.identity, transform);
        var troop = spawnedTroop.GetComponent<Troop>();
        troop.Owner = this;
        troop.TerritoryAssigned = t;
        SpawnedTroops++;
        TerritoryManager.Instance.AssignTerritory(t, this);
        t.AddTroop(troop);
        Debug.Log("Territory " + t.TerritoryName + " now controlled by " + PlayerName);
    }

    /// <summary>
    /// Checks wether player is owner of a territory
    /// </summary>
    /// <param name="t"></param>
    /// <returns> true if player is owner, false if player isn't </returns>
    public bool AmIOwner(Territory t) => TerritoryManager.Instance.GetTerritoryOwner(t) == this;

    /// <summary>
    /// removes TerritoryMonoBehaviour onclick when MonoBehaviour is destroyed
    /// </summary>
    private void OnDisable()
    {
        TerritoryMonoBehaviour.OnClick -= TerritoryMonoBehaviour_OnClick;
    }

    //add n number of troops to player
    /// <summary>
    /// Adds specified number of troops to player
    /// </summary>
    /// <param name="number"></param>
    public void AddTroops(int number)
    {
        TroopsCount += number;
        //make sure troops can't go below 0
        TroopsCount = Mathf.Max(TroopsCount, 0);
    }

    //remove n number of troops from player
    /// <summary>
    /// Removes specified number of troops to player 
    /// </summary>
    /// <param name="number"></param>
    public void RemoveTroops(int number)
    {
        TroopsCount -= number;
        //make sure troops can't go below 0
        TroopsCount = Mathf.Max(TroopsCount, 0);
    }
}