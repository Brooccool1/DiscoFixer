using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{


    [Header("Different Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;

    [Header("Volume")]
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI currentVolume;


    private void Start()
    {
        if(!PlayerPrefs.HasKey("AudioVolume"))
            PlayerPrefs.SetFloat("AudioVolume", audioPlayer.volume);

        OpenMainMenu();
        volumeSlider.value = audioPlayer.volume;
        audioPlayer.Play();
    }

    public void OpenSettings()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void OpenMainMenu()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void VolumeChange(float volume)
    {
        currentVolume.text = (volume * 100).ToString("0.0");
        audioPlayer.volume = volume;
        PlayerPrefs.SetFloat("AudioVolume", volume);
    }

}
