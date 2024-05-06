using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 60f;
    [SerializeField] private Collider confinementArea;
    private CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        _virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * (moveSpeed * Time.deltaTime);

        Vector3 newPosition = transform.position + movement;
        newPosition = ClampPosition(newPosition);
        transform.position = newPosition;

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput == 0) return;
        float newFOV = _virtualCamera.m_Lens.FieldOfView - scrollInput * zoomSpeed;
        _virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(newFOV, minZoom, maxZoom);
    }

    private Vector3 ClampPosition(Vector3 position)
    {
        if (confinementArea == null) return position;
        Bounds bounds = confinementArea.bounds;

        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);
        position.z = Mathf.Clamp(position.z, bounds.min.z, bounds.max.z);
        return position;
    }
}