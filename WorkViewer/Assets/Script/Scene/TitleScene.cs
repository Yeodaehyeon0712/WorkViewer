using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using TMPro;

public class TitleScene : BaseScene
{
    Button button_NextScene;
    //나중에 퍼센테이지로 변경할것 .
    TextMeshProUGUI text_Description;
    
    public override void StartScene()
    {
        DontDestroyOnLoad(gameObject);
        button_NextScene = transform.GetComponentInChildren<Button>();
        button_NextScene.onClick.AddListener(() => AsyncSceneChange().Forget());
        text_Description = transform.Find("Panel_Title/Text_Description").GetComponent<TextMeshProUGUI>();
        InitManager().Forget();
    }
    async UniTask InitManager()
    {
        text_Description.text = "Waiting ... ";
        LocalizingManager.Instance.Initialize();
        await UniTask.WaitUntil(() => LocalizingManager.Instance.IsLoad);

        DataManager.Instance.InitAddressableSystem();
        await UniTask.WaitUntil(() => DataManager.AddressableSystem.IsLoad);

        DataManager.Instance.Initialize();
        await UniTask.WaitUntil(() => DataManager.Instance.IsLoad);

        Debug.Log("Title Scene Manager Init Complete");
        text_Description.text = "Press Start";
    }

    private async UniTask AsyncSceneChange()
    {
        try
        {
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
}
