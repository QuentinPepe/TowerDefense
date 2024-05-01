using Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace Towers
{
    public class Tower : MonoBehaviour
    {
        public TowerSO Data { get; private set; }
        public CellPosition CellPosition { get; private set; }

        private float _lastShotTime;

        private void Update()
        {
            if (Data.IsUnityNull()) return;
            Shoot();
        }

        public void Initialize(TowerSO towerData, CellPosition cellPosition)
        {
            Data = towerData;
            CellPosition = cellPosition;
            _lastShotTime = -Data.fireRate;
        }

        private void Shoot()
        {
            if (Time.time - _lastShotTime < Data.fireRate)
                return;

            Collider[] results = new Collider[1];
            int size = Physics.OverlapSphereNonAlloc(transform.position, Data.range, results,
                LayerMask.GetMask("Enemy"));
            if (size == 0) return;
            GameObject enemy = results[0].gameObject;
            RotatesToward(enemy.transform.position);
            GameObject projectileObject = Instantiate(Data.projectilePrefab, transform.position, transform.rotation);
            projectileObject.GetComponent<Projectile>().SetTarget(enemy);
            _lastShotTime = Time.time;
        }

        private void RotatesToward(Vector3 target)
        {
            target.y = transform.position.y;
            gameObject.transform.LookAt(target);
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