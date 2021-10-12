using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    private TextMeshProUGUI _text;
    [SerializeField] private Text _playAgain;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        if (_text != null)
        {
            if (ScoreKeeper._dead)
            {
                _text.text = "You Died";
            }
            else
            {
                _text.text = "You Won";
            }
        }

        if (_playAgain != null)
        {
            if (ScoreKeeper._dead)
            {
                _playAgain.text = "Retry";
            }
            else
            {
                _playAgain.text = "Play Again";
            }
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
