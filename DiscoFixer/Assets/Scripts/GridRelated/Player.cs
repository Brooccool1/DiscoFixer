using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Player : MonoBehaviour
{
    public enum State
    {
        Walking,
        Fixing
    }

    public static bool alive = true;
    public static bool falling = false;
    public TextMeshProUGUI scoreBox;
    public static Vector2 direction = new Vector2(0, 0);
    public static State state = State.Walking;
    private static Vector2 position = new Vector2(0, 0);
    private GameObject[,] gridSize;
    private static bool alreadyPressed = false;
    public int score = 0;
    public int repairPoints = 10;
    public int autoHeatIncrease = 2;

    private Vector3 _goalPosition;
    
    private Vector3 _goalPos;

    // heat and Fire effect
    public static int heat = 0;
    private VisualEffect _fire;
    
    // Number of inputs, resets after 2 inputs
    private int _numOfInputs = 0;

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
        direction = new Vector2(0, -1);
        
        GameEvents.beat.onBeat += Move;
        GameEvents.beat.onBeat += AddHeatEveryBeat;
        _fire = GetComponentInChildren<VisualEffect>();
        heat = 0;
        alive = true;
        falling = false;
        
        ScoreKeeper._dead = true;
        ScoreKeeper.score = 0;
    }

    private void AddHeatEveryBeat()
    {
        heat += autoHeatIncrease;
    }

    private void _setGrid()
    {
        gridSize = Grid.grid;
        position = new Vector2(gridSize.GetLength(0) / 2, gridSize.GetLength(1) / 2);
        _lateStart = true;
    }

    private void _burning()
    {
        if (heat > 60)
        {
            _fire.enabled = true;
        }
        else
        {
            _fire.enabled = false;
        }
    }
    
    private void FixedUpdate()
    {
        if (alive == false)
        {
            GameEvents.beat.onBeat -= Move;
            GoToNonTile();
            Invoke("_dead", 1);
        }
        _burning();
        
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
        _currentPosition.x = Mathf.Lerp(transform.position.x, _goalPos.x, 0.2f);
        _currentPosition.y = Mathf.Lerp(transform.position.y, _goalPos.y, 0.2f);

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
        
        
    }

    private void FallDown()
    {
        var sprite = transform.GetChild(0).transform;
        var targetScale = sprite.localScale * 0.1f;
        var intermediateScale = Vector3.zero;
        intermediateScale.x = Mathf.Lerp(sprite.localScale.x, targetScale.x, 0.3f * Time.deltaTime);
        intermediateScale.y = Mathf.Lerp(sprite.localScale.y, targetScale.y, 0.3f * Time.deltaTime);
        sprite.localScale = intermediateScale;

        sprite.Rotate(0,0,10);
        var color = transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        sprite.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, color.a * 0.99f);
    }

    public static bool pressedArrows()
    {
        return Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0 ||
               Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical") < 0;
    }

    private void _controls()
    {
        if (!alive) return;
        
        if (pressedArrows())
        {
            if (!alreadyPressed)
            {
                alreadyPressed = true;
                direction = Vector2.zero;
                _numOfInputs = 0;
            }

            if (_numOfInputs > 3)
            {
                _numOfInputs = 0;
                direction = Vector2.zero;
            }
        }
        
        if (Input.GetAxis("Vertical") > 0) 
        {
            direction.y = 1;
            _numOfInputs++;
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            direction.y = -1; 
            _numOfInputs++;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            direction.x = -1; 
            _numOfInputs++;
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            direction.x = 1;
            _numOfInputs++;
        }

    }

    private void Move()
    {
        state = State.Walking;
        var targetTile = position + direction;
        CheckTileStatus();

        if (targetTile.x < 0 || targetTile.x > Grid.grid.GetLength(0)-1)
        {
            falling = true;
            alive = false;
            //direction.x = -direction.x;
        }

        if (targetTile.y < 0 || targetTile.y > Grid.grid.GetLength(1)-1)
        {
            alive = false;
            falling = true;
            //direction.y = -direction.y;
        }


        if (alive)
        {
            position = _goalPosition;
            _goalPos = Grid.grid[(int)position.x, (int)position.y].transform.position;
        }
        
        
        
        alreadyPressed = false;
        CheckForPickup(position);
        CheckAndFixTile();
        SetDirectionToNormal();

    }
    

    private void CheckTileStatus()
    {
        Vector2 currDir = direction;
        Vector2 addDir = direction;
        
        _goalPosition = Vector3.zero;
        
        while (true)
        {
            RaycastHit2D hitTile = Physics2D.Raycast(WorldPos + new Vector3(currDir.x, currDir.y, 0), addDir);
            Tile tile = null;


            if (hitTile)
            {
                tile = hitTile.collider.GetComponent<Tile>();
                if (tile.isBroken)
                {
                    if (_avoidHoles())
                    {
                        break;
                    }
                    alive = false;
                    falling = true;
                    break;
                }

                if (tile.hasWaterPickup)
                {
                    var preliminaryHeat = heat - tile.waterPickupEffect;
                    heat = preliminaryHeat < 0 ? 0 : preliminaryHeat;
                    tile.hasWaterPickup = false;
                }

                if (tile.isBreaking && !tile.isBroken)
                {
                    state = State.Fixing;
                    tile.isBreaking = false;
                    tile.state = tile.stages;
                    GetPoints();
                    
                    for (int i = 0; i < Grid.grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < Grid.grid.GetLength(1); j++)
                        {
                            if (Grid.grid[i, j].transform.position == tile.transform.position)
                            {
                                _goalPosition = new Vector3(i, j);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (_goalPosition == Vector3.zero)
                    {
                        _goalPosition = position + direction;
                        break;
                    }

                    GameObject[,] grid = Grid.grid;
                    if (direction.x > 0)
                    {
                        if (_goalPosition.x < grid.GetLength(0) - 1 && !grid[(int)_goalPosition.x + 1, (int)_goalPosition.y].GetComponent<Tile>().isBroken)
                        {
                            _goalPosition.x++;
                        }
                    }
                    else if (direction.x < 0)
                    {
                        if (_goalPosition.x > 0 && !grid[(int)_goalPosition.x - 1, (int)_goalPosition.y].GetComponent<Tile>().isBroken)
                        {
                            _goalPosition.x--;
                        }
                    }
                    if (direction.y > 0)
                    {
                        if (_goalPosition.y < grid.GetLength(1) - 1 && !grid[(int)_goalPosition.x, (int)_goalPosition.y + 1].GetComponent<Tile>().isBroken)
                        {
                            _goalPosition.y++;
                        }
                    }
                    else if (direction.y < 0)
                    {
                        if (_goalPosition.y > 0 && !grid[(int)_goalPosition.x, (int)_goalPosition.y - 1].GetComponent<Tile>().isBroken)
                        {
                            _goalPosition.y--;
                        }
                    }

                    break;
                }

                tile = null;
            }
            else
            {
                break;
            }

            currDir += addDir;
        }
        
        bool _avoidHoles()
        {
            if (currDir.x > 1 || currDir.y > 1)
            {
                return true;
            }
            if (currDir.x < -1 || currDir.y < -1)
            {
                return true;
            }

            return false;
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

    private void CheckForPickup(Vector2 pos)
    {
        var gameObject = Grid.grid[Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)];
        var tileScript = gameObject.GetComponent<Tile>();
        if (tileScript.hasWaterPickup)
        {
            Debug.Log($"heat before: {heat}");
            
            var preliminaryHeat = heat - tileScript.waterPickupEffect;
            heat = preliminaryHeat < 0 ? 0 : preliminaryHeat;
               
            Debug.Log($"heat after: {heat}");
            tileScript.hasWaterPickup = false;
        }

        if (tileScript.hasWiperPickup)
        {
            var repairedTiles =Grid.UseWiper();
            for (int i = 0; i < repairedTiles; i++)
            {
                GetPoints();
            }

            tileScript.hasWiperPickup = false;
        }
    }

    private void GetPoints()
    {
        //var tile = global::Grid.grid[(int)position.x, (int)position.y].GetComponent<Tile>();
        // commented out tile.state/2 as for the moment at least it is not conveyed good enough for the player to understand how it works.
        score += repairPoints * (3 - heat / 33); //* tile.state/2; 
        if (scoreBox != null)
        {
            scoreBox.text = score.ToString();
        }

        ScoreKeeper.score = score;
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

    private void _dead()
    {
        SceneManager.LoadScene("DeathScreen");
    }
}

