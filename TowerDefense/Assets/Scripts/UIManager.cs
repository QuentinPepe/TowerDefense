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

    private void Start()
    {
        Game.Instance.OnCurrencyUpdated += (currency) => UpdateText(currencyText, currency);
        Game.Instance.OnScoreUpdated += (score) => UpdateText(scoreText, score);
        Game.Instance.OnMultiplierUpdated += (multiplier) => UpdateText(multiplierText, multiplier);
    }

    private void UpdateText(TextMeshProUGUI text, int value)
    {
        text.text = "" + value;
    }
}