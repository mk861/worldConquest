using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is attached to each territory sprite which uses an instance to Territory class to manage game logic for each territory.
//this script handles unity-specific functionality like user input (I think???????)

public class TerritoryMonoBehaviour : MonoBehaviour {

    public Territory territory;
    public TextMesh troopCountText; //reference to TextMesh displaying troop counts but will probably change this

    void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        string id = name; //logic to derive ID?
        string territoryName = name;
        territory = new Territory(id, territoryName, spriteRenderer);
    }

    void OnMouseDown() {
        territory.OnClick();
    }

    //experimental...method will be called whenever update of visuals needed
    public void UpdateVisuals() {
        if(territory.Owner != null) {
            territory.TerritorySprite.color = territory.Owner.PlayerColor;
        }
        troopCountText.text = territory.TroopCount.ToString();
    }
}
