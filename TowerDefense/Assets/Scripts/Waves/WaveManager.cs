using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Waves
{
    public class WaveManager : MonoBehaviour
    {
        public event Action OnWaveStarted;
        public event Action OnWaveEnded;

        private List<WaveSO> _waves;
        private int _currentWaveIndex = 0;
        private WaveSO _currentWave;
        private IPhase _currentPhase;

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

        public void StartWave()
        {
            _currentWave = _waves[_currentWaveIndex];
            _currentPhase = new CombatPhase(_currentWave);
            OnWaveStarted?.Invoke();
        }

        public void AdvanceToNextWave()
        {
            _currentWaveIndex++;
            if (_currentWaveIndex >= _waves.Count)
            {
                // Toutes les vagues ont été terminées
                return;
            }

            SwitchToPlacementPhase();
        }

        public void SwitchToPlacementPhase()
        {
            _currentPhase = new PlacementPhase();
        }

        public void SwitchToDefensePhase()
        {
            StartWave();
        }

        private void Update()
        {
            _currentPhase.Update(this);
        }
    }
}