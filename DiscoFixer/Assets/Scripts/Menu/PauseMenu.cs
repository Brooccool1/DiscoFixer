using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    private float _timeScale = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _timeScale = _timeScale > 0 ? 0 : 1;
        }
        Time.timeScale = _timeScale;

        if (Time.timeScale < 1)
        {
            _canvas.SetActive(true);
        }
        else
        {
            _canvas.SetActive(false);
        }
    }
}
