using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayFX : MonoBehaviour
{
    //[SerializeField]
    //private VisualEffect VisualEffect;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.beat.onBeat += Playereffect;
    }

    private void Playereffect()
    {
        GetComponent<VisualEffect>().Play();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
