using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;


public class Tile : MonoBehaviour
    {
        //public Color color;
        public int stages = 9;
        public int state = 9;
        public bool isBreaking = false;
        public bool isBroken = false;
        public bool previousIsBreaking = false;
        private VisualEffect vfx;
        

        private void Start()
        {
            GameEvents.beat.onBeat += Break;
           vfx = GetComponentInChildren<VisualEffect>();
        }

        private void Break()
        {
            if (isBreaking)
            {
                state--;
                previousIsBreaking = true;
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

            if (!isBreaking && previousIsBreaking)
            {
                
                vfx.SetVector3("Color", new Vector3(50,1,1));
                vfx.Play();
                previousIsBreaking = false;
            }


    }
    }
