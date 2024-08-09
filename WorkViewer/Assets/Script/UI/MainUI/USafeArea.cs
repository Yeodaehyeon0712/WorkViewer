using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class USafeArea : MonoBehaviour
{
    private RectTransform safeAreaTransform;
    public static UnityEvent<Vector2, Vector2> onChangeSafeArea = new UnityEvent<Vector2, Vector2>();

    void Start()
    {
        safeAreaTransform = transform as RectTransform;
        ApplySafeArea();
    }

    void ApplySafeArea()
    {
        if (safeAreaTransform == null)
            return;

        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;

        onChangeSafeArea?.Invoke(anchorMin, anchorMax);

        //lastOrientation = Screen.orientation;
    }
}
