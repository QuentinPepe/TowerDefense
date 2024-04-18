using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private Arena _arena;
    private WaveManager _waveManager;
    private TowerManager _towerManager;
    private UIManager _uiManager;
    private CreatureManager _creatureManager;
    private int _score;
    private int _multiplier;
    private int _currency;

    public Action<int> OnScoreUpdated;
    public Action<int> OnMultiplierUpdated;
    public Action<int> OnCurrencyUpdated;
    public static Game Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        //TODO
    }

    public void UpdateGame()
    {
        //TODO
    }

    public void HandleCreatureEliminated(Creature creature)
    {
        //TODO
    }

    public void HandleCreatureReachedEnd(Creature creature)
    {
        //TODO
    }
}