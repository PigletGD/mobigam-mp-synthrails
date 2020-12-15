using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    [SerializeField] Text portraitText = null;
    [SerializeField] Text landscapeText = null;

    public void AddScore(int value)
    {
        score += value;

        portraitText.text = "SCORE: " + score.ToString();
        landscapeText.text = "SCORE: " + score.ToString();
    }
}
