using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Scoreboard", menuName = "QuantumGGJ/Scoreboard", order = 0)]
public class Scoreboard : ScriptableObject 
{
    public int MaxScore;
    public int CurrentScore;
    public int findMaxScore(int score)
    {
        this.MaxScore = Math.Max(this.MaxScore, score);
        return this.MaxScore;
    }

    public string getCurrentScore()
    {
        return this.CurrentScore.ToString();
    }

    public string getHighScore()
    {
        return "High Score: " + this.MaxScore.ToString();
    }
}

