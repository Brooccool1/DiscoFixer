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
    // Breaking
    public int breakFrequency = 5;
    private int breakCountdown;
    // Water
    public int waterPickupDelay = 10;
    private int waterPickupCountdown;
    //Wiper
    public int wiperPickupDelay = 15;
    private int wiperPickupCountdown;
    // Freeze
    public int freezePickupDelay = 15;
    private int freezePickupCountdown;
    
    private Color color1;
    private Color color2;
    

    private void Start()
    {
        GameEvents.beat.onBeat += ChangeColors;
        GameEvents.beat.onBeat += TileBreaker;
        GameEvents.beat.onBeat += WaterPickupSpawner;
        GameEvents.beat.onBeat += WiperPickupSpawner;
        GameEvents.beat.onBeat += FreezePickupSpawner;
        breakCountdown = breakFrequency;
        waterPickupCountdown = waterPickupDelay;
        wiperPickupCountdown = wiperPickupDelay;
        freezePickupCountdown = freezePickupDelay;
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

    private void WaterPickupSpawner()
    {
        if (waterPickupCountdown == 0)
        {
            var tile = grid[Random.Range(0, width), Random.Range(0, height)];
            var tileScript = tile.GetComponent<Tile>();
            tileScript.hasWaterPickup = true;
            tileScript._waterStayTime = 10;
            waterPickupCountdown = waterPickupDelay;
        }
        else
        {
            waterPickupCountdown--;
        }
    }


    private void FreezePickupSpawner()
    {
        if (freezePickupCountdown == 0)
        {
            var tile = grid[Random.Range(0, width), Random.Range(0, height)];
            var tileScript = tile.GetComponent<Tile>();
            tileScript.hasFreezePickup = true;
            tileScript._freezeStayTime = 10;
            freezePickupCountdown = freezePickupDelay;
        }
        else
        {
            freezePickupCountdown--;
        }
    }

    public static void ActivateFreeze()
    {
        StopTilesBreaking.active = true;
    }
    
    private void WiperPickupSpawner()
    {
        if (wiperPickupCountdown == 0)
        {
            var tileScript = new Tile();
            var available = false;
            while (!available)
            {
                var tile = grid[Random.Range(0, width), Random.Range(0, height)];
                tileScript = tile.GetComponent<Tile>();
                if (tileScript.state == 9 && !tileScript.hasWaterPickup)
                {
                    available = true;
                }
            }
            
            tileScript.hasWiperPickup = true;
            wiperPickupCountdown = wiperPickupDelay;
        }
        else
        {
            wiperPickupCountdown--;
        }
    }

    public static int UseWiper()
    {
        var repaired = 0;
        foreach (var tile in grid)
        {
            var tileScript = tile.GetComponent<Tile>();
            if (!tileScript.isBreaking) continue;
            tileScript.state = tileScript.stages;
            tileScript.isBreaking = false;
            tileScript.hasWiperPickup = false;
            repaired++;
        }

        return repaired;
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
            
        }
    }
    
}
