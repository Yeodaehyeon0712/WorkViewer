using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[RequireComponent(typeof(CanvasGroup))]
public abstract class UMovableUI : UBaseUI
{
    #region Variables
    [SerializeField]eUIDir dir;
    RectTransform rectTransform;
    Vector2 canvasSize;
    CanvasGroup canvasGroup;
    GameObject raycastBlock;
    #endregion
    protected override void InitReference()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasSize = UIManager.Instance.GameUI.GetComponent<CanvasScaler>().referenceResolution;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
        raycastBlock = transform.parent.Find("RaycastBlock").gameObject;
        SetFirstPosition();
    }
    public override void Disable()
    {
        base.Disable();
        raycastBlock.SetActive(false);
    }
    public override void Enable()
    {
        base.Enable();
        raycastBlock.SetActive(true);
    }
    protected override void OnRefresh()
    {
        
    }

    #region Movable Func
    void SetFirstPosition()
    {
        rectTransform.localPosition = dir switch
        {
            eUIDir.LeftToRight => new Vector2(-canvasSize.x, 0),
            eUIDir.RightToLeft => new Vector2(canvasSize.x, 0),
            eUIDir.TopToBottom => new Vector2(0, canvasSize.y),
            eUIDir.BottomToTop => new Vector2(0, -canvasSize.y),
            _ => Vector2.zero
        };
    }
    public void MoveIn(float duration=1f)
    {
        Enable();
        switch (dir)
        {
            case eUIDir.LeftToRight:
            case eUIDir.RightToLeft:
                rectTransform.DOLocalMoveX(0, duration).OnComplete(() => canvasGroup.interactable = true);
                break;
            case eUIDir.TopToBottom:
            case eUIDir.BottomToTop:
                rectTransform.DOLocalMoveY(0, duration).OnComplete(() => canvasGroup.interactable = true);
                break;
            default:return;
        }
    }
    public void MoveOut(float duration=1f)
    {
        canvasGroup.interactable = false;
        switch (dir)
        {
            case eUIDir.LeftToRight:
                rectTransform.DOLocalMoveX(-canvasSize.x, duration).onComplete=Disable;
                break;
            case eUIDir.RightToLeft:
                rectTransform.DOLocalMoveX(canvasSize.x, duration).onComplete=Disable;
                break;
            case eUIDir.TopToBottom:
                rectTransform.DOLocalMoveY(canvasSize.y, duration).onComplete = Disable;
                break;
            case eUIDir.BottomToTop:
                rectTransform.DOLocalMoveY(-canvasSize.y, duration).onComplete = Disable;
                break;
            default: return;
        }
    }
    #endregion
}
enum eUIDir
{
    None,
    LeftToRight,
    RightToLeft,
    TopToBottom,
    BottomToTop
}
