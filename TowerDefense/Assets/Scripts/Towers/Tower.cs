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
        private LineRenderer _radiusLineRenderer;

        [SerializeField] private Transform weapon;
        private const float LineRenderYOffset = 0.1f;

        private void Start()
        {
            _radiusLineRenderer = gameObject.AddComponent<LineRenderer>();
            _radiusLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _radiusLineRenderer.startWidth = 0.1f;
            _radiusLineRenderer.endWidth = 0.1f;
            _radiusLineRenderer.startColor = Color.red;
            _radiusLineRenderer.endColor = Color.red;
        }

        private void Update()
        {
            if (Data.IsUnityNull()) return;
            Shoot();
        }

        public void DrawRadius(float range)
        {
            int numPoints = 50;
            Vector3[] points = new Vector3[numPoints + 1];
            float angle = 0f;
            float angleStep = 2f * Mathf.PI / numPoints;

            for (int i = 0; i < numPoints; i++)
            {
                points[i] = transform.position + new Vector3(Mathf.Cos(angle), LineRenderYOffset, Mathf.Sin(angle)) * range;
                angle += angleStep;
            }
            points[numPoints] = points[0];

            _radiusLineRenderer.positionCount = numPoints + 1;
            _radiusLineRenderer.SetPositions(points);
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
        public void HideRadius()
        {
            _radiusLineRenderer.positionCount = 0;
        }
    }
}