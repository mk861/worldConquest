using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MonoBehaviour {
    private SpriteRenderer rend;
    private Material defaultMaterial;
    private Material newMaterial;

    /// <summary>
    /// Assigns components and calls whichContinent() when script loads
    /// </summary>
    void Start() {
        rend = GetComponent<SpriteRenderer>();
        defaultMaterial = rend.material;
        whichContinent();
    }

    /// <summary>
    /// Assigns material when mouse is over
    /// </summary>
    void OnMouseOver() {
        rend.material = newMaterial;
    }

    /// <summary>
    /// Assigns material back to default when mouse is removed
    /// </summary>
    void OnMouseExit() {
        rend.material = defaultMaterial;
    }

    /// <summary>
    /// Loads the correct material based on the tag that the object the mouse is currently over has
    /// </summary>
    private void whichContinent() {
        String currentTag = gameObject.tag;
        switch (currentTag) {
            case "North_America":
                newMaterial = Resources.Load<Material>("NorthAmericaBrighter");
                break;
            case "Europe":
                newMaterial = Resources.Load<Material>("EuropeBrighter");
                break;
            case "South_America":
                newMaterial = Resources.Load<Material>("SouthAmericaBrighter");
                break;
            case "Africa":
                newMaterial = Resources.Load<Material>("AfricaBrighter");
                break;
            case "Oceana":
                newMaterial = Resources.Load<Material>("OceanaBrighter");
                break;
            case "Asia":
                newMaterial = Resources.Load<Material>("AsiaBrighter");
                break;
        }
    }
}
