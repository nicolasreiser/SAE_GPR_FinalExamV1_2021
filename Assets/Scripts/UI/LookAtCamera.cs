using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        canvas.worldCamera = mainCamera;
    }

    private void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, Vector3.up);
    }
}
