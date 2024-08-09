using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
public class Model : MonoBehaviour
{
    ModelData _data;
    public ModelData Data => _data;
    public void InitReference(long index)
    {     
        _data = DataManager.ModelTable[index];
        gameObject.SetActive(false);
    }
    public void Spawn()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
