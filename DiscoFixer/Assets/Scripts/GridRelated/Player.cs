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
    public static Vector2 position = new Vector2(0, 1);
    public GameObject[,] gridSize;
    public static bool alreadyPressed = false;

    private void Start()
    {
        gridSize = Grid.grid;
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

    private static void Move()
    {
        position += direction;
        alreadyPressed = false;
    }

}

