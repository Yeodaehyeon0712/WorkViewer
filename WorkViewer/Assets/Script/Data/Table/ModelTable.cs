using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Data
{
    public class ModelData
    {
        public long Index;
        public string NameKey;
        public string MadeByKey;
        public string DescriptionKey;
        public string ResourcePath;

        public ModelData(long index,Dictionary<string, string> dataPair)
        {
            Index = index;
            NameKey = dataPair["NameKey"];
            MadeByKey = dataPair["MakerKey"];
            DescriptionKey = dataPair["DescriptionKey"];
            ResourcePath = dataPair["ResourcePath"];
        }
    }
}
public class ModelTable : TableBase
{
    Dictionary<long , Data.ModelData> _modelDataDic = new Dictionary<long , Data.ModelData>();
    public long[] Keys => _modelDataDic.Keys.ToArray();
    public Data.ModelData this[long index]
    {
        get
        {
            if (_modelDataDic.ContainsKey(index))
                return _modelDataDic[index];

            return null;
        }
    }
    protected override void OnLoad()
    {
        LoadData(_tableName);
        foreach (var contents in _dataDic)
        {
            _modelDataDic.Add(contents.Key, new Data.ModelData(contents.Key, contents.Value));
        }
    }
}
