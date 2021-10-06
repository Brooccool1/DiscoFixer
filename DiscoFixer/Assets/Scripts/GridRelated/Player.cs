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
    public static Vector3 transformPos = new Vector3();
    
    private void Start()
    {
        gridSize = Grid.grid;
        position = new Vector2(gridSize.GetLength(0) / 2, gridSize.GetLength(1) / 2);
        transformPos = GetComponent<Transform>().position;
        GameEvents.beat.onBeat += Move;
    }

    private void Update()
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

        
        // if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
        // { direction = new Vector2(1, 1); }
        // if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)) 
        // { direction = new Vector2(1, -1); }
        // if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) 
        // { direction = new Vector2(-1, 1); }
        // if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) 
        // { direction = new Vector2(-1, -1); }
        
        Debug.Log(direction);
        
        // For every beat, Move()
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
        transform.position = Grid.grid[(int)position.x, (int)position.y].transform.position;
        
        alreadyPressed = false;
        CheckAndFixTile();

        
        // void FlipX()
        // {
        //     
        //     // if (targetTile.x == 1)
        //     // {
        //     //     targetTile.x = -1;
        //     // }
        //     // else
        //     // {
        //     //     targetTile.x = 1;
        //     // }
        // }
        // void FlipY()
        // {
        //     
        //     if (targetTile.y == 1)
        //     {
        //         targetTile.y = -1;
        //     }
        //     else
        //     {
        //         targetTile.y = 1;
        //     }
        // }
        
    }

    private static void CheckAndFixTile()
    {
        // if (Grid.grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].breaking);
        // {
        //     Grid.grid[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)].breaking = false;
        // }
    }

}

