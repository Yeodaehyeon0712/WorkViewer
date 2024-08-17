using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizingManager : TSingletonMono<LocalizingManager>,ISubject<eLanguage>
{
    private List<IObserver<eLanguage>> observers = new List<IObserver<eLanguage>>();
    eLanguage currentLanguage = eLanguage.English;
    public static bool IsBeingDestroyed = false;
    public eLanguage CurrentLanguage
    {
        get => currentLanguage;
        set 
        {
            if (currentLanguage == value) 
                return;
            
                currentLanguage = value;
                OnLanguageChanged();         
        }
    }

    #region AbstractMethod
    protected override void OnInitialize()
    {
        IsLoad = true;
    }
    private void OnApplicationQuit()
    {
        IsBeingDestroyed = true;
    }

    #endregion

    #region Localize Method
    public string GetLocalizing(int key)
    {
        if (DataManager.LocalizingTable[key] != null)
            return DataManager.LocalizingTable[key];

        return "Unfound table";
    }
    public string GetLocalizing(long key)
    {
        int parseKey = (int)key;
        if (DataManager.LocalizingTable[parseKey] != null)
            return DataManager.LocalizingTable[parseKey];

        return "Unfound table";
    }
    public string GetLocalizing(int key, params object[] parsingParameters)
    {
        return string.Format(GetLocalizing(key), parsingParameters);
    }
    public void SetLanguage(int languageIndex)
    {
        int validIndex = Mathf.Clamp(languageIndex, 0, (int)eLanguage.End - 1);
        eLanguage targetLanguage = (eLanguage)validIndex;
        if (currentLanguage != targetLanguage)
        {
            currentLanguage = targetLanguage;
            OnLanguageChanged();
        }
    }
    
    void OnLanguageChanged()
    {
        Notify(currentLanguage);
    }
    #endregion  

    #region ISubject Method
    public void AddObserver(IObserver<eLanguage> observer)
    {
        if (!observers.Contains(observer))
            observers.Add(observer);
    }
    public void RemoveObserver(IObserver<eLanguage> observer)
    {
        if (observers.Contains(observer))
            observers.Remove(observer);
    }
    public void Notify(eLanguage value)
    {
        if (observers.Count < 1)
            return;

        foreach (var observer in observers)
            observer.OnNotify(value);
        //이건 목록 복사를 고민해보자 .
        //var observersCopy = new List<IObserver<eLanguage>>(observers);
        //foreach (var observer in handlersCopy)
        //{
        //    observer.OnNotify(value);
        //}
    }
    #endregion
}
public enum eLanguage
{
    English,
    Korean,
    End
}