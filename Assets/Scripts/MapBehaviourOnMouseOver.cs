using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehaviour : MonoBehaviour {
    private SpriteRenderer rend;
    private Material defaultMaterial;
    private Material newMaterial;
    void Start() {
        rend = GetComponent<SpriteRenderer>();
        defaultMaterial = rend.material;
        whichContinent();
    }
    void OnMouseOver() {
        rend.material = newMaterial;
    }

    void OnMouseExit() {
        rend.material = defaultMaterial;
    }

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
