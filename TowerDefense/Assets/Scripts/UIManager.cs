using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Waves;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText { get; private set; }
    [SerializeField] private TextMeshProUGUI multiplierText;
    public TextMeshProUGUI CurrencyText { get; private set; }
    public TextMeshProUGUI WaveText { get; private set; }
    public TextMeshProUGUI RemainingCreatureText { get; private set; }

    private void Start()
    {
        Game.Instance.OnCurrencyUpdated += (currency) => UpdateText(CurrencyText, currency);
        Game.Instance.OnScoreUpdated += (score) => { UpdateText(ScoreText, score); };

        Game.Instance.OnCreatureRemoved += (currentCreatureNumber) =>
        {
            UpdateText(RemainingCreatureText, currentCreatureNumber);
        };

        Game.Instance.OnMultiplierUpdated += (multiplier) => UpdateText(multiplierText, multiplier);
        Game.Instance.GetWaveManager().OnWaveStarted += HandleNewWave;
    }

    private void UpdateText(TextMeshProUGUI text, int value, string message = "")
    {
        text.text = message + value;
    }


    public void UpdateWaveText(int value)
    {
        UpdateText(WaveText, value, "Wave : ");
    }

    private void HandleNewWave(WaveSO currentWave)
    {
        UpdateText(RemainingCreatureText, currentWave.Creatures.Count());
        UpdateWaveText(currentWave.WaveNumber);
    }
}