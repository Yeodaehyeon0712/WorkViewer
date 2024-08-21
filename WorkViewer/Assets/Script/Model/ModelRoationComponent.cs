using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelRoationComponent
{
    Transform model;
    private Vector2 touchStartPosition;
    private Quaternion startRotation;
    float rotationAmount = 0.1f;

    public void SetModel(Transform model)
    {
        this.model = model;
    }
    public void ResetRotation()
    {
        if (model == null) return;

        model.rotation = Quaternion.identity;
        model = null;
    }
    public void UpdateModelRotation()
    {
        if (model == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            StartRotation();
        }
        else if (Input.GetMouseButton(0))
        {
            RotateModel();
        }
    }
    void StartRotation()
    {
        if (IsPointerOverUI()) return;

        touchStartPosition = Input.mousePosition;
        startRotation = model.rotation;
    }

    void RotateModel()
    {
        Vector2 touchDelta = (Vector2)Input.mousePosition - touchStartPosition;

        float rotationX = touchDelta.y * rotationAmount;
        float rotationY = -touchDelta.x * rotationAmount;

        model.rotation = startRotation;
        model.Rotate(Vector3.right, rotationX, Space.World);
        model.Rotate(Vector3.up, rotationY, Space.World);
    }

    bool IsPointerOverUI()
    {
        //이거 작동여부 다시 확인 ..
        return EventSystem.current.IsPointerOverGameObject();
    }
}
