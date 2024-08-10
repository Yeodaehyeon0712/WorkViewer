using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class USettingUI : UPopUpUI,IObserver<eLanguage>
{
    #region Variable
    Button btn_Exit;

    Button btn_Language_Left;
    TextMeshProUGUI text_Language;
    Button btn_Language_Right;

    Slider slider_Sound;

    Slider slider_Light;
    #endregion

    #region Init Method
    protected override void InitReference()
    {
        base.InitReference();
        btn_Exit = transform.Find("Btn_Exit").GetComponent<Button>();
        btn_Exit.onClick.AddListener(OnExitBtnClick);

        var languageLayout = transform.Find("Panel_Language/Panel_Btn");
        btn_Language_Left = languageLayout.Find("Btn_Left").GetComponent<Button>();
        btn_Language_Left.onClick.AddListener(() => OnLanguageBtnClick(isLeft: true));
        btn_Language_Right = languageLayout.Find("Btn_Right").GetComponent<Button>();
        btn_Language_Right.onClick.AddListener(() => OnLanguageBtnClick(isLeft: false));
        text_Language = languageLayout.Find("Text_Language").GetComponent<TextMeshProUGUI>();

        slider_Sound = transform.Find("Panel_Sound/Slider_Sound").GetComponent<Slider>();
        slider_Light = transform.Find("Panel_Light/Slider_Light").GetComponent<Slider>();

        LocalizingManager.Instance.AddObserver(this);
        SetLanguageUI(LocalizingManager.Instance.CurrentLanguage);
    }
    #endregion

    #region Exit Method
    void OnExitBtnClick()
    {
        base.Disable();
    }
    #endregion

    #region Language Method
    public void OnLanguageBtnClick(bool isLeft)
    {
        int currentLanguage = (int)LocalizingManager.Instance.CurrentLanguage;
        int nextLanguage = isLeft ? currentLanguage - 1 : currentLanguage + 1;
        LocalizingManager.Instance.SetLanguage(nextLanguage);
        LocalizingManager.Instance.CurrentLanguage = (eLanguage)nextLanguage;
    }
    public void OnNotify(eLanguage value)
    {
        SetLanguageUI(value);
    }
    void SetLanguageUI(eLanguage value)
    {
        int currentLanguage = (int)value;
        bool canMoveLeft = (currentLanguage != 0);
        btn_Language_Left.gameObject.SetActive(canMoveLeft);

        bool canMoveRight = (currentLanguage != ((int)eLanguage.End) - 1);
        btn_Language_Right.gameObject.SetActive(canMoveRight);

        text_Language.text = value.ToString();
    }
    #endregion

    #region Sound Method
    #endregion

    #region Light Method
    #endregion
}
