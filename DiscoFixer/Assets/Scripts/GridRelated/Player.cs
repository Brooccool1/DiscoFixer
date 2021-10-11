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
    public bool falling = false;
    public TextMeshProUGUI scoreBox;
    public static Vector2 direction = new Vector2(0, 0);
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
    
    private void FixedUpdate()
    {
        if (alive == false)
        {
            GameEvents.beat.onBeat -= Move;
            GoToNonTile();
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
        _currentPosition.x = Mathf.Lerp(transform.position.x, _goalPos.x, 0.1f);
        _currentPosition.y = Mathf.Lerp(transform.position.y, _goalPos.y, 0.1f);

        transform.position = _currentPosition;
        // transform.position = Vector3.Slerp(transform.position, _goalPos, 0.03f);
        if (heat >= 100)
        {
            alive = false;
        }
        if (falling)
        {
            FallDown();  
        }
    }

    private void GoToNonTile()
    {
        // var prelPosition = new Vector3(direction.x, direction.y, 0);
        // transform.position += prelPosition;
        
        var targetPosition = new Vector3(direction.x, direction.y, 0) + transform.position;
        var intermediatePos = Vector3.zero;
        intermediatePos.x = Mathf.Lerp(transform.position.x, targetPosition.x, 0.1f);
        intermediatePos.y = Mathf.Lerp(transform.position.y, targetPosition.y, 0.1f);
        transform.position = intermediatePos;
        
        falling = true;
    }

    private void FallDown()
    { 
        var targetScale = transform.localScale * 0.1f;
        var intermediateScale = Vector3.zero;
        intermediateScale.x = Mathf.Lerp(transform.localScale.x, targetScale.x, 0.3f * Time.deltaTime);
        intermediateScale.y = Mathf.Lerp(transform.localScale.y, targetScale.y, 0.3f * Time.deltaTime);
        transform.localScale = intermediateScale;

        transform.Rotate(0,0,10);
        var color = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, color.a * 0.99f);
    }

    public static bool pressedArrows()
    {
        return Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0 ||
               Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical") < 0;
    }

    private void _controls()
    {
        if (!alive) return;
        
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
            alive = false;
            //direction.x = -direction.x;
        }

        if (targetTile.y < 0 || targetTile.y > Grid.grid.GetLength(1)-1)
        {
            alive = false;
            //direction.y = -direction.y;
        }


        if (alive)
        {
            position += direction;
            _goalPos = Grid.grid[(int)position.x, (int)position.y].transform.position;
        }
        
        
        
        alreadyPressed = false;
        CheckForPickup();
        CheckAndFixTile();
        SetDirectionToNormal();

    }
    

    private void CheckTileStatus()
    {
        RaycastHit2D hitTile = Physics2D.Raycast(WorldPos + new Vector3(direction.x, direction.y, 0), direction);
        Tile tile = null;

        if (hitTile)
        {
            tile = hitTile.collider.GetComponent<Tile>();
            if (tile.isBroken)
            {
                alive = false;
                return;
            }  
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

