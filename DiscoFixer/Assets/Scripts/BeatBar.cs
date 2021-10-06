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

    private void Start()
    {
        GameEvents.beat.onBeat += CreateBeatThings;
        
    }

    private void Update()
    {
        MoveBeatThings();
    }

    private void CreateBeatThings()
    {
        var spawnPosL = new Vector2(background.transform.position.x - background.transform.localScale.x/2, background.transform.localPosition.y);
        var spawnPosR = new Vector2(background.transform.position.x + background.transform.localScale.x/2, background.transform.localPosition.y);
        var beatThingL = Instantiate(beatPrefab, spawnPosL, transform.rotation);
        var beatThingR = Instantiate(beatPrefab, spawnPosR, transform.rotation);
        beatThingsL.Add(beatThingL);
        beatThingsR.Add(beatThingR);
    }


    private void MoveBeatThings()
    {
        for (int i = 0; i < beatThingsL.Count; i++)
        {
            if (beatThingsL[i].transform.position.x >= 0)
            {
                var beat = beatThingsL[i];
                beatThingsL.RemoveAt(i);
                Destroy(beat);
            }
            else
            {
                var newPos = new Vector2(beatThingsL[i].transform.position.x + 0.02f, beatThingsL[i].transform.position.y);
                beatThingsL[i].transform.position = newPos;
            }
        }
        for (int i = 0; i < beatThingsR.Count; i++)
        {
            if (beatThingsR[i].transform.position.x <= 0)
            {
                var beat = beatThingsR[i];
                beatThingsR.RemoveAt(i);
                Destroy(beat);
            }
            else
            {
                var newPos = new Vector2(beatThingsR[i].transform.position.x - 0.02f, beatThingsR[i].transform.position.y);
                beatThingsR[i].transform.position = newPos;
            }
        }
        
        
        // foreach (var beat in beatThingsL)
        // {
        //     if (beat.transform.position.x >= 0)
        //     {
        //         
        //         Destroy(beat);
        //     }
        //     else
        //     {
        //         var newPos = new Vector2(beat.transform.position.x + 0.02f, beat.transform.position.y);
        //         beat.transform.position = newPos;
        //     }
        // }
        // foreach (var beat in beatThingsR)
        // {
        //     if (beat.transform.position.x <= 0)
        //     {
        //         Destroy(beat);
        //     }
        //     else
        //     {
        //         var newPos = new Vector2(beat.transform.position.x - 0.02f, beat.transform.position.y);
        //         beat.transform.position = newPos;
        //     } 
        // }
    }
}



