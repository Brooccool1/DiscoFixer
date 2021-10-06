using System;
using UnityEditor;
using UnityEngine;


    public class Tile : MonoBehaviour
    {
        public Color color;
        public int state = 9;
        public bool isBreaking = false;
        public bool isBroken = false;
        
        private void Break()
        {
            isBreaking = true;
            // Unless fixed, for every beat state--
            
            //If state == 0 -> broken = true
        }
        
    }
