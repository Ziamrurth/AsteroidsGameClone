using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager {
    #region Singleton
    private static ScoreManager instance;

    private ScoreManager()
    { }

    public static ScoreManager GetInstance()
    {
        if (instance == null)
            instance = new ScoreManager();
        return instance;
    }
    #endregion

    public delegate void OnScoreChanged(int score);
    public OnScoreChanged onScoreChangedCallback;

    public int Score { get; set; }

    public void Start()
    {
        Score = 0;
        GameManager.GetInstance().onObjectDestroyedCallback += AddScore;
    }

    public void AddScore(SpaceObject spaceObject)
    {
        int newScore = Score;

        if (spaceObject as Asteroid != null)
        {
            if((spaceObject as Asteroid).Size == 1f)
            {
                newScore += 10;
            }
            if((spaceObject as Asteroid).Size == 0.5f)
            {
                newScore += 50;
            }
            if ((spaceObject as Asteroid).Size == 0.25f)
            {
                newScore += 100;
            }
        }
        if(spaceObject as Ufo != null)
        {
            newScore += 200;
        }

        if(newScore != Score)
        {
            Score = newScore;
            onScoreChangedCallback?.Invoke(Score);
        }
    }
}
