using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBar : MonoBehaviour
{
    [SerializeField] private GameObject beatPrefab;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject discoBall;
    private List<GameObject> beatThingsL = new List<GameObject>();
    private List<GameObject> beatThingsR = new List<GameObject>();
    private Transform ballTF;
    public float minBallSizeModifier = 1;
    public float maxBallSizeModifier = 1;
    public float ballShrinkSpeed = 1;
    public float beatThingSpeed = 1;
    
    private void Start()
    {
        ballTF = discoBall.transform;
        GameEvents.beat.onBeat += SpawnBeatThings;
        GameEvents.beat.onBeat += PulseDiscoBall;

    }

    private void FixedUpdate()
    {
        MoveBeatThings();
        if (ballTF.localScale.x > 1.2 * minBallSizeModifier)
        {
            ballTF.localScale = Vector3.Lerp(ballTF.localScale, ballTF.localScale * -0.1f, ballShrinkSpeed * Time.deltaTime);
            
        }

        foreach (var thing in beatThingsL)
        {
            thing.transform.Rotate(0,0,10);
        }
        foreach (var thing in beatThingsR)
        {
            thing.transform.Rotate(0,0,-10);
        }
    }

    private void PulseDiscoBall()
    {
        ballTF.localScale = new Vector3(2, 2, 0) * maxBallSizeModifier;
    }

    private void SpawnBeatThings()
    {
        var centerOffset = new Vector3(1.2f, 0, 0);
        var beatSpawnPosL = background.transform.position - centerOffset;
        var beatSpawnPosR = background.transform.position + centerOffset;
        var beatThingL = Instantiate(beatPrefab, beatSpawnPosL, transform.rotation);
        beatThingL.transform.Rotate(0,0,180);
        var beatThingR = Instantiate(beatPrefab, beatSpawnPosR, transform.rotation);
        beatThingsL.Add(beatThingL);
        beatThingsR.Add(beatThingR);
    }


    private void MoveBeatThings()
    {
        var destroyPos = background.transform.position.x + background.transform.localScale.x/2;
        
        for (int i = 0; i < beatThingsL.Count; i++)
        {
            if (beatThingsL[i].transform.position.x <= -destroyPos)
            {
                var beat = beatThingsL[i];
                beatThingsL.RemoveAt(i);
                Destroy(beat);
            }
            else
            {
                var newPos = new Vector2((beatThingsL[i].transform.position.x - 0.04f) * beatThingSpeed, beatThingsL[i].transform.position.y);
                beatThingsL[i].transform.position = newPos;
            }
        }
        for (int i = 0; i < beatThingsR.Count; i++)
        {
            if (beatThingsR[i].transform.position.x >= destroyPos)
            {
                var beat = beatThingsR[i];
                beatThingsR.RemoveAt(i);
                Destroy(beat);
            }
            else
            {
                var newPos = new Vector2((beatThingsR[i].transform.position.x + 0.04f) * beatThingSpeed, beatThingsR[i].transform.position.y);
                beatThingsR[i].transform.position = newPos;
            }
        }
        

    }
}



