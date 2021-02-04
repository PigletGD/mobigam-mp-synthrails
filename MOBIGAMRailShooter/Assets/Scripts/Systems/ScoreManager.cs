using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;

    public Text portraitText = null;
    public Text landscapeText = null;

    public int moneyCollected = 0;

    public void AddScore(int value)
    {
        score += value;

        portraitText.text = "SCORE: " + score.ToString();
        landscapeText.text = "SCORE: " + score.ToString();
    }

    public void AddMoney() => moneyCollected++;
}