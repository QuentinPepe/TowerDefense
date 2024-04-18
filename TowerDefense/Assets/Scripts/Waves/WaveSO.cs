using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WaveSO", menuName = "WaveSO", order = 0)]
public class WaveSO : ScriptableObject
{
    [SerializeField] private List<CreatureSO> creatures;
    [SerializeField] private List<float> spawnIntervals;

    public IEnumerable<CreatureSO> Creatures => creatures;
    public IEnumerable<float> SpawnIntervals => spawnIntervals;
}