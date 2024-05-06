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
        private readonly Collider[] _results = new Collider[10];

        [SerializeField] private Transform weapon;
        private const float LineRenderYOffset = 0.1f;

        private AudioSource _audioSource;

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
            _audioSource = GetComponent<AudioSource>();
            _audioSource.clip = Data.shootSound;
        }

        private void Shoot()
        {
            if (Time.time - _lastShotTime < Data.fireRate)
                return;

            int size = Physics.OverlapSphereNonAlloc(transform.position, Data.range, _results,
                LayerMask.GetMask("Enemy"));

            GameObject closestEnemy = null;
            float closestDistance = float.MaxValue;
            for (int i = 0; i < size; i++)
            {
                Collider enemy = _results[i];
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (!(distance < closestDistance)) continue;
                closestEnemy = enemy.gameObject;
                closestDistance = distance;
            }
            if (!closestEnemy) return;
            if (closestDistance > Data.range) return;
            RotatesToward(closestEnemy.transform.position);
            GameObject projectileObject = Instantiate(Data.projectilePrefab, weapon.position, transform.rotation);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.SetTarget(closestEnemy);
            projectile.SetDamage(Data.damage);
            _lastShotTime = Time.time;
            _audioSource.Play();
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