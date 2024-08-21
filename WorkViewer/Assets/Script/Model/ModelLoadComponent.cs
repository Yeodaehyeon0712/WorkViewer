using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelLoadComponent
{
    //To Do : 예외 처리 할것 .
    public (DoublyLinkedList<Model>buffer, DoublyLinkedListNode<Model> middleNode) GetModelBuffer(int middleIndex,int bufferSize)
    {
        DoublyLinkedList<Model> _modelBuffer = new DoublyLinkedList<Model>();
        DoublyLinkedListNode<Model> _middleNode = null;

        (int firstIndex, int lastIndex) = GetRange(middleIndex, bufferSize);

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            var model = LoadModelAsync(modelIndex:i,addLast:true);
            _modelBuffer.AddLast(model);

            if (i == middleIndex)
                _middleNode = _modelBuffer.Tail;
        }

        return (_modelBuffer, _middleNode);
    }

    public Model LoadModelAsync(long modelIndex,bool addLast)
    {
        try
        {
            var resourcePath = DataManager.ModelTable[modelIndex].ResourcePath;
            if (string.IsNullOrEmpty(resourcePath))
                return null;

            var model = AddressableSystem.GetModel(resourcePath);
            if (model == null)
                return null;

            var instantiatedObject = Object.Instantiate(model, ModelManager.Instance.transform);

            if (instantiatedObject.TryGetComponent(out Model modelComponent) == false)
            {
                Object.Destroy(instantiatedObject);
                throw new System.Exception($"The loaded asset at path: {resourcePath} does not contain a Model component.");
            }

            modelComponent.InitReference(modelIndex);

            if (addLast)
                modelComponent.transform.SetAsLastSibling();           
            else            
                modelComponent.transform.SetAsFirstSibling();
            
            return modelComponent;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred while loading models: {ex.Message}");
            return null;
        }
    }

    (int, int) GetRange(int startIndex, int rangeCount)
    {
        var maxModelIndex = DataManager.ModelTable.Keys.Length;
        int halfRange = rangeCount / 2;
        int start = System.Math.Max(1, startIndex - halfRange);
        int end = System.Math.Min(maxModelIndex, startIndex + halfRange);

        while (end - start + 1 < rangeCount)
        {
            if (start > 1)
                start--;
            else if (end < maxModelIndex)
                end++;
            else
                break;
        }
        return (start, end);
    }
}
