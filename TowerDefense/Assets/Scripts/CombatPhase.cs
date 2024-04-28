using Waves;

public class CombatPhase : IPhase
{
    private WaveManager _waveManager;

    public void OnEnter(WaveManager waveManager)
    {
        _waveManager = waveManager;
        _waveManager.DefensePhaseUI.SetActive(true);
        Game.Instance.StartSpawnCreature(waveManager.CurrentWave);
    }
    public void Update()
    {

    }
    public void OnFinished()
    {
        _waveManager.DefensePhaseUI.SetActive(false);
        _waveManager.AdvanceToNextWave();
    }
}