using UnityEngine;

public class LookatCamera : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    Camera camera;

    void Start()
    {
        camera = Camera.main;
        canvas.worldCamera = camera;
    }

    private void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, Vector3.up);
    }
}
