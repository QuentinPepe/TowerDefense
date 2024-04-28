using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverCanvas;

    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private TextMeshProUGUI currencyText;

    [SerializeField] private TextMeshProUGUI remainingCreaturesText;

    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.OnGameOver += ActiveGameOverPanel;
    }

    private void ActiveGameOverPanel(GameInfo gameInfo)
    {
        gameOverCanvas.SetActive(true);
        scoreText.text = "Score : " + gameInfo.Score;
        waveText.text = "Wave : " + gameInfo.CurrentWave;
        currencyText.text = "Currency : " + gameInfo.Currency;
        remainingCreaturesText.text = "Remaining creatures : " + gameInfo.RemainingCreatures;
    }
}