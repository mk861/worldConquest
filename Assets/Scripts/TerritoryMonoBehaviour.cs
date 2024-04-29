using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string id = Guid.NewGuid().ToString(); //logic to derive ID?
        string territoryName = name;
        territory = new Territory(id, territoryName, spriteRenderer, this);
    }

    void OnMouseDown()
    {
        territory.OnClick();

        onClickEventArgs.clickedTerritory = this;
        OnClick?.Invoke(this, onClickEventArgs);
    }

    //experimental...method will be called whenever update of visuals needed
    public void UpdateVisuals()
    {
        if (territory.Owner != null)
        {
            territory.TerritorySprite.color = territory.Owner.PlayerColor;
        }
        troopCountText.text = territory.TroopsCount.ToString();
    }

    public bool IsAdjacentTo(Territory other)
    {
        return Array.Exists(adjacentTerritories, t => t.territory == other);
    }
}
