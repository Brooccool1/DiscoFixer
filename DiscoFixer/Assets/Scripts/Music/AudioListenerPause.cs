using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerPause : MonoBehaviour
{
    private AudioListener _listener;

    private void Start()
    {
        _listener = GetComponent<AudioListener>();
    }

    void Update()
    {
        if (Time.timeScale < 1)
        {
            AudioListener.pause = true;
        }
        else
        {
            AudioListener.pause = false;
        }
    }
}
