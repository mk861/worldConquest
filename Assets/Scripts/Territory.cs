using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WorldDomination;

[System.Serializable]
public class Territory
{
    public string TerritoryID { get; private set; }
    public string TerritoryName { get; set; }
    public Player Owner
    {
        get
        {
            if (TroopsCount == 0)
                return null;
            return TerritoryManager.Instance.GetTerritoryOwner(this);
        }
    }
    public int TroopsCount
    {
        get
        {
            int amount = 0;
            for (int i = 0; i < Troops.Count; i++)
            {
                amount += (int)Troops[i].troopType;
            }
            return amount;
        }
    }
    public List<Troop> Troops { get; set; }
    public SpriteRenderer TerritorySprite { get; set; }
    public TerritoryMonoBehaviour TerritoryMonoBehaviour { get; private set; }

    public Territory(string id, string name, SpriteRenderer sprite, TerritoryMonoBehaviour territoryMonoBehaviour)
    {
        TerritoryID = id;
        TerritoryName = name;
        TerritorySprite = sprite;
        Troops = new List<Troop>();
        TerritoryMonoBehaviour = territoryMonoBehaviour;
    }

    //add troops to territory
    public void AddTroops(List<Troop> troops)
    {
        foreach (Troop t in troops)
        {
            Troops.Add(t);
        }
        //max number of troops????? 
        UpdateTroopCountDisplay();
    }

    public void AddTroop(Troop troop)
    {
        Troops.Add(troop);
        TerritoryManager.Instance.AssignTerritory(this, Owner);
        UpdateTroopCountDisplay();
    }

    //remove troops from territory
    public void RemoveTroops(int number)
    {
        Owner.RemoveTroops(number);
        for (int i = number - 1; i >= 0; i--)
        {
            Troops.RemoveAt(i);
        }

        if (Troops.Count == 0)
        {
            TerritoryManager.Instance.RemoveTerritory(this);
        }
        //ensure can't go below 0
        UpdateTroopCountDisplay();
    }


    //assign ownership of territory to different player
    public void ChangeOwnership(Player newOwner)
    {
        TerritoryManager.Instance.ChangeTerritoryOwnership(this, newOwner);
        UpdateTerritoryColor();
        //more logic needed
    }

    //METHODS BELOW MIGHT NOT STAY...CONSIDERING
    public void Highlight()
    {
        //highlight sprite
    }

    public void SetNormal()
    {
        //revert to default appearance
    }

    public void OnClick()
    {
        //handle OnClick events 
    }

    private void UpdateTroopCountDisplay()
    {
        //update label/ other UI elements with TroopCount probably?
    }

    private void UpdateTerritoryColor()
    {
        //change color to match owner???
        if (Owner != null)
        {
            TerritorySprite.color = Owner.PlayerColor;
        }
    }
}
