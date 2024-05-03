using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private bool _timerActive;
    private float _currentTime;
    [SerializeField] private TMP_Text _text;

    // Start is called before the first frame update
    /// <summary>
    /// Sets current time to 0 when script first loaded
    /// </summary>
    void Start()
    {
        _currentTime = 0;
    }

    // Update is called once per frame
    /// <summary>
    /// If time is active, adds time to timer and updates it
    /// </summary>
    void Update()
    {
        if (_timerActive)
        {
            _currentTime = _currentTime + Time.deltaTime;
        }

        TimeSpan time = TimeSpan.FromSeconds(_currentTime);

        _text.text = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString(); 
    }

    /// <summary>
    /// Sets time to active
    /// </summary>
    public void StartTimer()
    {
        _timerActive = true; 
    }
}
