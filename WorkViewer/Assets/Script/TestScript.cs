using UnityEngine;
using UnityEngine.EventSystems;

public class TestScript : MonoBehaviour
{
    [SerializeField] private Transform model;

    private Vector2 touchStartPosition;
    private Quaternion startRotation;
    float rotationAmount = 0.1f;

    void Update()
    {
        if (model == null)return;

        if (Input.GetMouseButtonDown(0))
        {
            StartRotation();
        }
        else if (Input.GetMouseButton(0))
        {
            RotateModel();
        }
    }

    private void StartRotation()
    {
        if (IsPointerOverUI()) return;

        touchStartPosition = Input.mousePosition;
        startRotation = model.rotation;
    }

    private void RotateModel()
    {
        Vector2 touchDelta = (Vector2)Input.mousePosition - touchStartPosition;

        float rotationX = touchDelta.y * rotationAmount;
        float rotationY = -touchDelta.x * rotationAmount;

        model.rotation = startRotation;
        model.Rotate(Vector3.right, rotationX, Space.World);  
        model.Rotate(Vector3.up, rotationY, Space.World);    
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}