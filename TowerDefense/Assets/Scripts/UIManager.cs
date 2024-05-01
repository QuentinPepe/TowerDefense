using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Waves;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    public TextMeshProUGUI CurrencyText;
    public TextMeshProUGUI WaveText;
    public TextMeshProUGUI RemainingCreatureText;

    private void Start()
    {
        Game.Instance.OnCurrencyUpdated += (currency) => UpdateText(CurrencyText, currency, "Currency : ");
        Game.Instance.OnScoreUpdated += (score) => { UpdateText(ScoreText, score, "Score : "); };

        Game.Instance.OnCreatureRemoved += (currentCreatureNumber) =>
        {
            UpdateText(RemainingCreatureText, currentCreatureNumber, "Remaining : ");
        };

        Game.Instance.OnMultiplierUpdated += (multiplier) => UpdateText(multiplierText, multiplier, "Multiplier : ");
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