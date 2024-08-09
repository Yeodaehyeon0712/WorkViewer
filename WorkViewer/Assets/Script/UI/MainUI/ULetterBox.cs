using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ULetterBox : MonoBehaviour
{
    enum LetterBoxDirection
    {
        Top,
        Bottom,  
    }

    RectTransform _panel;
    [SerializeField]  LetterBoxDirection _direction = LetterBoxDirection.Top;

    void Awake()
    {
        _panel = (RectTransform)transform;
        USafeArea.onChangeSafeArea.AddListener(UpdateLetterBox);
    }

    private void UpdateLetterBox(Vector2 min, Vector2 max)
    {
        if (_direction == LetterBoxDirection.Top)
            _panel.anchorMin = new Vector2(0, max.y);
        else
            _panel.anchorMax = new Vector2(1, min.y);

        _panel.sizeDelta = Vector2.zero;
        _panel.anchoredPosition = Vector2.zero;
    }
}
