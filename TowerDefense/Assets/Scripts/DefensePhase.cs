using System.Linq;
using Waves;

public class DefensePhase : IPhase
{
    private WaveManager _waveManager;

    public void OnEnter(WaveManager waveManager)
    {
        _waveManager = waveManager;
        _waveManager.DefensePhaseUI.SetActive(true);


        Game.Instance.OnCreatureRemoved += HandleCreatureEliminated;

        Game.Instance.StartSpawnCreature(waveManager.CurrentWave);
    }
    public void Update()
    {
    }

    private void HandleCreatureEliminated(int remainingCreatures)
    {
        if (remainingCreatures <= 0)
        {
            _waveManager.AdvanceToNextWave();
        }
    }

    public void OnFinished()
    {
        _waveManager.DefensePhaseUI.SetActive(false);
        Game.Instance.OnCreatureRemoved -= HandleCreatureEliminated;
        _waveManager.OnWaveEnded?.Invoke();
    }
}