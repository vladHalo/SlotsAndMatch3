using System;
using System.Collections.Generic;
using Core.Scripts.Tools.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskSubjectsManager : MonoBehaviour
{
    [SerializeField] private TaskModel[] _taskModels;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private List<TypeItem> _typeItems;

    public List<TypeItem> selectedTypeItems;

    [Space] [SerializeField] private List<TypeItemModel> _levelTypeItems;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void MinusTask(TypeItem typeItem)
    {
        _taskModels.ForEach(x =>
        {
            if (x.taskTypeItem == typeItem)
            {
                x.taskIndex--;
                x.taskText.text = $"{x.taskIndex}";
            }
        });
    }

    [Button]
    public void SetTaskItemsRandom(int itemCount)
    {
        selectedTypeItems.Clear();
        InitializeTypeItems();

        for (int i = 0; i < _taskModels.Length; i++)
            _taskModels[i].taskImage.gameObject.SetActive(i < itemCount);

        for (int i = 0; i < itemCount; i++)
        {
            var randomItem = _typeItems.Random();
            _typeItems.Remove(randomItem);
            selectedTypeItems.Add(randomItem);
            _taskModels[i].taskTypeItem = randomItem;
            _taskModels[i].taskImage.sprite = _sprites[(int)randomItem];
            SetTasksCount(i, randomItem);
        }
    }

    [Button]
    public void SetTaskItems(int levelIndex)
    {
        selectedTypeItems.Clear();

        var levelItems = _levelTypeItems[levelIndex].typeItems;

        for (int i = 0; i < _taskModels.Length; i++)
            _taskModels[i].taskImage.gameObject.SetActive(i < levelItems.Count);

        selectedTypeItems = new List<TypeItem>(levelItems);
        for (int i = 0; i < selectedTypeItems.Count; i++)
        {
            _taskModels[i].taskTypeItem = selectedTypeItems[i];
            _taskModels[i].taskImage.sprite = _sprites[(int)selectedTypeItems[i]];
            SetTasksCount(i, selectedTypeItems[i]);
        }
    }

    private void SetTasksCount(int index, TypeItem typeItem)
    {
        _gameManager.slotsManager.slots.ForEach(x =>
        {
            if (x.typeItem == typeItem)
            {
                _taskModels[index].taskIndex++;
            }
        });
        _taskModels[index].taskText.text = $"{_taskModels[index].taskIndex}";
    }

    private void InitializeTypeItems()
    {
        _typeItems.Clear();
        for (int i = 0; i < _sprites.Length; i++)
        {
            _typeItems.Add((TypeItem)i);
        }
    }
}

[Serializable]
public class TypeItemModel
{
    public List<TypeItem> typeItems;
}

[Serializable]
public class TaskModel
{
    public int taskIndex;
    public TypeItem taskTypeItem;
    public Image taskImage;
    public TextMeshProUGUI taskText;
}