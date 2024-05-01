using Grid;
using UnityEngine;
using Waves;
public class PlacementPhase : IPhase
{
    private WaveManager _waveManager;
    private GridController _gridController;

    public void OnEnter(WaveManager waveManager)
    {
        _waveManager = waveManager;
        _gridController = waveManager.GridController;
        _waveManager.PlacementPhaseUI.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _waveManager.SwitchToDefensePhase();
        }

        _gridController.UpdateEvent();
    }

    public void OnFinished()
    {
        _waveManager.PlacementPhaseUI.SetActive(false);
    }
}