using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]

public class LocalizedText : MonoBehaviour,IObserver<eLanguage>
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
        SetLocalizedText();
    }
    void OnDisable()
    {
        LocalizingManager.Instance.RemoveObserver(this);
    }
    #endregion

    #region Localizing Method
    void SetLocalizedText()
    {
        text.text = LocalizingManager.Instance.GetLocalizing(localIndex);
    }
    #endregion

    #region Observer Method
    public void OnNotify(eLanguage value)
    {
        SetLocalizedText();
    }
    #endregion
}
