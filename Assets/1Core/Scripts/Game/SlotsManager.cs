using System;
using System.Collections.Generic;
using Core.Scripts.Tools.Extensions;
using Core.Scripts.Tools.Tools;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
using RangeInt = Core.Scripts.Tools.Tools.RangeInt;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] private float _heightLines, _paddingLinesX, _paddingLinesY;
    [SerializeField] private float _heightMove;

    //[SerializeField] private BlockItemFactory _blockItemFactory;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private RangeInt _rangeWay;
    [SerializeField] private RangeFloat _rangeDuration;
    [SerializeField] private LineModel[] _linesModel;
    [SerializeField] private Sprite[] _sprites;

    private GameManager _gameManager;

    public List<Slot> slots;

    private void OnValidate()
    {
        AlignBoxes();
    }

    private void Start()
    {
        _gameManager = GameManager.instance;
        slots.ForEach(x => x.spritesList.ForEach(y => y.sprite = _sprites.Random()));
    }

    private void AlignBoxes()
    {
        for (int i = 0; i < _linesModel.Length; i++)
        {
            _linesModel[i].line.position = _linesModel[i].line.position.SetY(_heightLines);
            if (i == 4) continue;
            _linesModel[i].line.position = _linesModel[i].line.position.SetX(_paddingLinesX * _linesModel[i].index);
        }

        int index = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (i % 9 == 0)
            {
                index = 0;
            }

            slots[i].box.localPosition = slots[i].box.localPosition.SetY(index * _paddingLinesY);
            index++;
        }
    }

    //[Button]
    // private void SetSortingLayer()
    // {
    //     int index = 0;
    //     for (int i = 0; i < slots.Count; i++)
    //     {
    //         if (i % 9 == 0)
    //         {
    //             index = 0;
    //         }
    //
    //         slots[i].box.name = i.ToString();
    //         //slots[i].box.AddComponent<BoxCollider2D>();
    //         slots[i].box.GetComponent<SpriteRenderer>().sortingLayerName = "Layer" + index;
    //         slots[i].box.GetComponent<SpriteMask>().sortingLayerName = "Layer" + index;
    //         slots[i].spritesList[0].GetComponent<SpriteRenderer>().sortingLayerName = "Layer" + index;
    //         slots[i].spritesList[1].GetComponent<SpriteRenderer>().sortingLayerName = "Layer" + index;
    //         index++;
    //     }
    // }

    [Button]
    public void EnableSlots() => _mapManager.EnableSlots(0, slots);

    [Button]
    public void SetSlots()
    {
        _linesModel.ForEach(x =>
        {
            foreach (Transform item in x.line)
            {
                var slot = new Slot();
                slot.box = item;
                slot.line = item.GetChild(0);
                slot.spritesList = new List<SpriteRenderer>
                {
                    slot.line.GetChild(0).GetComponent<SpriteRenderer>(),
                    slot.line.GetChild(1).GetComponent<SpriteRenderer>()
                };
                slots.Add(slot);
            }
        });
    }

    [Button, HideInEditorMode]
    public void MoveLines()
    {
        _gameManager.gameStatus = GameStatus.Stop;
        int randomIndexWay = _rangeWay.RandomInRange();
        float duration = randomIndexWay / _rangeDuration.RandomInRange();

        slots.ForEach(x => x.Move(randomIndexWay, duration, _heightMove, _sprites));
        Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(_ =>
        {
            _gameManager.taskSubjectsManager.SetTaskItemsRandom(2);
            _gameManager.blockManager.SetTasksCount(_gameManager.taskSubjectsManager.selectedTypeItems);
            _gameManager.timerGame.StartTimer(0);
            _gameManager.gameStatus = GameStatus.Play;
        });
    }

    private void EnableChildren(Transform parent, bool enable)
    {
        foreach (Transform i in parent)
        {
            i.gameObject.SetActive(enable);
        }
    }
}

[Serializable]
public class Slot
{
    public Transform box;
    public Transform line;
    [HideInInspector] public int indexWay;
    public List<SpriteRenderer> spritesList;
    public TypeItem typeItem;

    public void Move(int randomIndexWay, float duration, float heightMove, Sprite[] sprites)
    {
        AlignSlots(heightMove);
        indexWay = 0;

        line.DOLocalMoveY(-(randomIndexWay * heightMove), duration).OnUpdate(() =>
        {
            if (line.localPosition.y + indexWay * heightMove < .01f)
            {
                if (indexWay != randomIndexWay)
                    ChangePositionSlots(heightMove, sprites);
                indexWay++;
            }
        });
    }

    private void ChangePositionSlots(float heightMove, Sprite[] sprites)
    {
        int rand = Random.Range(0, sprites.Length);
        typeItem = (TypeItem)rand;
        ChangePositionSlotsDefault(sprites[rand], heightMove);
    }

    private void ChangePositionSlotsDefault(Sprite sprite, float heightMove)
    {
        var last = spritesList[^1];
        last.transform.localScale = new Vector3(.1f, .1f, .1f);
        last.transform.localPosition =
            last.transform.localPosition.SetY(spritesList[0].transform.localPosition.y + heightMove);
        spritesList.RemoveAt(spritesList.Count - 1);
        spritesList.Insert(0, last);
        spritesList[0].sprite = sprite;
    }

    private void AlignSlots(float heightMove)
    {
        spritesList[0].transform.localPosition = Vector3.zero;
        spritesList[1].transform.localPosition = Vector3.zero.SetY(heightMove);
        line.localPosition = line.localPosition.SetY(0);
    }
}

[Serializable]
public class LineModel
{
    public int index;
    public Transform line;
}

public enum TypeItem
{
    Bomb,
    Energy,
    Gold,
    Key,
    Poison,
    Rube,
    Shield,
    WoodShield
}