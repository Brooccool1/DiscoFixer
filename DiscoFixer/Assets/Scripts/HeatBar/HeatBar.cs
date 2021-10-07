using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatBar : MonoBehaviour
{
    private LineRenderer _line;
    private Vector3 _startPos = new Vector3();
    private int _oldHeat = 0;
    
    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _startPos = transform.position - (transform.up * transform.localScale.y / 2);
        
        _line.SetPosition(0, _startPos);
    }
    
    void Update()
    {
        if (_oldHeat != Player.heat)
        {
            _line.positionCount++;
            _oldHeat++;
            
            _line.SetPosition(_oldHeat, _startPos + (transform.up * Player.heat / 2));
        }
    }
}
