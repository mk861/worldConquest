using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldDomination;

//this class is attached to each territory sprite which uses an instance to Territory class to manage game logic for each territory.
//this script handles unity-specific functionality like user input (I think???????)

public class TerritoryMonoBehaviour : MonoBehaviour
{
    public class OnClickEventArgs : System.EventArgs
    {
        public TerritoryMonoBehaviour clickedTerritory;
    }
    private OnClickEventArgs onClickEventArgs = new OnClickEventArgs();
    public static event System.EventHandler<OnClickEventArgs> OnClick;

    public Territory territory;
    public TerritoryMonoBehaviour[] adjacentTerritories;
    public TextMesh troopCountText; //reference to TextMesh displaying troop counts but will probably change this
    //added
    public Card territoryCard;

    /// <summary>
    /// Initialises variables when script is first called
    /// </summary>
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string id = Guid.NewGuid().ToString(); //logic to derive ID?
        string territoryName = name;
        territory = new Territory(id, territoryName, spriteRenderer, this);
    }

    /// <summary>
    /// Invokes onClick event when territory is clicked, passing current territory as the argument
    /// </summary>
    void OnMouseDown()
    {
        onClickEventArgs.clickedTerritory = this;
        OnClick?.Invoke(this, onClickEventArgs);
    }

    /// <summary>
    /// Check wether territory
    /// </summary>
    /// <param name="other"></param>
    /// <returns> An array of the adjacent territories <returns>
    public bool IsAdjacentTo(Territory other)
    {
        return Array.Exists(adjacentTerritories, t => t.territory == other);
    }
}
