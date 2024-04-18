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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}