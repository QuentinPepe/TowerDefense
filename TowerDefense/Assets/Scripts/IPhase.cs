using Waves;
public interface IPhase
{
    void OnEnter(WaveManager waveManager);
    void Update();
    void OnFinished();
}