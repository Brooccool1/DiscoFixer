using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    private static int _score = 0;

    public static int score
    {
        get { return _score; }
        set { _score = value; }
    }
}
