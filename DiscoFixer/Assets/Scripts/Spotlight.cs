using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spotlight : MonoBehaviour
{
    private Vector2 _GoalPosition = Vector2.zero;
    
    // Bool for axis, true = y; false = x;
    [SerializeField] private bool _yAxis = true;
    
    // Beats to skip
    private int _offBeat = 0;
    
    // Color will be white for a while then turn to red
    private int _charge = 20;

    private void Start()
    {
        GameEvents.beat.onBeat += _onBeat;
    }

    private void _onBeat()
    {
        _setPosition();
        _loadingBeam();
    }
    
    private void _setPosition()
    {
        if (_offBeat <= 0)
        {
            if (_yAxis)
            {
                int rand = Random.Range(0, Grid.grid.GetLength(0));
                _GoalPosition = new Vector2(Grid.grid.GetLength(0) / 2 - rand, Grid.grid.GetLength(1) * 0.6f);
            }
            else
            {
                int rand = Random.Range(0, Grid.grid.GetLength(1));
                _GoalPosition = new Vector2(Grid.grid.GetLength(0) * 0.6f, Grid.grid.GetLength(1) / 2 - rand);
            }

            _offBeat = 5;
        }

        _offBeat--;
    }

    private void _loadingBeam()
    {
        _charge--;
        
        GetComponent<SpriteRenderer>().color = new Color(_charge, _charge * 0.1f, _charge * 0.1f);

        if (_charge <= 1)
        {
            _charge = 20;
        }
    }

    private void Update()
    {
        // Didn't want to work in Start;
        if (_GoalPosition == Vector2.zero)
        {
            _setPosition();
        }
        
    
        Vector3 _currentPos = Vector3.zero;
        if (_yAxis)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _currentPos.x = Mathf.Lerp(transform.position.x, _GoalPosition.x, 0.05f);
            _currentPos.y = _GoalPosition.y;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
            _currentPos.x = _GoalPosition.x;
            _currentPos.y = Mathf.Lerp(transform.position.y, _GoalPosition.y, 0.05f);;
        }

        transform.position = _currentPos;
    }
}