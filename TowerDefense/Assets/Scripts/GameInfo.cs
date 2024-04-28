using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameInfo
{
    public int Score;
    public int CurrentWave;
    public int Currency;
    public int RemainingCreatures;

    public GameInfo(int score, int currentWave, int currency, int remainingCreatures)
    {
        Score = score;
        CurrentWave = currentWave;
        Currency = currency;
        RemainingCreatures = remainingCreatures;
    }
}
