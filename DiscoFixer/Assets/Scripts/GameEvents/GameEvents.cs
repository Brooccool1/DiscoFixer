using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents beat;

    private void Awake()
    {
        beat = this;
    }

    public event Action onBeat;
    public void OnBeat()
    {
        if (onBeat != null)
        {
            onBeat.Invoke();
        }
    }
}
