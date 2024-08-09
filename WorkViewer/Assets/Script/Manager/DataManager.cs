using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : TSingletonMono<DataManager>
{
    public static AddressableSystem AddressableSystem;
    public static ModelTable ModelTable;
    public static LocalizingTable LocalizingTable;
    protected override void OnInitialize()
    {
        AddressableSystem = new AddressableSystem();
        ModelTable = LoadTable<ModelTable>(eTableName.ModelTable);
        ModelTable.Reload();
        LocalizingTable = LoadTable<LocalizingTable>(eTableName.LocalizingTable);
        LocalizingTable.Reload();
        IsLoad = true;      
    }

    public T LoadTable<T>(eTableName name, bool isReload = false) where T : TableBase, new()
    {
        T t = new T();
        t.SetTableName = name.ToString();
        return t;
    }
}
[System.Flags]
public enum eTableName
{
    ModelTable = 1 << 0,
    LocalizingTable = 1 << 1,
    All = ~0,
}
