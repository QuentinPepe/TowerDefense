using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreatureS;
using Towers;
using UnityEngine;
using Waves;

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
    private IEnumerator _creatureSpawnCoroutine;

    public Action OnCreatureRemoved;
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
        _waveManager.OnWaveStarted += () => {
            WaveSO currentWave = _waveManager.CurrentWave;
            _uiManager.SetMaxCreature(currentWave.Creatures.Count());
            _uiManager.UpdateWaveText(currentWave.WaveNumber);
        };
    }

    public void StartGame()
    {
        //TODO
    }

    private void UpdateGame()
    {
        OnMultiplierUpdated?.Invoke(_multiplier);
        OnScoreUpdated?.Invoke(_score);
        OnCurrencyUpdated?.Invoke(_currency);
        OnCreatureRemoved?.Invoke();
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

    public void StartSpawnCreature(WaveSO currentWaveData)
    {
        _creatureSpawnCoroutine = _creatureManager.SpawnCoroutine(currentWaveData);
        StartCoroutine(_creatureSpawnCoroutine);
    }
}