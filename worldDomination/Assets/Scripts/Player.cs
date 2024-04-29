using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerName {get; set;}
    //do I need color?
    public Color PlayerColor {get; set;}
    public GameObject playerPrefab;
    public List<Territory> TerritoriesOwned {get; private set;} = new List<Territory>();
    public int TroopsCount {get; private set; }
    public bool IsActive { get; set; } = true;
    public bool IsTurn { get; set; } = false;
    public bool HasWon { get; set; } = false;

    public Player(string name, Color color) {
        PlayerName = name;
        PlayerColor = color;
    }

    //add n number of troops to player
    public void AddTroops(int number) {
        TroopsCount += number;
        //make sure troops can't go below 0
        TroopsCount = Mathf.Max(TroopsCount, 0);
    }
    
    //remove n number of troops from player
    public void RemoveTroops(int number){
        TroopsCount -= number;
        //make sure troops can't go below 0
        TroopsCount = Mathf.Max(TroopsCount, 0);
    }

    //add territory to players ownership
    public void ControlTerritory(Territory territory) {
        if (!TerritoriesOwned.Contains(territory)) {
            TerritoriesOwned.Add(territory);
            //mark territory as controlled by player
        } //else
    }

    //remove territory from players ownership
    public void LoseTerritory(Territory territory) {
        if(TerritoriesOwned.Contains(territory)) {
            TerritoriesOwned.Remove(territory);
            //lose control of teritory
        } //else
    }

    //BeginTurn, EndTurn, Serialize/Deserialize???????
}
