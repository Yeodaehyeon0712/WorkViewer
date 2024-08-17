using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class TableBase
{
    protected string _tableName;
    public string SetTableName { set => _tableName = value; }
    protected Dictionary<int, Dictionary<string, string>> _dataDic;
    public void Reload()
    {
        OnLoad();
    }
    protected abstract void OnLoad();
    protected void LoadData(string name)
    {      
        TextAsset textAsset = AddressableSystem.GetTable($"Table/{name}.csv");

#if UNITY_ANDROID
        string[] arr = textAsset.ToString().Split('\r');
#elif UNITY_IOS
        string[] arr = textAsset.ToString().Split('\n');
#endif
        _dataDic = new Dictionary<int, Dictionary<string, string>>(arr.Length - 1);
        string[] keys = arr[0].Split(',');
        Regex regex = new Regex("\"(.*?)\"");
        for (int i = 1; i < arr.Length; ++i)
        {
            var matches = regex.Matches(arr[i]);
            if (matches.Count > 0)
            {
                foreach (var match in matches)
                {
                    string matchString = match.ToString();
                    string str = matchString.Replace("\"", "").Replace(",", "P!0");
                    arr[i] = arr[i].Replace(matchString, str);
                }
            }

            string[] values = arr[i].Split(',');
            int idx = int.Parse(values[0]);
            _dataDic.Add(idx, new Dictionary<string, string>());
            for (int j = 0; j < keys.Length; ++j)
                _dataDic[idx].Add(keys[j], values[j]);
        }
    }
}
