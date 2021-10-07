using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    private LineRenderer _line;
    
    void Start()
    {
        _line = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        // start position
        _line.SetPosition(0, transform.position);
        
        // end position
        _line.SetPosition(1, (-transform.up * 20) + transform.position); 
        
        if (_line.startColor.a > 0) 
        {
             _line.startColor -= new Color(0, 0, 0, 0.05f); 
        }
    }
}
