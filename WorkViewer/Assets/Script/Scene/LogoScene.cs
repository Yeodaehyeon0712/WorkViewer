using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.UI;

public class LogoScene : BaseScene
{
    [SerializeField] Image _logo;
    [SerializeField] AnimationCurve _fadeCurve;

    private void Awake()
    {
        StartScene();
    }
    public override void StartScene()
    {
        DontDestroyOnLoad(gameObject);
        _logo.DOFade(1f, 2f).SetEase(_fadeCurve).OnComplete(() => AsyncSceneChange().Forget()); 
    }

    private async UniTask AsyncSceneChange()
    {
        try
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("TitleScene").ToUniTask();
            Debug.Log("Scene changed successfully");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load scene: {e.Message}");
        }
        finally
        {
            GameObject.Find("TitleScene").GetComponent<TitleScene>().StartScene();
            DestroyImmediate(gameObject);
        }
    }
}
