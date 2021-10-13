using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileDestroyer : MonoBehaviour
{
    [SerializeField] private int _percentageOfBreaking = 2;
    [SerializeField] private int _beatsToSkip = 5;

    [Header("Normal sprites")] 
    [SerializeField] private List<Sprite> _normalSprites;
    
    [Header("Broken sprites")] 
    [SerializeField] private List<Sprite> _brokenSprites;

    // Index in List
    private int _currentSprite;
    
    // Start of update
    private bool _started = false;
    
    // Have hit the floor
    private bool _hitFloor;

    private SpriteRenderer _renderer;
    
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
        _renderer = GetComponent<SpriteRenderer>();
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
        _renderer = GetComponent<SpriteRenderer>();
        
        _beatSkip--;
        if (_beatSkip <= 0)
        {
            _grid = Grid.grid;
            
            // Effects
            _currentSprite = Random.Range(0, _normalSprites.Count);
            _hitFloor = false;
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, 255);
            _renderer.sprite = _normalSprites[_currentSprite];
            
            _collider.enabled = false;
            _beatSkip = _beatsToSkip;

            // Get all non destroyed tiles
            List<Vector3> tiles = new List<Vector3>();
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    if (!_grid[i, j].GetComponent<Tile>().isBroken)
                    {
                        tiles.Add(_grid[i, j].transform.position);
                    }
                }
            }

            int index = Random.Range(0, tiles.Count);
            _goalPosition = tiles[index];
            transform.position = tiles[index] + new Vector3(0, 20);
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

        // Falling
        if (Vector3Int.FloorToInt(transform.position) != Vector3Int.FloorToInt(_goalPosition))
        {
            transform.position = Vector3.MoveTowards(transform.position, _goalPosition, 0.5f);
        }
        
        // Hit floor
        else
        {
            if (!_hitFloor)
            {
                _collider.enabled = true;
                for (int i = 0; i < _gridSize[0]; i++)
                {
                    for (int j = 0; j < _gridSize[1]; j++)
                    {
                        if (_collider.OverlapPoint(_grid[i, j].transform.position))
                        {
                            // Finding tiles in area
                            if (Random.Range(0, 101) < _percentageOfBreaking)
                            {
                                Grid.grid[i, j].GetComponent<Tile>().isBreaking = true;
                            }
                        }
                    }
                }
            }
            _renderer = GetComponent<SpriteRenderer>();

            // Fadeing
            _renderer.sprite = _brokenSprites[_currentSprite];
            _renderer.color = new Color(_renderer.color.r, _renderer.color.g, _renderer.color.b, _renderer.color.a * 0.9f);
            _hitFloor = true;

        }
    }
}
