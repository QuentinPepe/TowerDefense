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

    private void Start()
    {
        _score = 0;
        _multiplier = 0;
        _currency = 0;
        UpdateGame();
    }

    public void StartGame()
    {
        //TODO
    }

    public void UpdateGame()
    {
        OnMultiplierUpdated?.Invoke(_multiplier);
        OnScoreUpdated?.Invoke(_score);
        OnCurrencyUpdated?.Invoke(_currency);
    }

    public void HandleCreatureEliminated(Creature creature)
    {
        _creatureManager.RemoveCreature(creature);
        if (_multiplier < 0) _multiplier = 1;
        else _multiplier++;
        _score += creature.Data.score * _multiplier;
        _currency += creature.Data.reward;
        UpdateGame();
    }

    public void HandleCreatureReachedEnd(Creature creature)
    {
        _creatureManager.RemoveCreature(creature);
        if (_multiplier > 0) _multiplier = -1;
        else _multiplier--;
        _score += creature.Data.score * _multiplier;
        UpdateGame();
    }
}