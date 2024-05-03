using System;
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

    /// <summary>
    /// Assigns components when script is first loaded
    /// </summary>
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        //dice sprites loaded from DiceSides 
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
    }

    //RollTheDice activated when dice clicked on
    /// <summary>
    /// Starts dice rolling coroutine when dice is clicked on
    /// </summary>
    private void OnMouseDown()
    {
        StartCoroutine(RollTheDice());
    }

    //coroutine declared 
    /// <summary>
    /// Coroutine that rolls the dice, picking a random side
    /// </summary>
    /// <param name="callback"></param>
    /// <returns> wait time </returns>
    public IEnumerator RollTheDice(Action<int> callback = null)
    {
        int randomDiceSide = 0;
        int finalSide = 0;
        //20 iterations of random dice rolling before final side shown
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(0, 6);
            //show dice side accoring to random value picked
            rend.sprite = diceSides[randomDiceSide];
            //wait 0.05seconds (float) before next iteration
            yield return new WaitForSeconds(0.05f);
        }

        finalSide = randomDiceSide + 1;
        callback?.Invoke(finalSide);
        Debug.Log(finalSide);
    }
}
