using System;
using System.Collections;
using System.Linq;
using CreatureS;
using UnityEngine;
using Waves;

public class Game : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private CreatureManager creatureManager;

    private int _score;
    private int _multiplier;
    private int _currency;
    private IEnumerator _creatureSpawnCoroutine;

    public Action<int> OnCreatureRemoved;
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
        waveManager.OnWaveStarted += () => {
            WaveSO currentWave = waveManager.CurrentWave;
            uiManager.SetMaxCreature(currentWave.Creatures.Count());
            uiManager.UpdateWaveText(currentWave.WaveNumber);
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
    }

    public void HandleCreatureEliminated(Creature creature)
    {
        creatureManager.RemoveCreature(creature);
        if (_multiplier < 0) _multiplier = 1;
        else _multiplier++;
        _score += creature.Data.score * _multiplier;
        _currency += creature.Data.reward;
        UpdateGame();
    }

    public void HandleCreatureReachedEnd(Creature creature)
    {
        creatureManager.RemoveCreature(creature);
        if (_multiplier > 0) _multiplier = -1;
        else _multiplier--;
        _score += creature.Data.score * _multiplier;
        UpdateGame();
    }

    public void StartSpawnCreature(WaveSO currentWaveData)
    {
        _creatureSpawnCoroutine = creatureManager.SpawnCoroutine(currentWaveData);
        StartCoroutine(_creatureSpawnCoroutine);
    }
}