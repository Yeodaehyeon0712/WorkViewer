using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UPopUpUI :UBaseUI
{
    GameObject popUpBG;
    protected override void InitReference()
    {
        popUpBG = transform.parent.Find("PopUpBG").gameObject;
    }
    protected override void OnRefresh()
    {

    }
    public override void Enable()
    {
        base.Enable();
        popUpBG.SetActive(true);
    }
    public override void Disable()
    {
        base.Disable();
        popUpBG.SetActive(false);
    }
}
