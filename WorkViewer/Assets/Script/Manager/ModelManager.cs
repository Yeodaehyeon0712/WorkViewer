using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ModelManager : TSingletonMono<ModelManager>
{

    #region Field
    DoublyLinkedList<Model> _modelBuffer = new DoublyLinkedList<Model>();
    DoublyLinkedListNode<Model> _middleNode;

    int currentModelIndex=1;
    int maxModelIndex;
    int bufferSize = 5;
    int minResetBufferIndex;
    int maxResetBufferIndex;
    #endregion

    #region Component
    ModelLoadComponent modelLoader;
    ModelRoationComponent modelRotator;
    #endregion

    #region Init Method
    protected override void OnInitialize()
    {
        maxModelIndex = DataManager.ModelTable.Keys.Length;
        minResetBufferIndex = (bufferSize / 2 + bufferSize % 2);
        maxResetBufferIndex = maxModelIndex - (bufferSize / 2);

        modelLoader = new ModelLoadComponent();
        modelRotator = new ModelRoationComponent();

        SetModelBuffer(currentModelIndex);
        IsLoad = true;
    }

    void SetModelBuffer(int middleIndex)
    {
        var result = modelLoader.GetModelBuffer(middleIndex, bufferSize);
        _modelBuffer = result.buffer;
        _middleNode = result.middleNode;
        ActiveModel();
    }
    #endregion

    #region Active Node Method
    //To Do : ¸íÄª °í¹Î
    public void ActiveModel()
    {
        modelRotator.SetModel(_middleNode.Value.transform);
        _middleNode.Value.Spawn();
    }
    public void InactiveMode()
    {
        modelRotator.ResetRotation();
        _middleNode.Value.Hide();
    }
    public Model GetActiveModel()
    {
        return _middleNode.Value;
    }
    #endregion

    #region Next Model Method
    public void NextModel(bool isRight)
    {
        if ((isRight==false&&currentModelIndex <= 1) || (isRight==true&&currentModelIndex >= maxModelIndex))
            return;
        ShowNextModel(isRight);

        if(ShouldResetBuffer(isRight))
            ResetModelBuffer(isRight);
    }

    void ShowNextModel(bool isRight)
    {
        modelRotator.ResetRotation();
        _middleNode.Value.Hide();

        Mathf.Clamp(currentModelIndex += isRight ? 1 : -1,1,maxModelIndex);
        var nextModel = isRight ? _middleNode.Next : _middleNode.Prev;

        _middleNode = nextModel;
        modelRotator.SetModel(_middleNode.Value.transform);
        _middleNode.Value.Spawn();
    }

    bool ShouldResetBuffer(bool isRight)
    {
        if (isRight)
            return (currentModelIndex > minResetBufferIndex && currentModelIndex <= maxResetBufferIndex);
        else
            return (currentModelIndex >= minResetBufferIndex && currentModelIndex < maxResetBufferIndex);      
    }
    
    void ResetModelBuffer(bool isRight)
    {
        var removeModel = isRight?_modelBuffer.Head:_modelBuffer.Tail;
        if (isRight)
            _modelBuffer.RemoveFirst();
        else
            _modelBuffer.RemoveLast();
        Destroy(removeModel.Value.gameObject);
        
        long newModelIndex = isRight ? _modelBuffer.Tail.Value.Data.Index : _modelBuffer.Head.Value.Data.Index;
        newModelIndex += isRight ? 1 : -1;

        var newModel = modelLoader.LoadModelAsync(newModelIndex, addLast: isRight);
        if (isRight)
            _modelBuffer.AddLast(newModel);
        else
            _modelBuffer.AddFirst(newModel);

    }
    #endregion

    #region RotationComponent
    void Update()
    {
        modelRotator.UpdateModelRotation();
    }
    #endregion
}
