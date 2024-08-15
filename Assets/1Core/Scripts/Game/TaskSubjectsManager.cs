using System;
using System.Collections.Generic;
using System.Linq;
using _1Core.Scripts.Game;
using Core.Scripts.Tools.Extensions;
using Core.Scripts.Views;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TaskSubjectsManager : MonoBehaviour
{
    public int itemCount;
    [SerializeField] private LoadBar _loadBar;
    [SerializeField] private TaskModel[] _taskModels;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private List<TypeItem> _typeItems;

    public List<TypeItem> selectedTypeItems;

    [Space] [SerializeField] private List<TypeItemModel> _levelTypeItems;

    private GameManager _gameManager;
    private int _totalTaskCount, _currentTaskTotal;

    public UnityEvent onWinEvent;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void OffTaskImage() => _taskModels.ForEach(x => x.taskImage.gameObject.SetActive(false));

    public void MinusTask(TypeItem typeItem)
    {
        var item = _taskModels.FirstOrDefault(x => x.taskTypeItem == typeItem);
        item.taskIndex--;
        _currentTaskTotal--;
        item.taskText.text = $"{item.taskIndex}";
        if (item.taskIndex <= 0) CheckWin();
        _loadBar.SetProgress((float)(_totalTaskCount - _currentTaskTotal) / _totalTaskCount);
    }

    private void CheckWin()
    {
        for (int i = 0; i < _taskModels.Length; i++)
        {
            if (_taskModels[i].taskImage.gameObject.activeSelf && _taskModels[i].taskIndex > 0)
            {
                return;
            }
        }

        AudioManager.instance.PlaySoundEffect(SoundType.Win);
        _gameManager.gameStatus = GameStatus.Stop;
        onWinEvent?.Invoke();
    }

    [Button]
    private void SetTaskItemsRandom()
    {
        itemCount = Random.Range(1, 4);
        _totalTaskCount = 0;
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

        _currentTaskTotal = _totalTaskCount;
    }

    [Button]
    public void SetTaskItems()
    {
        var level = _gameManager.statsManager.GetStats(StatsType.Level);
        if (level >= _levelTypeItems.Count)
        {
            SetTaskItemsRandom();
            return;
        }

        _totalTaskCount = 0;
        selectedTypeItems.Clear();

        var levelItems = _levelTypeItems[level].typeItems;

        for (int i = 0; i < _taskModels.Length; i++)
            _taskModels[i].taskImage.gameObject.SetActive(i < levelItems.Count);

        selectedTypeItems = new List<TypeItem>(levelItems);
        for (int i = 0; i < selectedTypeItems.Count; i++)
        {
            _taskModels[i].taskTypeItem = selectedTypeItems[i];
            _taskModels[i].taskImage.sprite = _sprites[(int)selectedTypeItems[i]];
            SetTasksCount(i, selectedTypeItems[i]);
        }

        _currentTaskTotal = _totalTaskCount;
    }

    private void SetTasksCount(int index, TypeItem typeItem)
    {
        _taskModels[index].taskIndex = _gameManager.slotsManager.enableSlots.Count(x => x.typeItem == typeItem);
        _totalTaskCount += _taskModels[index].taskIndex;
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