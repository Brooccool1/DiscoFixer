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
    public List<Color> tileColors;

    private void Start()
    {
        Color[] colors = { Color.cyan, Color.blue, Color.green, Color.magenta, Color.red, Color.yellow, Color.white,  };
        tileColors.AddRange(colors);
        
        grid = new GameObject[width, height];
        
        var basePos = new Vector2(transform.position.x - width/2, transform.position.y - height/2);
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                var offset = new Vector2(i, j);
                var tile = Instantiate(tilePrefab, basePos + offset, transform.rotation);
                tile.GetComponent<SpriteRenderer>().color = tileColors[Random.Range(0, tileColors.Count)];
                grid[i, j] = tile;
            }
        }
    }


    
}
