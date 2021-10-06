using System;
using UnityEditor;
using UnityEngine;


    public class Tile : MonoBehaviour
    {
        //public Color color;
        public int stages = 9;
        public int state = 9;
        public bool isBreaking = false;
        public bool isBroken = false;

        private void Start()
        {
            GameEvents.beat.onBeat += Break;
        }

        private void Break()
        {
            if (isBreaking)
            {
                state--;
            }

            if (state == 0)
            {
                isBroken = true;
            }
        }

        private void Update()
        {
            if (isBreaking)
            {
                GetComponent<SpriteRenderer>().material.color = Color.red;
            }

            if (isBroken)
            {
                GetComponent<SpriteRenderer>().material.color = Color.black;
            }
        }
    }
