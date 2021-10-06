using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        Walking,
        Fixing
    }
    
    public static Vector2 direction;
    public State state = State.Walking;
    public static Vector2 position = new Vector2(0, 0);
    public GameObject[,] gridSize;
    public static bool alreadyPressed = false;
    
    private Vector3 _goalPos = new Vector3(0, 0);


    // Couldn't always set grid in Start
    private bool _lateStart = false;
    
    private void Start()
    {
        GameEvents.beat.onBeat += Move;
    }

    private void _setGrid()
    {
        gridSize = Grid.grid;
        position = new Vector2(gridSize.GetLength(0) / 2, gridSize.GetLength(1) / 2);
        _lateStart = true;
    }
    
    private void Update()
    {
        // Runs in the first time update runs
        if (!_lateStart)
        {
            _setGrid();
        }
        _controlls();

        // lerp = smooth
        // Slerp = nice bounce but buggy
        Vector3 _currentPosition = Vector3.zero;
        _currentPosition.x = Mathf.Lerp(transform.position.x, _goalPos.x, 0.03f);
        _currentPosition.y = Mathf.Lerp(transform.position.y, _goalPos.y, 0.03f);

        transform.position = _currentPosition;
        // transform.position = Vector3.Slerp(transform.position, _goalPos, 0.03f);
    }

    private void _controlls()
    {
        if (Input.anyKey && !alreadyPressed)
        {
            alreadyPressed = true;
            direction = Vector2.zero;
        }
        
        if (Input.GetKey(KeyCode.UpArrow)) 
        { direction.y = 1; }
        if (Input.GetKey(KeyCode.DownArrow)) 
        { direction.y = -1; }
        if (Input.GetKey(KeyCode.LeftArrow)) 
        { direction.x = -1; }
        if (Input.GetKey(KeyCode.RightArrow)) 
        { direction.x = 1; }
    }

    private void Move()
    {
        var targetTile = position + direction;
        if (targetTile.x < 0 || targetTile.x > Grid.grid.GetLength(0)-1)
        {
            direction.x = -direction.x;
        }

        if (targetTile.y < 0 || targetTile.y > Grid.grid.GetLength(1)-1)
        {
            direction.y = -direction.y;
        }

        
        position += direction;
        _goalPos = Grid.grid[(int)position.x, (int)position.y].transform.position;
        
        alreadyPressed = false;
        CheckAndFixTile();

    }

    private static void CheckAndFixTile()
    {
        var gameObject = Grid.grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)];
        var tileScript = gameObject.GetComponent<Tile>();
        if (!tileScript.isBreaking) return;
        tileScript.isBreaking = false;
        tileScript.state = tileScript.stages;
    }

}

