using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CreatureManager : MonoBehaviour
{
    private Dictionary<CreatureSO, ObjectPool<Creature>> _poolDictionary;
    private ObjectPool<Creature> _creaturePool;
    public Action<Creature> OnCreatureSpawned;

    public Creature SpawnCreature(CreatureSO creatureData, Vector3 spawnPosition, Vector3 target)
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
}