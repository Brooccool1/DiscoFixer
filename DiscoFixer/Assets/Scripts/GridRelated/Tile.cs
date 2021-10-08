using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;


public class Tile : MonoBehaviour
{
    public int stages = 9;
    public int state;
    public bool isBreaking = false;
    public bool isBroken = false;
    public bool previousIsBreaking = false;
    private VisualEffect vfx;
    

    private void Start()
    {
        GameEvents.beat.onBeat += Break;
        vfx = GetComponentInChildren<VisualEffect>();
        state = stages;
    }

    private void Break()
    {
        if (isBreaking)
        {
            state--;
            if(!isBroken)
            {
                previousIsBreaking = true;
            }
        }

        if (state == 0)
        {
            
            

            isBroken = true;


        }
    }

    private void Update()
    {
        


        if (isBreaking && !isBroken)
        {
            GetComponent<SpriteRenderer>().material.color = Color.red;
        }

        if (isBroken)
        {
            GetComponent<SpriteRenderer>().material.color = Color.black;
        }

        if (!isBreaking && previousIsBreaking)
        {
            
            vfx.SetVector3("Color", new Vector3(2,159,2));
            vfx.Play();
            previousIsBreaking = false;
        }


         if (isBroken && previousIsBreaking)

         {
                vfx.SetVector3("Color", new Vector3(160, 20, 2));
                vfx.Play();
                previousIsBreaking = false;
         }

    }
}
