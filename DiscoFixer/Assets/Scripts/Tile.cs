using System;
using UnityEditor;
using UnityEngine;


    public class Tile : MonoBehaviour
    {
        public Color color;
        public int state = 9;
        public bool broken = false;
        
        private void Break()
        {
            broken = true;
            // Unless fixed, for every beat state--
        }
        
    }
