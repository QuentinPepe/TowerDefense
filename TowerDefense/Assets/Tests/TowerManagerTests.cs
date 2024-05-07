using System.Collections;
using Grid;
using NUnit.Framework;
using Towers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests
{
    public class TowerManagerTests
    {
        private TowerManager _towerManager;
        private Game _game;
        private TowerSO _basicTowerData;
        private TowerSO _upgradedTowerData;
        private GameObject _basicTowerPrefab;
        private GameObject _upgradedTowerPrefab;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            yield return SceneManager.LoadSceneAsync("Main");

            _towerManager = Object.FindObjectOfType<TowerManager>();
            _game = Game.Instance;

            GameObject basicTowerGameObject = new GameObject();
            GameObject upgradedTowerGameObject = new GameObject();

            basicTowerGameObject.AddComponent<Tower>();
            basicTowerGameObject.AddComponent<AudioSource>();
            upgradedTowerGameObject.AddComponent<Tower>();
            upgradedTowerGameObject.AddComponent<AudioSource>();

            _basicTowerPrefab = basicTowerGameObject;
            _upgradedTowerPrefab = upgradedTowerGameObject;

            _basicTowerData = ScriptableObject.CreateInstance<TowerSO>();
            _basicTowerData.cost = 100;
            _basicTowerData.prefab = _basicTowerPrefab;

            _upgradedTowerData = ScriptableObject.CreateInstance<TowerSO>();
            _upgradedTowerData.cost = 200;
            _upgradedTowerData.prefab = _upgradedTowerPrefab;

            _basicTowerData.upgrade = _upgradedTowerData;
        }

        [UnityTest]
        public IEnumerator PlaceTower_SufficientCurrency_TowerPlacedAndCurrencyUpdated()
        {
            CellPosition position = new CellPosition(5, 5);
            int initialCurrency = _game.Currency;

            bool success = _towerManager.PlaceTower(_basicTowerData, position);

            Assert.IsTrue(success, "Tower placement should succeed with sufficient currency.");
            Assert.AreEqual(initialCurrency - _basicTowerData.cost, _game.Currency,
                "Currency should be deducted after placing a tower.");
            Tower placedTower = _towerManager.GetTower(position);
            Assert.IsNotNull(placedTower, "A tower should exist at the specified position.");
            Assert.AreEqual(_basicTowerData, placedTower.Data, "Placed tower should have the correct data.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator PlaceTower_InsufficientCurrency_TowerNotPlaced()
        {
            _game.Currency = _basicTowerData.cost - 1;
            CellPosition position = new CellPosition(2, 2);

            bool success = _towerManager.PlaceTower(_basicTowerData, position);

            Assert.IsFalse(success, "Tower placement should fail with insufficient currency.");
            Assert.IsNull(_towerManager.GetTower(position), "No tower should be placed at the specified position.");
            Assert.AreEqual(_basicTowerData.cost - 1, _game.Currency, "Currency should remain unchanged.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator UpgradeTower_SufficientCurrency_TowerUpgraded()
        {
            _game.Currency = _upgradedTowerData.cost + _basicTowerData.cost;
            CellPosition position = new CellPosition(3, 3);

            _towerManager.PlaceTower(_basicTowerData, position);
            Tower placedTower = _towerManager.GetTower(position);

            Tower upgradedTower = _towerManager.UpgradeTower(placedTower);

            Assert.IsNotNull(upgradedTower, "Tower should be upgraded.");
            Assert.AreEqual(_upgradedTowerData, upgradedTower.Data, "Upgraded tower should have the correct data.");
            Assert.AreEqual(position, upgradedTower.CellPosition, "Upgraded tower should retain the same position.");
            Assert.AreEqual(0, _game.Currency, "Currency should be deducted correctly after upgrade.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator UpgradeTower_InsufficientCurrency_TowerNotUpgraded()
        {
            LogAssert.Expect(LogType.Error, "Cannot upgrade tower: Invalid tower or insufficient funds.");

            _game.Currency = _upgradedTowerData.cost - 1 + _basicTowerData.cost;
            CellPosition position = new CellPosition(4, 4);

            _towerManager.PlaceTower(_basicTowerData, position);
            Tower placedTower = _towerManager.GetTower(position);

            Tower upgradedTower = _towerManager.UpgradeTower(placedTower);

            Assert.IsNull(upgradedTower, "Tower should not be upgraded.");
            Assert.AreEqual(placedTower, _towerManager.GetTower(position), "Original tower should remain unchanged.");
            Assert.AreEqual(_upgradedTowerData.cost - 1, _game.Currency, "Currency should remain unchanged.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator DestroyTower_TowerExists_TowerDestroyed()
        {
            CellPosition position = new CellPosition(6, 6);
            _towerManager.PlaceTower(_basicTowerData, position);
            Tower placedTower = _towerManager.GetTower(position);

            _towerManager.DestroyTower(placedTower);

            Assert.IsNull(_towerManager.GetTower(position), "Tower should be removed from the specified position.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator IsCellOccupied_TowerPlaced_ReturnsTrue()
        {
            CellPosition position = new CellPosition(7, 7);
            _towerManager.PlaceTower(_basicTowerData, position);

            bool isOccupied = _towerManager.IsCellOccupied(position);

            Assert.IsTrue(isOccupied, "Cell should be marked as occupied.");

            yield return null;
        }

        [UnityTest]
        public IEnumerator IsCellOccupied_NoTowerPlaced_ReturnsFalse()
        {
            CellPosition position = new CellPosition(8, 8);

            bool isOccupied = _towerManager.IsCellOccupied(position);

            Assert.IsFalse(isOccupied, "Cell should be marked as not occupied.");

            yield return null;
        }
    }
}