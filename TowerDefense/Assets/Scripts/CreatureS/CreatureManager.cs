using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Grid;
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
        public int CurrentCreatureNumber { get; private set; }

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
            creature.TeleportTo(spawnPosition);
            creature.MoveTo(target);
        }

        public void RemoveCreature(Creature creature)
        {
            CurrentCreatureNumber--;
            Game.Instance.OnCreatureRemoved?.Invoke(CurrentCreatureNumber);
            _creaturePool = _poolDictionary[creature.Data];
            creature.gameObject.SetActive(false);
            _creaturePool.Release(creature);
        }

        public IEnumerator SpawnCoroutine(WaveSO currentWaveData)
        {
            CurrentCreatureNumber = currentWaveData.Creatures.Count();
            foreach (CreatureSO creatureData in currentWaveData.Creatures)
            {
                SpawnCreature(creatureData, spawnPoint, targetPoint);
                yield return new WaitForSeconds(currentWaveData.SpawnInterval);
            }
        }

        private async void CreatePoolDictionary()
        {
            _poolDictionary = new Dictionary<CreatureSO, ObjectPool<Creature>>();
            AsyncOperationHandle<IList<IList<CreatureSO>>> loadOperation =
                Addressables.LoadAssetsAsync<IList<CreatureSO>>("CreatureData", null);
            await loadOperation.Task;
            foreach (CreatureSO creatureData in loadOperation.Result)
            {
                ObjectPool<Creature> creaturePool = new ObjectPool<Creature>(() => {
                        GameObject creatureObject = Instantiate(creatureData.prefab);
                        Creature creature = creatureObject.GetComponent<Creature>();
                        creature.Initialize(creatureData);
                        return creature;
                    }, (creature) => creature.gameObject.SetActive(true),
                    (creature) => creature.gameObject.SetActive(false));
                _poolDictionary.Add(creatureData, creaturePool);
            }
        }
        public void SetStartPosition(CellPosition cellPosition)
        {
            spawnPoint = new Vector3(cellPosition.X + 0.5f, 0, cellPosition.Z + 0.5f);
        }

        public void SetEndPosition(CellPosition cellPosition)
        {
            targetPoint = new Vector3(cellPosition.X + 0.5f, 0, cellPosition.Z + 0.5f);
        }
    }
}