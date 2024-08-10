using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UDescriptionUI : UMovableUI
{
    #region Variables
    TextMeshProUGUI text_Name;
    TextMeshProUGUI text_MadeBy;
    TextMeshProUGUI text_Description;
    Button btn_Exit;
    //VariableLocalizedString local_Name;


    #endregion
    protected override void InitReference()
    {
        base.InitReference();
        //local_Name = transform.Find("Panel_Description/Text_Name").GetComponent<VariableLocalizedString>();
        //local_Name.InitReference();
        SetData();
    }
    void SetData()
    {
        //local_Name.UpdateLocalizedString("UI_1");
    }
}
