using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatTracker : MonoBehaviour
{
    [SerializeField] private AudioClip _song;
    [SerializeField, Range(1, 10)] private int _skippedBeats = 1;

    [Header("Main menu or not")] 
    [SerializeField] private bool _mainMenu = false;
    
    // Started song
    private bool _started = false;
    
    // Timer for lenght of song
    private float _timer = 0;
    
    // boots and pants, bpm = 144
    private int _bpm = 130;
    private float _beat;
    
    // There's a tiny gap at the start of a mp3 file
    [Header("Change to sync beat with song")]
    [SerializeField]private float _songOffset = 0f;
    
    private float _songPosition;

    // DSP at the start of the song
    private float _dspTimeSong;
    
    private float _lastBeat = 0;

    private AudioSource _audioPlayer;
    
    private void _startSong()
    {
        _beat = (60 * 100 / _bpm * 100) * 0.0001f;
        _dspTimeSong = (float)AudioSettings.dspTime;
        
        _audioPlayer = GetComponent<AudioSource>();
        _audioPlayer.Play();

    }

    private void Update()
    {
        if (_audioPlayer)
        {
            if (Time.timeScale < 1)
            {
                _audioPlayer.Pause();
            }
            else
            {
                if (!_audioPlayer.isPlaying)
                {
                    _audioPlayer.Play();
                }
            }
        }
    }

    private void _nextScene()
    {
        ScoreKeeper._dead = false;
        SceneManager.LoadScene("DeathScreen");
    }

    void FixedUpdate()
    {
        if (!_mainMenu)
        {
            if (_timer >= _song.length)
            {
                Invoke("_nextScene", 0.5f);
            }
        }

        if (!_started)
        {
            if (Player.pressedArrows())
            {
                _startSong();
                _started = true;
            }
        }
        else
        {
            _songPosition = (float) (AudioSettings.dspTime - _dspTimeSong) * _audioPlayer.pitch + _songOffset;

            // What happens every beat
            if (_songPosition > _lastBeat + _beat)
            {
                GameEvents.beat.OnBeat();
                _lastBeat += _beat * _skippedBeats;
            }

            _timer += Time.deltaTime;
        }
    }
}
