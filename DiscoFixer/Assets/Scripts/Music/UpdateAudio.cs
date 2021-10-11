using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (audioSource == null)
            GetComponent<AudioSource>();
        if(PlayerPrefs.HasKey("AudioVolume"))
            audioSource.volume = PlayerPrefs.GetFloat("AudioVolume");
    }

}
