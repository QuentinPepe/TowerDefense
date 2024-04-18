using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CreatureManager : MonoBehaviour
{
    private ObjectPool<Creature> _creaturePool;
    public Action<Creature> OnCreatureSpawned;

    private Creature SpawnCreature(CreatureSO creatureData, Vector3 position)
    {
        Creature creature = _creaturePool.Get();
        //TODO
        return creature;
    }
}