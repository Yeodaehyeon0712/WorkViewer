using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UDescriptionUI : UMovableUI
{
    #region Fields
    LocalizingText text_Name;
    LocalizingText text_MadeBy;
    LocalizingText text_Description;
    Button btn_Exit;

    #endregion
    protected override void InitReference()
    {
        base.InitReference();
        var panel = transform.Find("Panel_Description");
        text_Name = panel.Find("Text_Name").GetComponent<LocalizingText>();
        text_MadeBy=panel.Find("Text_MadeBy").GetComponent<LocalizingText>();
        text_Description=panel.Find("Text_Description").GetComponent<LocalizingText>();
        btn_Exit = panel.Find("Btn_Exit").GetComponent<Button>();
        btn_Exit.onClick.AddListener(()=>MoveOut());
    }
    public override void Enable()
    {
        base.Enable();
        SetActiveModelDescription();
    }
    void SetActiveModelDescription()
    {
        var modelData = ModelManager.Instance.GetActiveModel().Data;
        text_Name.SetLocalizingIndex((int)modelData.NameKey);
        text_MadeBy.SetLocalizingIndex((int)modelData.MadeByKey);
        text_Description.SetLocalizingIndex((int)modelData.DescriptionKey);
    }
}
