using System;
using System.Collections;
using CreatureS;
using UnityEngine;
using Waves;

public class Game : MonoBehaviour
{
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private CreatureManager creatureManager;
    [SerializeField] private PlayerEntity playerEntity;

    private int _currency;
    private int _score;
    private int _multiplier;
    private IEnumerator _creatureSpawnCoroutine;

    public Action<int> OnCreatureRemoved;
    public Action<int> OnScoreUpdated;
    public Action<int> OnMultiplierUpdated;
    public Action<int> OnCurrencyUpdated;
    public Action<GameInfo> OnGameOver;
    public Action OnTowerHit;
    public static Game Instance { get; private set; }

    public int Currency {
        get => _currency;
        set {
            _currency = value;
            OnCurrencyUpdated?.Invoke(_currency);
        }
    }
    private int Multiplier {
        get => _multiplier;
        set {
            _multiplier = value;
            OnMultiplierUpdated?.Invoke(Multiplier);
        }
    }
    private int Score {
        get => _score;
        set {
            _score = value;
            OnScoreUpdated?.Invoke(_score);
        }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Score = 0;
        Multiplier = 0;
        Currency = 500;
        Time.timeScale = 1;
    }

    public void HandleCreatureEliminated(Creature creature)
    {
        creatureManager.RemoveCreature(creature);
        if (Multiplier < 0) Multiplier = 1;
        else Multiplier++;
        Score += creature.Data.score * Multiplier;
        Currency += creature.Data.reward;
    }

    public void HandleCreatureReachedEnd(Creature creature)
    {
        if (Multiplier > 0) Multiplier = -1;
        else Multiplier--;
        Score += creature.Data.score * Multiplier;
        playerEntity.TakeDamage(creature.Data.damage);
        creatureManager.RemoveCreature(creature);
        OnTowerHit?.Invoke();
    }

    public void CheckGameOver(int currentHealth)
    {
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        Time.timeScale = 0;
        OnGameOver?.Invoke(new GameInfo(Score, waveManager.CurrentWave.WaveNumber, Currency,
            creatureManager.CurrentCreatureNumber));
    }

    public void StartSpawnCreature(WaveSO currentWaveData)
    {
        _creatureSpawnCoroutine = creatureManager.SpawnCoroutine(currentWaveData);
        StartCoroutine(_creatureSpawnCoroutine);
    }

    public WaveManager GetWaveManager()
    {
        return waveManager;
    }
}