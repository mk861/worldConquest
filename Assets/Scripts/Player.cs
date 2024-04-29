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

    private void OnEnable()
    {
        TerritoryMonoBehaviour.OnClick += TerritoryMonoBehaviour_OnClick;
    }

    private void TerritoryMonoBehaviour_OnClick(object sender, TerritoryMonoBehaviour.OnClickEventArgs e)
    {
        if (!IsTurn)
            return;

        // If we are in placing starting troops, place 1 troop at the click location
        if (SpawnedTroops < TroopsCount && GameManager.Instance.GameState == GameManager.State.PlacingStartingTroops)
        {
            if (TroopsCount <= 0)
            {
                Debug.Log("Not enough troops");
                return;
            }

            if (TerritoryManager.Instance.GetTerritoryOwner(e.clickedTerritory.territory) != null)
            {
                Debug.Log("This territory is already controlled by " + TerritoryManager.Instance.GetTerritoryOwner(e.clickedTerritory.territory).PlayerName);
                return;
            }

            PlaceTroop();
            GameManager.Instance.GoNextTurn();
        }
        else if (SpawnedTroops < TroopsCount && GameManager.Instance.GameState == GameManager.State.PlacingExtraTroops)
        {
            if (TroopsCount <= 0)
            {
                Debug.Log("Not enough troops");
                return;
            }

            Player territoryOwner = TerritoryManager.Instance.GetTerritoryOwner(e.clickedTerritory.territory);
            if (territoryOwner != this)
            {
                Debug.Log("This territory is already controlled by " + TerritoryManager.Instance.GetTerritoryOwner(e.clickedTerritory.territory).PlayerName);
                return;
            }

            PlaceTroop();
        }

        void PlaceTroop()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            GameObject spawnedTroop = Instantiate(troopPrefab, mousePosition, Quaternion.identity, transform);
            var troop = spawnedTroop.GetComponent<Troop>();
            troop.Owner = this;
            troop.TerritoryAssigned = e.clickedTerritory.territory;
            SpawnedTroops++;
            TerritoryManager.Instance.AssignTerritory(e.clickedTerritory.territory, this);
            e.clickedTerritory.territory.AddTroop(troop);
            Debug.Log("Territory " + e.clickedTerritory.territory.TerritoryName + " now controlled by " + PlayerName);
        }
    }

    private void OnDisable()
    {
        TerritoryMonoBehaviour.OnClick -= TerritoryMonoBehaviour_OnClick;
    }

    public Player(string name, Color color)
    {
        PlayerName = name;
        PlayerColor = color;
    }

    //add n number of troops to player
    public void AddTroops(int number)
    {
        TroopsCount += number;
        //make sure troops can't go below 0
        TroopsCount = Mathf.Max(TroopsCount, 0);
    }

    //remove n number of troops from player
    public void RemoveTroops(int number)
    {
        TroopsCount -= number;
        //make sure troops can't go below 0
        TroopsCount = Mathf.Max(TroopsCount, 0);
    }

    //add territory to players ownership
    public void ControlTerritory(Territory territory)
    {
        if (!TerritoriesOwned.Contains(territory))
        {
            TerritoriesOwned.Add(territory);
            //mark territory as controlled by player
        } //else
    }

    //remove territory from players ownership
    public void LoseTerritory(Territory territory)
    {
        if (TerritoriesOwned.Contains(territory))
        {
            TerritoriesOwned.Remove(territory);
            //lose control of teritory
        } //else
    }

    //BeginTurn, EndTurn, Serialize/Deserialize???????
}
