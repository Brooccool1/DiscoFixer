using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostEffect : MonoBehaviour
{

    public Volume volume;
    ChromaticAberration chromatic;
    Bloom bloom;
    LensDistortion distortion;
    private bool allowUpdate = false;

    //private float effectTime = 0.1f;
    //private float elaspsedTime;
    //private float percentageComplete;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.beat.onBeat += PostFX;
        volume = GetComponent<Volume>();

        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
        volume.profile.TryGet<LensDistortion>(out distortion);
    }

    // Update is called once per frame
    private void PostFX()
    {
        allowUpdate = true;
        Invoke("EndTime", 1);
    }

    private void EndTime()
    {
        allowUpdate = false;
    }
    void Update()
    {
       if (allowUpdate)
        {
           bloom.intensity.value = Mathf.Lerp(1, 8, Mathf.PingPong(Time.deltaTime, 1));
           

        }
        
    }
}
