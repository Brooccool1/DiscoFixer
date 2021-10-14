using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StopTilesBreaking : MonoBehaviour
{
    public static bool active;
    private int _activeTimer = 0;
    [SerializeField] private int _lifeSpan = 4;

    private void Start()
    {
        GameEvents.beat.onBeat += _onBeat;
    }

    private void _onBeat()
    {
        if (!active)
        {
            _activeTimer = _lifeSpan;
            return;
        } 
        
        _activeTimer--;
        active = _activeTimer > 0;
    }
}
