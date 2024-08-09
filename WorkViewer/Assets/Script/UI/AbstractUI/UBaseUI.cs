using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UBaseUI : MonoBehaviour
{
    public UBaseUI Initialize()
    {
        InitReference();
        return this;
    }
    protected abstract void InitReference();
    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }
    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }
    public virtual void Refresh()
    {
        OnRefresh();
    }
    protected abstract void OnRefresh();

}
