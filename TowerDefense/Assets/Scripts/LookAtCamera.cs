using UnityEngine;
public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        gameObject.transform.LookAt(_camera.transform);
    }

}