using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Pool;
using UnityEngine.ResourceManagement.AsyncOperations;
using Waves;

namespace CreatureS
{
    public class CreatureManager : MonoBehaviour
    {
        [SerializeField] private Vector3 spawnPoint;
        [SerializeField] private Vector3 targetPoint;
        private Dictionary<CreatureSO, ObjectPool<Creature>> _poolDictionary;
        private ObjectPool<Creature> _creaturePool;
        public Action<Creature> OnCreatureSpawned;

        private void Start()
        {
            CreatePoolDictionary();
        }

        private void SpawnCreature(CreatureSO creatureData, Vector3 spawnPosition, Vector3 target)
        {
            _creaturePool = _poolDictionary[creatureData];
            Creature creature = _creaturePool.Get();
            creature.gameObject.SetActive(true);
            OnCreatureSpawned?.Invoke(creature);
            creature.transform.position = spawnPosition;
            creature.MoveTo(target);
        }

        public void RemoveCreature(Creature creature)
        {
            Game.Instance.OnCreatureRemoved?.Invoke();
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

        private void CreatePoolDictionary()
        {
            _poolDictionary = new Dictionary<CreatureSO, ObjectPool<Creature>>();
            AsyncOperationHandle<IList<CreatureSO>> loadOperation =
                Addressables.LoadAssetAsync<IList<CreatureSO>>("CreatureData");
            foreach (CreatureSO creatureData in loadOperation.Result)
            {
                ObjectPool<Creature> creaturePool = new ObjectPool<Creature>(() => {
                        GameObject creatureObject = Instantiate(creatureData.prefab);
                        return creatureObject.GetComponent<Creature>();
                    }, (creature) => creature.gameObject.SetActive(true),
                    (creature) => creature.gameObject.SetActive(false));
                _poolDictionary.Add(creatureData, creaturePool);
            }
        }
    }
}