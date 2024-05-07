using System.Collections;
using CreatureS;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Waves;

namespace Tests
{

    public class GameTests
    {
        private Game _game;
        private ICreature _creatureMock;
        private CreatureSO _creatureData;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Main");

            _game = Object.FindObjectOfType<Game>();
            Object.FindObjectOfType<CreatureManager>();

            _creatureData = ScriptableObject.CreateInstance<CreatureSO>();
            _creatureData.score = 100;
            _creatureData.reward = 50;
            _creatureData.damage = 10;

            _creatureMock = Substitute.For<ICreature>();
            _creatureMock.Data.Returns(_creatureData);
        }

        [UnityTest]
        public IEnumerator HandleCreatureEliminated_IncreasesMultiplierAndUpdatesScoreAndCurrency()
        {
            int initialScore = _game.Score;
            int initialCurrency = _game.Currency;
            int initialMultiplier = _game.Multiplier;

            _game.HandleCreatureEliminated(_creatureMock);

            Assert.AreEqual(initialScore + _creatureData.score * (initialMultiplier + 1), _game.Score);
            Assert.AreEqual(initialCurrency + _creatureData.reward, _game.Currency);
            Assert.AreEqual(initialMultiplier + 1, _game.Multiplier);

            yield return null;
        }

        [UnityTest]
        public IEnumerator HandleCreatureReachedEnd_DecreasesMultiplierAndUpdatesScore()
        {
            _game.Multiplier = 3;
            int initialScore = _game.Score;

            _game.HandleCreatureReachedEnd(_creatureMock);

            Assert.AreEqual(initialScore - _creatureData.score, _game.Score);
            Assert.AreEqual(-1, _game.Multiplier);

            yield return null;
        }

        [UnityTest]
        public IEnumerator CheckGameOver_PlayerHealthZero_GameOverInvoked()
        {
            bool gameOverInvoked = false;
            _game.OnGameOver += (_) => gameOverInvoked = true;

            _game.GetWaveManager().CurrentWave = ScriptableObject.CreateInstance<WaveSO>();
            _game.CheckGameOver(0);

            Assert.IsTrue(gameOverInvoked);

            yield return null;
        }


        [UnityTest]
        public IEnumerator GetWaveManager_WaveManagerNotNull_ReturnsWaveManager()
        {
            Assert.IsNotNull(_game.GetWaveManager());
            yield return null;
        }

    }
}