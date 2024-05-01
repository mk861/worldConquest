using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using WorldDomination;

public class SetStateButton : MonoBehaviour
{
    public GameManager.State changeToState; 

    public void SetState()
    {
        GameManager.Instance.SetState(changeToState);
    }
}
