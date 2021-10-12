using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileDestroyer : MonoBehaviour
{
    [SerializeField] private int _percentageOfBreaking = 2;
    [SerializeField] private int _beatsToSkip = 5;
    
    // Start of update
    private bool _started = false;
    
    private GameObject[,] _grid;
    private int[] _gridSize = new int[2];
    private int _beatSkip = 0;

    private CircleCollider2D _collider;
    
    private Vector3 _goalPosition = new Vector3(100, 100);
    private Vector3 _startPosition;
    
    void Start()
    {
        _collider = GetComponent<CircleCollider2D>();
        GameEvents.beat.onBeat += _drop;
    }

    // Starts in update to give the grid time to be created 
    private void _lateStart()
    {
        _grid = Grid.grid;
        _gridSize[0] = _grid.GetLength(0);
        _gridSize[1] = _grid.GetLength(1);
    }

    private void _drop()
    {
        _beatSkip--;
        if (_beatSkip <= 0)
        {
            _collider.enabled = false;
            _beatSkip = _beatsToSkip;

            int x = Random.Range(0, _gridSize[0]);
            int y = Random.Range(0, _gridSize[1]);
            _goalPosition = _grid[x, y].transform.position;
            transform.position = _grid[x, y].transform.position + new Vector3(0, 20);
            _startPosition = transform.position;
        }
    }
    
    void FixedUpdate()
    {
        if (!_started)
        {
            _lateStart();
            _started = true;
        }

        if (Vector3Int.FloorToInt(transform.position) != Vector3Int.FloorToInt(_goalPosition))
        {
            transform.position = Vector3.MoveTowards(transform.position, _goalPosition, 0.5f);
        }
        else
        {
            _collider.enabled = true;
            for (int i = 0; i < _gridSize[0]; i++)
            {
                for (int j = 0; j < _gridSize[1]; j++)
                {
                    if (_collider.OverlapPoint(_grid[i, j].transform.position))
                    {
                        if (Random.Range(0, 101) < _percentageOfBreaking)
                        {
                            Grid.grid[i, j].GetComponent<Tile>().isBreaking = true;
                        }
                    }
                }
            }

            transform.position = new Vector3(100, 100);
            _goalPosition = new Vector3(100, 100);
        }
    }
}
