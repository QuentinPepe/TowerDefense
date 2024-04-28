using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Waves
{
    public class WaveManager : MonoBehaviour
    {
        public Action<WaveSO> OnWaveStarted;
        public Action OnWaveEnded;

        private List<WaveSO> _waves;
        private int _currentWaveIndex = 0;
        private IPhase _currentPhase;

        public WaveSO CurrentWave { get; private set; }

        [SerializeField] private GameObject placementPhaseUI;
        [SerializeField] private GameObject defensePhaseUI;

        public GameObject PlacementPhaseUI => placementPhaseUI;
        public GameObject DefensePhaseUI => defensePhaseUI;


        private void Start()
        {
            LoadWavesFromAddressables();
            SwitchToPlacementPhase();
        }

        private async void LoadWavesFromAddressables()
        {
            _waves = new List<WaveSO>();
            AsyncOperationHandle<IList<WaveSO>> loadOperation = Addressables.LoadAssetAsync<IList<WaveSO>>("WaveData");
            await loadOperation.Task;

            if (loadOperation.Status == AsyncOperationStatus.Succeeded)
            {
                _waves.AddRange(loadOperation.Result);
                _waves.Sort((a, b) => a.WaveNumber.CompareTo(b.WaveNumber));
            }
            else
            {
                Debug.LogError("Failed to load wave data from Addressables.");
            }
        }

        public void AdvanceToNextWave()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex >= _waves.Count)
            {
                // Toutes les vagues ont été terminées : TODO
                return;
            }

            SwitchToPlacementPhase();
        }

        private void SwitchToPlacementPhase()
        {
            _currentPhase = new PlacementPhase();
            _currentPhase.OnEnter(this);
        }

        public void SwitchToDefensePhase()
        {
            CurrentWave = _waves[_currentWaveIndex];
            _currentPhase = new DefensePhase();
            _currentPhase.OnEnter(this);
            OnWaveStarted?.Invoke(CurrentWave);
        }

        private void Update()
        {
            _currentPhase.Update();
        }
        public void FinishPhase()
        {
            _currentPhase.OnFinished();
        }
    }
}