using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]

public class LocalizingText : MonoBehaviour,IObserver<eLanguage>
{
    [SerializeField] int localIndex=0;
    TextMeshProUGUI text;

    #region UnityMethods
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void OnEnable()
    {
        LocalizingManager.Instance.AddObserver(this);
        SetLocalizingText();
    }
    void OnDisable()
    {
        if (LocalizingManager.IsBeingDestroyed) 
            return;
        LocalizingManager.Instance.RemoveObserver(this);
    }
    #endregion

    #region Localizing Method
    void SetLocalizingText()
    {
        text.text = LocalizingManager.Instance.GetLocalizing(localIndex);
    }
    public void SetLocalizingIndex(int index)
    {
        localIndex = index;
        SetLocalizingText();
    }
    #endregion

    #region Observer Method
    public void OnNotify(eLanguage value)
    {
        SetLocalizingText();
    }
    #endregion
}
