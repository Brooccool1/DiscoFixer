using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PickupSounds : MonoBehaviour
{
    [SerializeField] private AudioClip _water;
    [SerializeField] private AudioClip _wiper;
    [SerializeField] private AudioClip _freeze;
    [SerializeField] private AudioClip _fixCombo;

    private static AudioSource _player;
    
    // Statics
    private static AudioClip _Swater;
    private static AudioClip _Swiper;
    private static AudioClip _Sfreeze;
    private static AudioClip _SfixCombo;

    private void Start()
    {
        _Swater = _water;
        _Swiper = _wiper;
        _Sfreeze = _freeze;
        _SfixCombo = _fixCombo;
        _player = GetComponent<AudioSource>();
    }

    public static void Water()
    {
        if (!_Swater) return;
        if (_player.isPlaying) return;
        _player.clip = _Swater;
        _player.Play();
    }
    
    public static void Wiper()
    {
        if (!_Swiper) return;
        if (_player.isPlaying) return;
        _player.clip = _Swiper;
        _player.Play();
    }
    
    public static void Freeze()
    {
        if (!_Sfreeze) return;
        if (_player.isPlaying) return;
        _player.clip = _Sfreeze;
        _player.Play();
    }
    
    public static void Combo()
    {
        if (!_SfixCombo) return;
        if (_player.isPlaying) return;
        _player.clip = _SfixCombo;
        _player.Play();
    }
}
