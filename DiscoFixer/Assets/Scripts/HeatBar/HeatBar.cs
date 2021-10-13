using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatBar : MonoBehaviour
{
    // Curving bar
    public bool curve = true;
    [Range(0, 90)] public float curveDegrees = 0;
    private float _currentCurve = 90;
    
    private LineRenderer _line;
    private Vector3 _startPos = new Vector3();
    private int _oldHeat = 0;
    public List<Sprite> flames;
    public GameObject flame;
    public List<Sprite> multipliers;
    public GameObject multiplier;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _startPos = transform.position;
        
        _line.SetPosition(0, _startPos);

        _currentCurve -= curveDegrees;
    }
    
    void Update()
    {
        if (_oldHeat < Player.heat)
        {
            if (curve)
            {
                _currentCurve += curveDegrees;
            }
            
            _line.positionCount++;
            _oldHeat++;
            
            // Get the radius from degrees an apply them to a new vector
            float radius = _currentCurve * Mathf.Deg2Rad;
            Vector3 newPos = new Vector3();
            newPos += new Vector3(Mathf.Cos(radius), Mathf.Sin(radius));
            
            
            _line.SetPosition(_oldHeat, _line.GetPosition(_oldHeat - 1) + newPos * 0.02f);
        }
        else if (_oldHeat > Player.heat)
        {
            _currentCurve -= curveDegrees;
            _line.positionCount--;
            _oldHeat--;
        }

        flame.GetComponent<SpriteRenderer>().sprite = Player.heat switch
        {
            < 33 => flames[0],
            > 67 => flames[2],
            _ => flames[1]
        };

        multiplier.GetComponent<SpriteRenderer>().sprite = Player.heat switch
        {
            < 33 => multipliers[2],
            > 67 => multipliers[0],
            _ => multipliers[1]
        };
    }
}
