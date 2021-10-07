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

    private Vector2 beatSpawnPos = new Vector2(0, 0);
    public float scaleChange = 0.1f;
    public Vector3 scaleVector = new Vector3(0.1f, 0.1f, 0);

    private void Start()
    {
        GameEvents.beat.onBeat += CreateBeatThings;
        GameEvents.beat.onBeat += PulseDiscoBall;

    }

    private void Update()
    {
        MoveBeatThings();
        PulseDiscoBall();
    }

    private void PulseDiscoBall()
    {
        var ballScale = discoBall.transform.localScale;
        ballScale += scaleVector;
        // ballScale.x = Mathf.Lerp(ballScale.x, ballScale.x -= scaleChange.x,0.05f);
        // ballScale.y = Mathf.Lerp(ballScale.y, ballScale.y -= scaleChange.y,0.05f);
    }

    private void CreateBeatThings()
    {
        
        // var spawnPosL = new Vector2(background.transform.position.x - background.transform.localScale.x/2, background.transform.localPosition.y);
        // var spawnPosR = new Vector2(background.transform.position.x + background.transform.localScale.x/2, background.transform.localPosition.y);
        var beatThingL = Instantiate(beatPrefab, beatSpawnPos, transform.rotation);
        var beatThingR = Instantiate(beatPrefab, beatSpawnPos, transform.rotation);
        beatThingsL.Add(beatThingL);
        beatThingsR.Add(beatThingR);
    }


    private void MoveBeatThings()
    {
        var destroyXPosL = background.transform.position.x - background.transform.localScale.x/2;
        var destroyXPosR = background.transform.position.x + background.transform.localScale.x/2;

        for (int i = 0; i < beatThingsL.Count; i++)
        {
            if (beatThingsL[i].transform.position.x <= destroyXPosL)
            {
                var beat = beatThingsL[i];
                beatThingsL.RemoveAt(i);
                Destroy(beat);
            }
            else
            {
                var newPos = new Vector2(beatThingsL[i].transform.position.x - 0.03f, beatThingsL[i].transform.position.y);
                beatThingsL[i].transform.position = newPos;
            }
        }
        for (int i = 0; i < beatThingsR.Count; i++)
        {
            if (beatThingsR[i].transform.position.x >= destroyXPosR)
            {
                var beat = beatThingsR[i];
                beatThingsR.RemoveAt(i);
                Destroy(beat);
            }
            else
            {
                var newPos = new Vector2(beatThingsR[i].transform.position.x + 0.03f, beatThingsR[i].transform.position.y);
                beatThingsR[i].transform.position = newPos;
            }
        }
        

    }
}



