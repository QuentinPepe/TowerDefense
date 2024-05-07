using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreatureS;
using Grid;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class CreatureManagerTests
    {
        private CreatureManager _creatureManager;
        private ICreature _creatureMock;
        private CreatureSO _creatureData;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Main");

            _creatureManager = Object.FindObjectOfType<CreatureManager>();

            yield return new WaitWhile(() => _creatureManager.PoolDictionary == null || _creatureManager.PoolDictionary.Count == 0);

            _creatureData = ScriptableObject.CreateInstance<CreatureSO>();
            _creatureData.score = 100;
            _creatureData.reward = 50;

            ObjectPool<ICreature> creaturePool = new ObjectPool<ICreature>(() => {
                    GameObject creatureObject = Object.Instantiate(_creatureData.prefab);
                    ICreature creature = creatureObject.GetComponent<ICreature>();
                    creature.Initialize(_creatureData);
                    return creature;
                }, (creature) => creature.SetActive(true),
                (creature) => creature.SetActive(false));
            _creatureManager.PoolDictionary.Add(_creatureData, creaturePool);

            _creatureMock = Substitute.For<ICreature>();
            _creatureMock.Data.Returns(_creatureData);
        }

        [UnityTest]
        public IEnumerator RemoveCreature_CreatureExistsInDictionary_RemovesCreatureFromPool()
        {
            int initialCurrentCreatureNumber = _creatureManager.CurrentCreatureNumber;
            int initialPoolCountForCreatureData = _creatureManager.PoolDictionary[_creatureData].CountInactive;

            _creatureManager.RemoveCreature(_creatureMock);

            yield return null;

            Assert.AreEqual(initialCurrentCreatureNumber - 1, _creatureManager.CurrentCreatureNumber, "CurrentCreatureNumber should be decremented after removing a creature.");
            Assert.AreEqual(initialPoolCountForCreatureData + 1, _creatureManager.PoolDictionary[_creatureData].CountInactive, "The creature should be added back to the pool.");
            Assert.IsFalse(_creatureMock.IsActive(), "The creature should be set to inactive after being removed.");
        }

        [UnityTest]
        public IEnumerator RemoveCreature_CreatureNotInDictionary_LogsWarning()
        {
            LogAssert.Expect(LogType.Warning, new System.Text.RegularExpressions.Regex($"No pool found for {_creatureData.name}. Skipping release."));

            _creatureMock.Data.Returns(ScriptableObject.CreateInstance<CreatureSO>());
            _creatureManager.RemoveCreature(_creatureMock);

            yield return null;
        }

        [UnityTest]
        public IEnumerator PoolDictionary_CreatesValidPools()
        {
            Dictionary<CreatureSO, ObjectPool<ICreature>> creaturePools = _creatureManager.PoolDictionary;

            Assert.IsNotNull(creaturePools, "The PoolDictionary should be initialized.");
            Assert.IsTrue(creaturePools.Count > 0, "There should be at least one pool available.");
            Assert.IsInstanceOf<ObjectPool<ICreature>>(creaturePools.Values.First(), "The values in the PoolDictionary should be of type ObjectPool<ICreature>.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator SetStartPosition_SetsCorrectSpawnPoint()
        {
            CellPosition testPosition = new CellPosition(10, 10);
            _creatureManager.SetStartPosition(testPosition);

            Vector3 expectedSpawnPoint = new Vector3(testPosition.X + 0.5f, 0, testPosition.Z + 0.5f);

            FieldInfo spawnPointField = typeof(CreatureManager).GetField("spawnPoint", BindingFlags.NonPublic | BindingFlags.Instance);
            if (spawnPointField != null)
            {
                Vector3 actualSpawnPoint = (Vector3)spawnPointField.GetValue(_creatureManager);

                Assert.AreEqual(expectedSpawnPoint, actualSpawnPoint, "Spawn point should be correctly set to the provided cell position.");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator SetEndPosition_SetsCorrectTargetPoint()
        {
            CellPosition testPosition = new CellPosition(5, 5);
            _creatureManager.SetEndPosition(testPosition);

            Vector3 expectedTargetPoint = new Vector3(testPosition.X + 0.5f, 0, testPosition.Z + 0.5f);

            FieldInfo targetPointField = typeof(CreatureManager).GetField("targetPoint", BindingFlags.NonPublic | BindingFlags.Instance);
            if (targetPointField != null)
            {
                Vector3 actualTargetPoint = (Vector3)targetPointField.GetValue(_creatureManager);

                Assert.AreEqual(expectedTargetPoint, actualTargetPoint, "Target point should be correctly set to the provided cell position.");
            }

            yield return null;
        }
    }
}