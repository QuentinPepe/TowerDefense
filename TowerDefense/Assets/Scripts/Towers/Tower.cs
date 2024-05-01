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
        private float _placementTime;
        [SerializeField] private Transform weapon;

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
            _placementTime = Time.time;
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
            GameObject projectileObject = Instantiate(Data.projectilePrefab, weapon.position, transform.rotation);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.SetTarget(enemy);
            projectile.SetDamage(Data.damage);
            _lastShotTime = Time.time;
        }

        private void RotatesToward(Vector3 target)
        {
            target.y = weapon.position.y;
            weapon.transform.LookAt(target);
        }


        public bool CanUpgrade()
        {
            return Data.upgrade != null;
        }

        public float GetTimeSincePlacement()
        {
            return Time.time - _placementTime;
        }
    }
}