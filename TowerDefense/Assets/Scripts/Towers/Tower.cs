using Grid;
using UnityEngine;

namespace Towers
{
    public class Tower : MonoBehaviour
    {
        public TowerSO Data { get; private set; }
        public CellPosition CellPosition { get; private set; }

        private float _lastShotTime;

        private void Start()
        {
            _lastShotTime = -Data.fireRate;
        }

        private void Update()
        {
            Shoot();
        }
        public void Initialize(TowerSO towerData, CellPosition cellPosition)
        {
            Data = towerData;
            CellPosition = cellPosition;
        }

        private void Shoot()
        {
            if (Time.time - _lastShotTime < Data.fireRate)
                return;

            Collider[] results = new Collider[10];
            Physics.OverlapSphereNonAlloc(transform.position, Data.range, results);
            foreach (Collider hitCollider in results)
            {
                if (!hitCollider.CompareTag("Enemy")) continue;
                // TODO : Instantiate a projectile and shoot it towards the enemy
                _lastShotTime = Time.time;
                break;
            }
        }

        public void Upgrade()
        {
            Data = Data.upgrade;
            // TODO : Optionally update the tower's appearance or capabilities
        }

        public bool CanUpgrade()
        {
            return Data.upgrade != null;
        }
    }
}