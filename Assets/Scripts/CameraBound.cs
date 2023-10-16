using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraBound : MonoBehaviour
{
    public float MaxX { private set; get; }
    public float MaxY { private set; get; }
    public float MinX { private set; get; }
    public float MinY { private set; get; }

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        MaxX = _camera.orthographicSize * _camera.aspect;
        MinX = -_camera.orthographicSize * _camera.aspect;
        MaxY = _camera.orthographicSize;
        MinY = -_camera.orthographicSize;
    }
}
