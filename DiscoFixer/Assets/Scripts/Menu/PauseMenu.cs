using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    private float _timeScale = 1;
    
    [Header("Volume")]
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private Text currentVolume;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private bool _notParent = false;

    private void Start()
    {
        if (!_notParent)
        {
            _canvas.SetActive(false);
        }
        if(currentVolume != null)
        {
            currentVolume.text = "Volume: " + (PlayerPrefs.GetFloat("AudioVolume") * 100).ToString();
            volumeSlider.value = audioPlayer.volume;
        }

    }

    void Update()
    {
        if (!_notParent)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _timeScale = _timeScale > 0 ? 0 : 1;

                Time.timeScale = _timeScale;

                _canvas.SetActive(Time.timeScale < 1);
            }
        }
    }

    public void ContinueButton()
    {
        Time.timeScale = 1;
        _canvas.SetActive(false);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void VolumeChange(float volume)
    {
        currentVolume.text = "Volume: " + (volume * 100).ToString("0.0");
        audioPlayer.volume = volume;
        PlayerPrefs.SetFloat("AudioVolume", volume);
    }
}
