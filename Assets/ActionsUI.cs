using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldDomination;

public class ActionsUI : SerializedMonoBehaviour
{
    public Dictionary<GameManager.State, GameObject> stateWindows = new();

    public GameObject toggleWindow;

    /// <summary>
    /// Creates OnStateChanged when script instance is loaded
    /// </summary>
    public void Awake()
    {
        GameManager.OnStateChanged += OnStateChanged;
    }

    /// <summary>
    /// Removes OnStateChanged when MonoBehaviour is destroyed
    /// </summary>
    private void OnDestroy()
    {
        GameManager.OnStateChanged -= OnStateChanged;
    }

    /// <summary>
    /// Called at the begining, sets the game state to idle to begin game
    /// </summary>
    public void Start()
    {
        OnStateChanged(GameManager.State.Idle);
    }

    /// <summary>
    /// Changes the UI window depending on the state that the game is in 
    /// </summary>
    /// <param name="obj"></param>
    private void OnStateChanged(GameManager.State obj)
    {
        bool exists = stateWindows.ContainsKey(obj);
        toggleWindow.SetActive(stateWindows.ContainsKey(obj));

        if (!exists)
            return;

        stateWindows.ForEach(o => o.Value.SetActive(false));
        stateWindows[obj].SetActive(true);
    }
}
