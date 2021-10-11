using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        Walking,
        Fixing
    }

    public bool alive = true;
    public TextMeshProUGUI scoreBox;
    private static Vector2 direction;
    public State state = State.Walking;
    private static Vector2 position = new Vector2(0, 0);
    private GameObject[,] gridSize;
    private static bool alreadyPressed = false;
    public int score = 0;
    public int repairPoints = 10;
    
    private Vector3 _goalPos = new Vector3(0, 0);

    // heat
    public static int heat = 0;

    // Couldn't always set grid in Start
    private bool _lateStart = false;

    // Giving access to the players world position 
    private static Vector3 _worldPosition;
    public static Vector3 WorldPos
    {
        get { return _worldPosition; }
        private set { }
    }
    
    private void Start()
    {
        GameEvents.beat.onBeat += Move;
        heat = 0;
    }

    private void _setGrid()
    {
        gridSize = Grid.grid;
        position = new Vector2(gridSize.GetLength(0) / 2, gridSize.GetLength(1) / 2);
        _lateStart = true;
    }
    
    private void Update()
    {
        if (alive == false)
        {
            GameEvents.beat.onBeat -= Move;
        }
        _worldPosition = transform.position;
        
        // Runs in the first time update runs
        if (!_lateStart)
        {
            _setGrid();
        }
        _controls();

        // lerp = smooth
        // Slerp = nice bounce but buggy
        Vector3 _currentPosition = Vector3.zero;
        _currentPosition.x = Mathf.Lerp(transform.position.x, _goalPos.x, 0.03f);
        _currentPosition.y = Mathf.Lerp(transform.position.y, _goalPos.y, 0.03f);

        transform.position = _currentPosition;
        // transform.position = Vector3.Slerp(transform.position, _goalPos, 0.03f);
    }

    public static bool pressedArrows()
    {
        return Input.GetAxisRaw("Horizontal") > 0.2 || Input.GetAxisRaw("Horizontal") < -0.2 ||
               (Input.GetAxisRaw("Vertical") > 0.2 || Input.GetAxisRaw("Vertical") < -0.2);
    }

    private void _controls()
    {
        if (!alreadyPressed)
        {
            if (pressedArrows())
            {
                alreadyPressed = true;
                direction = Vector2.zero;
            }
        }
        
        if (Input.GetAxis("Vertical") > 0) 
        { direction.y = 1; }
        if (Input.GetAxis("Vertical") < 0) 
        { direction.y = -1; }
        if (Input.GetAxis("Horizontal") < 0) 
        { direction.x = -1; }
        if (Input.GetAxis("Horizontal") > 0) 
        { direction.x = 1; }
    }

    private void Move()
    {
        var targetTile = position + direction;
        CheckTileStatus();

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
        CheckForPickup();
        CheckAndFixTile();
        SetDirectionToNormal();

    }

    private void Dead()
    {
        
    }

    private void CheckTileStatus()
    {
        RaycastHit2D hitTile = Physics2D.Raycast(WorldPos + new Vector3(direction.x, direction.y, 0), direction);
        Tile tile = null;

        if (hitTile)
        {
            tile = hitTile.collider.GetComponent<Tile>();
            if (tile.isBreaking && !tile.isBroken)
            {
                Debug.Log("Hit a breaking tile");
                tile.isBreaking = false;
                tile.state = tile.stages;
                GetPoints();
                tile = null;
                hitTile = Physics2D.Raycast(WorldPos + new Vector3(direction.x + direction.x, direction.y + direction.y, 0), direction);
                if (hitTile)
                    tile = hitTile.collider.GetComponent<Tile>();

                if (tile != null && !tile.isBroken)
                {
                    if (direction.x > 0)
                        direction.x = 2;
                    else if (direction.x < 0)
                        direction.x = -2;
                    if (direction.y > 0)
                        direction.y = 2;
                    else if (direction.y < 0)
                        direction.y = -2;
                }


            }
            tile = null;

        }
    }

    private static void SetDirectionToNormal()
    {
        if (direction.x > 0)
            direction.x = 1;
        else if (direction.x < 0)
            direction.x = -1;
        if (direction.y > 0)
            direction.y = 1;
        else if (direction.y < 0)
            direction.y = -1;
    }

    private void CheckForPickup()
    {
        var gameObject = Grid.grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)];
        var tileScript = gameObject.GetComponent<Tile>();
        if (tileScript.hasWaterPickup)
        {
            Debug.Log($"heat before: {heat}");
            
            var preliminaryHeat = heat - tileScript.waterPickupEffect;
            heat = preliminaryHeat < 0 ? 0 : preliminaryHeat;
               
            Debug.Log($"heat after: {heat}");
            tileScript.hasWaterPickup = false;
        }
    }

    private void GetPoints()
    {
        var tile = global::Grid.grid[(int)position.x, (int)position.y].GetComponent<Tile>();
        // commented out tile.state/2 as for the moment at least it is not conveyed good enough for the player to understand how it works.
        score += repairPoints * (5 - heat / 20); //* tile.state/2; 
        if(scoreBox != null)
        scoreBox.text = score.ToString();
        Debug.Log(score);
    }

    private void CheckAndFixTile()
    {
        var gameObject = Grid.grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)];
        var tileScript = gameObject.GetComponent<Tile>();
        if (!tileScript.isBreaking || tileScript.isBroken) return;
        tileScript.isBreaking = false;
        tileScript.state = tileScript.stages;
        GetPoints();
    }

}

