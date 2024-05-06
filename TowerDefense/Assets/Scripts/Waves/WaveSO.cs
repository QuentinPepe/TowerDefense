using System.Collections.Generic;
using CreatureS;
using UnityEngine;

namespace Waves
{
    [CreateAssetMenu(fileName = "WaveSO", menuName = "WaveSO", order = 0)]
    public class WaveSO : ScriptableObject
    {
        [SerializeField] private List<CreatureSO> creatures;
        [SerializeField] private float spawnInterval;
        [SerializeField] private int waveNumber;

        public IEnumerable<CreatureSO> Creatures => creatures;
        public float SpawnInterval => spawnInterval;
        public int WaveNumber => waveNumber;
    }
}