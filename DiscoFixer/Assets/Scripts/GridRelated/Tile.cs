using System;
using System.Collections.Generic;
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
    public bool hasWaterPickup = false;
    public bool hasWiperPickup = false;
    public int _waterStayTime = 10;
    public int _wiperStayTime = 20;
    public int waterPickupEffect = 30;
    [SerializeField] private VisualEffect vfxBurst;
    [SerializeField] private VisualEffect impact;
    [SerializeField] private VisualEffect vfxBuildUp;
    [SerializeField] private float MaxSpawnRate;
    [SerializeField] private List<Sprite> _breakStages;

    private float spawnrate = 0f;

    private void Start()
    {
        GameEvents.beat.onBeat += OnEveryBeat;
        //vfx = GetComponentInChildren<VisualEffect>();
        state = stages;
    }

    private void OnEveryBeat()
    {
        Breaker();
        _waterPickup();
        _wiperPickup();
    }

    private void Breaker()
    {
        if (isBreaking)
        {
            state--;

            spawnrate = (1f - (float) state / 9f) * MaxSpawnRate;

            vfxBuildUp.SetFloat("SpawnRate", spawnrate);

            if (!isBroken)
            {
                previousIsBreaking = true;
            }
        }

        if (state == 0)
        {
            isBroken = true;
            isBreaking = false;
        }

        if (state == 8)
        {
            impact.Play();
        }
    }

    private void _waterPickup()
    {
        if (hasWaterPickup)
        {
            if (isBroken)
            {
                hasWaterPickup = false;
            }
            if (_waterStayTime <= 0)
            {
                hasWaterPickup = false;
            }
            _waterStayTime--;
        }
    }
    
    private void _wiperPickup()
    {
        if (!hasWiperPickup) return;
        if (isBroken)
        {
            hasWiperPickup = false;
        }
        if (_wiperStayTime <= 0)
        {
            hasWiperPickup = false;
        }
        _wiperStayTime--;
    }
    
   

    private void _changeSprite()
    {
        if (state > 0)
        {
            int sprite = state / 3;
            GetComponent<SpriteRenderer>().sprite = _breakStages[sprite];
        }
    }

    private void Update()
    {
        _changeSprite();
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
            
            vfxBurst.SetVector3("Color", new Vector3(2,159,2));
            vfxBurst.Play();
            previousIsBreaking = false;
            vfxBuildUp.Stop();
            spawnrate = 0;
        }
        
        if (isBroken && previousIsBreaking)
        {
            vfxBurst.SetVector3("Color", new Vector3(160, 20, 2));
            vfxBurst.Play();
            previousIsBreaking = false;
            vfxBuildUp.Stop();

            spawnrate = 0;
        }
        gameObject.GetComponentInChildren<Transform>().Find("Water").GetComponent<SpriteRenderer>().enabled = hasWaterPickup;
        gameObject.GetComponentInChildren<Transform>().Find("Wiper").GetComponent<SpriteRenderer>().enabled = hasWiperPickup;

        
    }
}
