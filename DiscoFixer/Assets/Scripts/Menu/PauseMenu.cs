using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    static bool gameIsPaused = false;
    [SerializeField] GameObject myCanvas;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!gameIsPaused)
            {
                gameIsPaused = true;
                myCanvas.SetActive(true);
            }
            else
            {
                gameIsPaused = false;
                myCanvas.SetActive(false);
            }
            
        }
    }
}
