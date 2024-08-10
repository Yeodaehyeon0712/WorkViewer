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
        //�Ŵ��� Init
        //���� Ÿ��Ʋ ��ư �����ֱ�
        //�̵��� 2�� �Ŵ��� Init
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
    //1�� �Ŵ��� �ε� ���� Ÿ��Ʋ �� �����ش� .
    // ��ư�� ���� �� �̵�
    //2�� �Ŵ��� ���� �ʿ� ..
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
