using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StopTilesBreaking : MonoBehaviour
{
    private Tile[,] _tiles;
    private bool _active;
    private int _activeTimer = 0;
    private int _lifeSpan = 4;

    private void Start()
    {
        GameEvents.beat.onBeat += _onBeat;
    }

    private void _onBeat()
    {
        if (!_active) return;
        
        _activeTimer--;
        _active = _activeTimer > 0;
    }
    
    void Update()
    {

        
        
        // Get tiles
        _tiles = new Tile[Grid.grid.GetLength(0), Grid.grid.GetLength(1)];
        for (int i = 0; i < Grid.grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.grid.GetLength(1); j++)
            {
                _tiles[i, j] = Grid.grid[i, j].GetComponent<Tile>();
            }
        }
    }
}
