using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class WaveSO : ScriptableObject
{
    [SerializeField] private List<CreatureSO> creatures;
    [SerializeField] private List<float> spawnIntervals;

    public IEnumerable<CreatureSO> Creatures => creatures;
    public IEnumerable<float> SpawnIntervals => spawnIntervals;
}