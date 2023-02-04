using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UpdateTextGameOver : MonoBehaviour
{
    
    public TextMeshProUGUI highScore;
    public Scoreboard scoreboard;

    public TextMeshProUGUI currentScore;

    void Start()
    {
        currentScore.text = scoreboard.getCurrentScore();
        highScore.text = scoreboard.getHighScore();
    }
}