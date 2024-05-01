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

    public void Awake()
    {
        GameManager.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= OnStateChanged;
    }

    public void Start()
    {
        OnStateChanged(GameManager.State.Idle);
    }

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
