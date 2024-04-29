using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Dice : MonoBehaviour
{
    //array of dice sides
    private Sprite[] diceSides;
    //renderer to change sprites
    private SpriteRenderer rend;

    void Start() {
        rend = GetComponent<SpriteRenderer>();
        //dice sprites loaded from DiceSides 
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
    }

    //RollTheDice activated when dice clicked on
    private void OnMouseDown() {
        StartCoroutine("RollTheDice");
    }

    //coroutine declared 
    private IEnumerator RollTheDice() {
        int randomDiceSide = 0;
        int finalSide = 0;
        //20 iterations of random dice rolling before final side shown
        for (int i = 0; i <= 20; i++) {
            randomDiceSide = Random.Range(0, 6);
            //show dice side accoring to random value picked
            rend.sprite = diceSides[randomDiceSide];
            //wait 0.05seconds (float) before next iteration
            yield return new WaitForSeconds(0.05f);
        }

    finalSide += randomDiceSide; 
    Debug.Log(finalSide);
    }
}
