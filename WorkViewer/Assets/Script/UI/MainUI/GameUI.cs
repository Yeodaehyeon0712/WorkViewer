using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public UMainUI Main;
    public USettingUI Setting;
    public UDescriptionUI Description;

    public void Initialize()
    {
        //Canvas = GetComponent<Canvas>();
        Transform safeArea = transform.Find("USafeArea");
        Main = safeArea.Find("UMainUI").GetComponent<UMainUI>();
        Main.Initialize();
        Setting = safeArea.Find("Group_PopUp/USettingUI").GetComponent<USettingUI>();
        Setting.Initialize();
        Description = safeArea.Find("Group_Movable/UDescriptionUI").GetComponent<UDescriptionUI>();
        Description.Initialize();
    }    
}
