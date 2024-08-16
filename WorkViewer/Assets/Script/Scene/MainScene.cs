using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MainScene : BaseScene
{
    public override void StartScene()
    {
        InitManager().Forget();
    }

    async UniTask InitManager()
    {       
        ModelManager.Instance.Initialize();
        await UniTask.WaitUntil(() => ModelManager.Instance.IsLoad);

        UIManager.Instance.Initialize();
        await UniTask.WaitUntil(() => UIManager.Instance.IsLoad);
        Debug.Log("Main Scene Manager Init Complete");
    }
}
