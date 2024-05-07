using System.Collections;
using System.Linq;
using CreatureS;
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
    }
}