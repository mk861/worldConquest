using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using WorldDomination;

public class SetStateButton : MonoBehaviour
{
    public GameManager.State changeToState; 

    /// <summary>
    /// Sets the state of the game 
    /// </summary>
    public void SetState()
    {
        GameManager.Instance.SetState(changeToState);
    }
}
