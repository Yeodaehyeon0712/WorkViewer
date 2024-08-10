using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : TSingletonMono<UIManager>
{
    Transform _parent;
    GameUI _gameUI;
    public UMainUI MainUI => _gameUI.Main; 
    public GameUI GameUI => _gameUI;
    public USettingUI SettingUI => GameUI.Setting;
    public UDescriptionUI DescriptionUI => GameUI.Description;
    protected override void OnInitialize()
    {
        _gameUI = Instantiate(Resources.Load<GameUI>("UI/GameUI"), transform);
        _gameUI.Initialize();        
        _parent = _gameUI.GetComponentInChildren<USafeArea>().transform;
        CanvasInit(_gameUI);
        IsLoad = true;
    }

    void CanvasInit(Component root)
    {
        Canvas canvas = root.GetComponent<Canvas>();
        CanvasScaler scaler = root.GetComponent<CanvasScaler>();        

        if (scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080,1920);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1F;
        }
    }


}
