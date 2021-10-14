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
    // Water
    public bool hasWaterPickup = false;
    public int _waterStayTime = 10;
    public int waterPickupEffect = 30;
    // Wiper
    public bool hasWiperPickup = false;
    public int _wiperStayTime = 10;
    // Freeze
    public bool hasFreezePickup = false;
    public int _freezeStayTime = 5;
    [SerializeField] private VisualEffect vfxBurst;
    [SerializeField] private VisualEffect vfxFreeze;
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
        vfxBuildUp.Play();
    }

    private void OnEveryBeat()
    {
        Breaker();
        _waterPickup();
        _wiperPickup();
        _freezePickup();
    }

    private void Breaker()
    {
        if (isBreaking && !StopTilesBreaking.active)
        {
            state--;
            vfxBuildUp.Play();

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
            spawnrate = (1f - (float)state / 9f) * MaxSpawnRate;
            vfxBuildUp.SetFloat("SpawnRate", spawnrate);
        }

        if (state == 8)
        {
            impact.Play();
        }
    }

    private void _waterPickup()
    {
        if (!hasWaterPickup) return;
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
    
    private void _freezePickup()
    {
        if (!hasFreezePickup) return;
        if (isBroken)
        {
            hasFreezePickup = false;
        }
        if (_freezeStayTime <= 0)
        {
            hasFreezePickup = false;
        }
        _freezeStayTime--;
    }
    
   public void _freezeFX()
    {
        vfxFreeze.Play();

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
        if (isBreaking && !isBroken && !StopTilesBreaking.active)
        {
            GetComponent<SpriteRenderer>().material.color = Color.red;
            vfxBuildUp.SetInt("Cold", 0);

        }

        if (isBreaking && !isBroken && StopTilesBreaking.active)
        {
            GetComponent<SpriteRenderer>().material.color = new Color(0.4f, 0.3f, 1.0f, 1.0f);
            vfxBuildUp.SetInt("Cold", 1);

        }

        if (isBroken)
        {
            GetComponent<SpriteRenderer>().material.color = Color.black;
        }

        if (!isBreaking && previousIsBreaking && !isBroken)
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
        gameObject.GetComponentInChildren<Transform>().Find("Freeze").GetComponent<SpriteRenderer>().enabled = hasFreezePickup;

        
    }
}
