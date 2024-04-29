using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Territory : MonoBehaviour
{
    public string TerritoryID { get; private set; }
    public string TerritoryName { get; set; }
    public Player Owner { get; set; }
    public int TroopCount {get; set; }
    public SpriteRenderer TerritorySprite { get; set; }
    
    public Territory(string id, string name, SpriteRenderer sprite) {
        TerritoryID = id;
        TerritoryName = name;
        TerritorySprite = sprite;
        TroopCount = 0;
    }

    //add troops to territory
    public void AddTroops(int number) {
        TroopCount += number;
        //max number of troops????? 
        UpdateTroopCountDisplay();
    }

    //remove troops from territory
    public void RemoveTroops(int number) {
        TroopCount -= number;
        //ensure can't go below 0
        TroopCount = Mathf.Max(TroopCount, 0); //or should it be 1???
        UpdateTroopCountDisplay();
    }

    //assign ownership of territory to different player
    public void ChangeOwnership(Player newOwner) {
        Owner = newOwner;
        UpdateTerritoryColor();
        //more logic needed
    }

    //METHODS BELOW MIGHT NOT STAY...CONSIDERING
    public void Highlight() {
        //highlight sprite
    }

    public void SetNormal() {
        //revert to default appearance
    }

    public void OnClick() {
        //handle OnClick events 
    }

    private void UpdateTroopCountDisplay() {
        //update label/ other UI elements with TroopCount probably?
    }

    private void UpdateTerritoryColor() {
        //change color to match owner???
        if (Owner != null) {
        TerritorySprite.color = Owner.PlayerColor;
        }
    }
}
