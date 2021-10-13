using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField] private bool _highscore;
    
    private TextMeshProUGUI _text;
    void Start()
    {
        if(!PlayerPrefs.HasKey("Highscore"))
            PlayerPrefs.SetFloat("Highscore", ScoreKeeper.score);
        
        _text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (PlayerPrefs.GetFloat("Highscore") < ScoreKeeper.score)
        {
            PlayerPrefs.SetFloat("Highscore", ScoreKeeper.score);
        }
        
        if (!_highscore)
        {
            _text.text = "Score: " + ScoreKeeper.score;
        }
        else
        {
            _text.text = "Highscore: " + PlayerPrefs.GetFloat("Highscore");
        }
    }
}
