using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ModelManager : TSingletonMono<ModelManager>
{
    #region Field
    ModelTable modelTable;

    DoublyLinkedList<Model> _modelBuffer = new DoublyLinkedList<Model>();
    DoublyLinkedListNode<Model> _middleNode;

    int currentModelIndex=1;
    int maxModelIndex;

    int bufferSize = 5;
    int minResetBufferIndex;
    int maxResetBufferIndex;
    #endregion

    protected override void OnInitialize()
    {
        modelTable = DataManager.ModelTable;
        maxModelIndex = DataManager.ModelTable.Keys.Length;

        minResetBufferIndex = (bufferSize / 2 + bufferSize % 2);
        maxResetBufferIndex = maxModelIndex - (bufferSize / 2);

        PreloadModelAsync(currentModelIndex).Forget();
    }

    #region LoadModel Method
    async UniTask PreloadModelAsync(int middleIndex = 1)
    {
        (int firstIndex, int lastIndex) = GetRange(middleIndex, bufferSize);

        for (int i = firstIndex; i <= lastIndex; i++)
            await LoadModelAsync(i,isMiddleNode:middleIndex==i);
        _middleNode.Value.Spawn();
        IsLoad = true;
    }

    async UniTask LoadModelAsync(long modelIndex,bool addLast=true,bool isMiddleNode=false)
    {
        try
        {
            var resourcePath = modelTable[modelIndex].ResourcePath;
            var asyncOperation = Resources.LoadAsync<GameObject>(resourcePath);
            await asyncOperation.ToUniTask();

            if (asyncOperation.asset is GameObject modelObject)
            {
                var instantiatedObject = Instantiate(modelObject,transform);
                await UniTask.Yield(); // Ensure the instantiation completes in the next frame
                var model = instantiatedObject.GetComponent<Model>();

                if (model == null)
                {
                    Destroy(instantiatedObject);
                    throw new System.Exception($"The loaded asset at path: {resourcePath} does not contain a Model component.");
                }

                model.InitReference(modelIndex);

                if (addLast)
                {
                    _modelBuffer.AddLast(model);
                    model.transform.SetAsLastSibling();
                }
                else
                {
                    _modelBuffer.AddFirst(model);
                    model.transform.SetAsFirstSibling();

                }

                if (isMiddleNode && addLast && _middleNode == null)
                    _middleNode = _modelBuffer.Tail;
            }
            else
            {
                throw new System.Exception($"Failed to load model at path: {resourcePath}");
            }                  
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"An error occurred while loading models: {ex.Message}");
        }
    }

    (int, int) GetRange(int startIndex, int rangeCount)
    {
        int halfRange = rangeCount / 2;
        int start = Math.Max(1, startIndex - halfRange);
        int end = Math.Min(maxModelIndex, startIndex + halfRange);

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
    #endregion
    
    public async UniTask NextModel(bool isRight)
    {
        if ((isRight==false&&currentModelIndex <= 1) || (isRight==true&&currentModelIndex >= maxModelIndex))
            return;
        ShowNextModel(isRight);

        if(ShouldResetBuffer(isRight))
            await ResetModelBuffer(isRight);
    }
    private void ShowNextModel(bool isRight)
    {
        _middleNode.Value.Hide();
        Mathf.Clamp(currentModelIndex += isRight ? 1 : -1,1,maxModelIndex);

        var nextModel = isRight ? _middleNode.Next : _middleNode.Prev;
        _middleNode = nextModel;
        _middleNode.Value.Spawn();
    }
    bool ShouldResetBuffer(bool isRight)
    {
        if (isRight)
            return (currentModelIndex > minResetBufferIndex && currentModelIndex <= maxResetBufferIndex);
        else
            return (currentModelIndex >= minResetBufferIndex && currentModelIndex < maxResetBufferIndex);      
    }
    
    async UniTask ResetModelBuffer(bool isRight)
    {
        var removeModel = isRight?_modelBuffer.Head:_modelBuffer.Tail;
        if (isRight)
            _modelBuffer.RemoveFirst();
        else
            _modelBuffer.RemoveLast();
        Destroy(removeModel.Value.gameObject);
        
        long newModelIndex = isRight ? _modelBuffer.Tail.Value.Data.Index : _modelBuffer.Head.Value.Data.Index;
        newModelIndex += isRight ? 1 : -1;
        await LoadModelAsync(newModelIndex, addLast: isRight);
    }

}
