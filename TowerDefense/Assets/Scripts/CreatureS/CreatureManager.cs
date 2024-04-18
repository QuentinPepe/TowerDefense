using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Waves;

public class CreatureManager : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 targetPoint;
    private Dictionary<CreatureSO, ObjectPool<Creature>> _poolDictionary;
    private ObjectPool<Creature> _creaturePool;
    public Action<Creature> OnCreatureSpawned;

    private Creature SpawnCreature(CreatureSO creatureData, Vector3 spawnPosition, Vector3 target)
    {
        _creaturePool = _poolDictionary[creatureData];
        Creature creature = _creaturePool.Get();
        creature.gameObject.SetActive(true);
        OnCreatureSpawned?.Invoke(creature);
        creature.transform.position = spawnPosition;
        creature.MoveTo(target);
        return creature;
    }

    public void RemoveCreature(Creature creature)
    {
        _creaturePool = _poolDictionary[creature.Data];
        creature.gameObject.SetActive(false);
        _creaturePool.Release(creature);
    }

    public IEnumerator SpawnCoroutine(WaveSO currentWaveData)
    {
        foreach (CreatureSO creatureData in currentWaveData.Creatures)
        {
            SpawnCreature(creatureData, spawnPoint, targetPoint);
            yield return new WaitForSeconds(currentWaveData.SpawnInterval);
        }
    }
}