using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TitleScene : BaseScene
{
    Button _titleButton;
    public override void StartScene()
    {
        DontDestroyOnLoad(gameObject);
        _titleButton = transform.GetComponentInChildren<Button>();
        _titleButton.onClick.AddListener(() => AsyncSceneChange().Forget());
        //매니저 Init
        //이후 타이틀 버튼 보여주기
        //이동후 2차 매니저 Init
    }
    private async UniTask AsyncSceneChange()
    {
        try
        {
            await InitManager();
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainScene").ToUniTask();
            Debug.Log("Scene changed successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load scene: {e.Message}");
        }
        finally
        {
            GameObject.Find("MainScene").GetComponent<MainScene>().StartScene();
            DestroyImmediate(gameObject);
        }
    }
    //1차 매니저 로드 이후 타이틀 씬 보여준다 .
    // 버튼을 통해 씬 이동
    //2차 매니저 생성 필요 ..
    async UniTask InitManager()
    {
        LocalizingManager.Instance.Initialize();
        await UniTask.WaitUntil(() => LocalizingManager.Instance.IsLoad); 

        DataManager.Instance.Initialize();
        await UniTask.WaitUntil(() => DataManager.Instance.IsLoad);

        //DataManager.AddressableSystem.Initialize();
        //await UniTask.WaitUntil(() => DataManager.AddressableSystem.IsLoad);

        ModelManager.Instance.Initialize();
        await UniTask.WaitUntil(() => ModelManager.Instance.IsLoad);

        UIManager.Instance.Initialize();
        await UniTask.WaitUntil(() => UIManager.Instance.IsLoad);
        Debug.Log("Manager Init Complete");
    }
}
