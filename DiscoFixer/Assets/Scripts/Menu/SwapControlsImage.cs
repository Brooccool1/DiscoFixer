using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapControlsImage : MonoBehaviour
{
    public int swapTimer;
    public Image image;
    public Sprite imageSprite1;
    public Sprite imageSprite2;
    public Sprite[] sprites;
    private float timeUntilSwap;
    int currentSprite = 0;

    private void Update()
    {
        timeUntilSwap -= Time.deltaTime;
        if (timeUntilSwap <= 0)
        {
            timeUntilSwap = swapTimer;
            Swap();
        }

    }

    private void Swap()
    {
        if (currentSprite == sprites.Length - 1)
            currentSprite = 0;
        else
            currentSprite++;

        if(currentSprite == 0)
        {
            image.sprite = sprites[0];
        }

        else if (currentSprite == 1)
        {
            image.sprite = sprites[1];
        }
    }
}
