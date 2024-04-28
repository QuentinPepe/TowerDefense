using System.Linq;
using Waves;

public class CombatPhase : IPhase
{
    private WaveManager _waveManager;
    private int _remainingCreatures;

    public void OnEnter(WaveManager waveManager)
    {
        _waveManager = waveManager;
        _waveManager.DefensePhaseUI.SetActive(true);

        _remainingCreatures = _waveManager.CurrentWave.Creatures.Count();

        Game.Instance.OnCreatureRemoved += HandleCreatureEliminated;

        // TODO On Defeat

        Game.Instance.StartSpawnCreature(waveManager.CurrentWave);
    }
    public void Update()
    {
    }

    private void HandleCreatureEliminated()
    {
        _remainingCreatures--;
        if (_remainingCreatures <= 0)
        {
            OnFinished();
        }
    }

    public void OnFinished()
    {
        _waveManager.DefensePhaseUI.SetActive(false);
        Game.Instance.OnCreatureRemoved -= HandleCreatureEliminated;
        _waveManager.AdvanceToNextWave();
    }
}