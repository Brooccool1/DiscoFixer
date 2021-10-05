using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTracker : MonoBehaviour
{
    [SerializeField] private AudioClip _song;
    [SerializeField, Range(1, 10)] private int _skippedBeats = 1;
    
    
    // boots and pants, bpm = 144
    private int _bpm = 144;
    private float _beat;
    private float _songOffset = 0.17f;
    private float _songPosition;

    private float _dspTimeSong;
    
    private float _lastBeat = 0;

    private AudioSource _audioPlayer;
    
    private void Start()
    {
        _beat = (60 * 100 / _bpm * 100) * 0.0001f;
        _dspTimeSong = (float)AudioSettings.dspTime;
        
        _audioPlayer = GetComponent<AudioSource>();
        _audioPlayer.Play();
    }

    void Update()
    {
        _songPosition = (float) (AudioSettings.dspTime - _dspTimeSong) * _audioPlayer.pitch - _songOffset;

        // What happens every beat
        if (_songPosition > _lastBeat + _beat)
        {
            print("beat");
            _lastBeat += _beat * _skippedBeats;
        }
    }
}