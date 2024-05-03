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

    /// <summary>
    /// Constructor for territory class
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="sprite"></param>
    /// <param name="territoryMonoBehaviour"></param>
    public Territory(string id, string name, SpriteRenderer sprite, TerritoryMonoBehaviour territoryMonoBehaviour)
    {
        TerritoryID = id;
        TerritoryName = name;
        TerritorySprite = sprite;
        Troops = new List<Troop>();
        TerritoryMonoBehaviour = territoryMonoBehaviour;
    }

    /// <summary>
    /// Adds troops to to a territory
    /// </summary>
    /// <param name="troops"></param>
    public void AddTroops(List<Troop> troops)
    {
        foreach (Troop t in troops)
        {
            Troops.Add(t);
        }
    }

    /// <summary>
    /// Adds a single troop to a territory
    /// </summary>
    /// <param name="troop"></param>
    public void AddTroop(Troop troop)
    {
        Troops.Add(troop);
        TerritoryManager.Instance.AssignTerritory(this, Owner);
    }

    /// <summary>
    /// Removes troops from a given territory
    /// </summary>
    /// <param name="number"></param>
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
    }
}
