using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI remainingCreatureText;
    private int _maxCreature;

    private void Start()
    {
        Game.Instance.OnCurrencyUpdated += (currency) => UpdateText(currencyText, currency);
        Game.Instance.OnScoreUpdated += (score) => {
            UpdateText(scoreText, score);
            UpdateText(remainingCreatureText, _maxCreature);
        };

        Game.Instance.OnCreatureRemoved += () => {
            _maxCreature--;
        };

        Game.Instance.OnMultiplierUpdated += (multiplier) => UpdateText(multiplierText, multiplier);
    }

    private void UpdateText(TextMeshProUGUI text, int value, string message = "")
    {
        text.text = message + value;
    }

    public void SetMaxCreature(int maxCreature)
    {
        _maxCreature = maxCreature;
        UpdateText(remainingCreatureText, _maxCreature);
    }

    public void UpdateWaveText(int value)
    {
        UpdateText(waveText, value, "Wave : ");
    }
}