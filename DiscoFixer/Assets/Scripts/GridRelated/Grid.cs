using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField, Range(1,9)] private int height = 7;
    [SerializeField, Range(1,13)] private int width = 11;
    public static GameObject[,] grid;
    public List<Color> tileColors = new List<Color>();
    public int breakFrequency = 5;
    private int breakCountdown;
    public int pickupDelay = 10;
    private int pickupCountdown;
    private int everyOther = 0;
    private Color color1;
    private Color color2;
    

    private void Start()
    {
        GameEvents.beat.onBeat += ChangeColors;
        GameEvents.beat.onBeat += TileBreaker;
        GameEvents.beat.onBeat += PickupSpawner;
        breakCountdown = breakFrequency;
        pickupCountdown = pickupDelay;
        grid = new GameObject[width, height];
        
        var basePos = new Vector2(transform.position.x - width/2, transform.position.y - height/2);
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                var offset = new Vector2(i, j);
                var tile = Instantiate(tilePrefab, basePos + offset, transform.rotation);
                tile.GetComponent<SpriteRenderer>().material.color = tileColors[Random.Range(0, tileColors.Count)];
                grid[i, j] = tile;
            }
        }
    }

    


    private void PickupSpawner()
    {
        Debug.Log($"pickupCountdown: {pickupCountdown}");
        if (pickupCountdown == 0)
        {
            var tile = grid[Random.Range(0, width), Random.Range(0, height)];
            var tileScript = tile.GetComponent<Tile>();
            tileScript.hasWaterPickup = true;
            pickupCountdown = pickupDelay;
        }
        else
        {
            pickupCountdown--;
        }
    }

    private void ChangeColors()
    {
        // Random //
        foreach (var tile in grid)
        {
            var tileScript = tile.GetComponent<Tile>();
            if (!tileScript.isBreaking && !tileScript.isBroken)
            {
                tile.GetComponent<SpriteRenderer>().material.color = tileColors[Random.Range(0, tileColors.Count)];
            }
        }
        
        
        // Every other column //
        // for (int i = 0; i < grid.GetLength(0); i++)
        // {
        //     for (int j = 0; j < grid.GetLength(1); j++)
        //     {
        //         if (i % 2 == 0)
        //         {
        //             grid[i, j].GetComponent<SpriteRenderer>().material.color = color1;
        //         }
        //         else
        //         {
        //             grid[i, j].GetComponent<SpriteRenderer>().material.color = color2;
        //         }
        //     } 
        // }
        
        
    }

    private void TileBreaker()
    {
        if (breakCountdown > 0)
        {
            breakCountdown--;
        }
        else
        {
            breakCountdown = breakFrequency;
            BreakATile();
            
        }
    }
    
    private void BreakATile()
    {
        var alreadyBroken = true;
        while (alreadyBroken)
        {
            // Get a random tile
            var tile = grid[Random.Range(0, width), Random.Range(0, height)];
            var tileScript = tile.GetComponent<Tile>();
            // Check if chosen tile is already breaking or broken
            if (!tileScript.isBreaking && !tileScript.isBroken)
            {
                tileScript.isBreaking = true;
                alreadyBroken = false;
            }
        }
        
        
    }
    
}
