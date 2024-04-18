using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WaveSO", menuName = "WaveSO", order = 0)]
public class WaveSO : ScriptableObject
{
    [SerializeField] private List<CreatureSO> creatures;
    [SerializeField] private float spawnInterval;

    public IEnumerable<CreatureSO> Creatures => creatures;
    public float SpawnInterval => spawnInterval;
}