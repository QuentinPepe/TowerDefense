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
        public Dictionary<CreatureSO, ObjectPool<ICreature>> PoolDictionary { get; private set; }
        private ObjectPool<ICreature> _creaturePool;
        public Action<ICreature> OnCreatureSpawned;
        public int CurrentCreatureNumber { get; private set; }

        private void Start()
        {
            CreatePoolDictionary();
        }

        private void SpawnCreature(CreatureSO creatureData, Vector3 spawnPosition, Vector3 target)
        {
            _creaturePool = PoolDictionary[creatureData];
            ICreature creature = _creaturePool.Get();
            creature.SetActive(true);
            OnCreatureSpawned?.Invoke(creature);
            creature.TeleportTo(spawnPosition);
            creature.MoveTo(target);
        }

        public void RemoveCreature(ICreature creature)
        {
            CurrentCreatureNumber--;
            Game.Instance.OnCreatureRemoved?.Invoke(CurrentCreatureNumber);

            if (PoolDictionary.TryGetValue(creature.Data, out _creaturePool))
            {
                creature.SetActive(false);
                _creaturePool.Release(creature);
            }
            else
            {
                Debug.LogWarning($"No pool found for {creature.Data.name}. Skipping release.");
            }
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
            PoolDictionary = new Dictionary<CreatureSO, ObjectPool<ICreature>>();
            AsyncOperationHandle<IList<CreatureSO>> loadOperation =
                Addressables.LoadAssetsAsync<CreatureSO>("CreatureData", null);
            await loadOperation.Task;
            foreach (CreatureSO creatureData in loadOperation.Result)
            {
                ObjectPool<ICreature> creaturePool = new ObjectPool<ICreature>(() => {
                        GameObject creatureObject = Instantiate(creatureData.prefab);
                        ICreature creature = creatureObject.GetComponent<ICreature>();
                        creature.Initialize(creatureData);
                        return creature;
                    }, (creature) => creature.SetActive(true),
                    (creature) => creature.SetActive(false));
                PoolDictionary.Add(creatureData, creaturePool);
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