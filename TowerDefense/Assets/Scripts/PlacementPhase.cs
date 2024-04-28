using UnityEngine;
using Waves;
public class PlacementPhase : IPhase
{
    private WaveManager _waveManager;

    public void OnEnter(WaveManager waveManager)
    {
        _waveManager = waveManager;
        _waveManager.PlacementPhaseUI.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // En attendant un bouton UI
        {
            OnFinished();
        }
    }

    public void OnFinished()
    {
        _waveManager.PlacementPhaseUI.SetActive(false);
        _waveManager.SwitchToDefensePhase();
    }
}