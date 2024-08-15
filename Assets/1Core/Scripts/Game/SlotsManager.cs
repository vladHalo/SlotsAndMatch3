using System;
using System.Collections.Generic;
using _1Core.Scripts.Game;
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
    [SerializeField] private float _scaleBox = .01f, _scaleItem = .3f;
    [SerializeField] private float _heightMove = 4.5f;
    [SerializeField] private float _linesX, _linesY;
    [SerializeField] private float _paddingBoxX, _paddingBoxY;

    [SerializeField] private BoolArrayVisualizer _boolArrayVisualizer;
    [SerializeField] private RangeInt _rangeWay;
    [SerializeField] private RangeFloat _rangeDuration;
    [SerializeField] private Transform[] _lines;
    [SerializeField] private SpriteProcentModel[] _spriteProcentModels;

    private GameManager _gameManager;

    [SerializeField] private List<Slot> _slots;
    public List<Slot> enableSlots;

    private void OnValidate()
    {
        AlignBoxes();
    }

    private void Start()
    {
        _gameManager = GameManager.instance;
        _slots.ForEach(x =>
            x.spritesList.ForEach(
                y => y.sprite = _spriteProcentModels[Random.Range(0, (int)TypeItem.WoodShield)].sprite));
    }

    private void AlignBoxes()
    {
        for (int i = 0; i < _lines.Length; i++)
            _lines[i].position = _lines[i].position.SetX(_linesX).SetY(_linesY);

        int indexX = 0, indexY = 0;
        for (int i = 0; i < _slots.Count; i++)
        {
            if (i % 9 == 0)
            {
                indexX = 0;
                indexY++;
            }

            _slots[i].box.localPosition =
                _slots[i].box.localPosition.SetX(indexX * _paddingBoxX).SetY(indexY * _paddingBoxY);
            _slots[i].box.localScale = new Vector3(_scaleBox, _scaleBox, 1);
            _slots[i].spritesList[0].transform.localPosition = new Vector3(0, _heightMove, 0);
            _slots[i].spritesList[0].transform.localScale = new Vector3(_scaleItem, _scaleItem, 1);
            _slots[i].spritesList[1].transform.localScale = new Vector3(_scaleItem, _scaleItem, 1);
            indexX++;
        }
    }

    [Button]
    public void EnableSlots()
    {
        enableSlots.Clear();
        var level = _gameManager.statsManager.GetStats(StatsType.Level);
        bool isRandom = level >= _boolArrayVisualizer.list.Count;

        bool[] activeArray = isRandom
            ? GenerateRandomArray(_slots.Count)
            : _boolArrayVisualizer.list[level].array;

        for (int i = 0; i < _slots.Count; i++)
        {
            ConfigureSlot(_slots[i], activeArray[i]);
            if (activeArray[i]) enableSlots.Add(_slots[i]);
        }
    }

    private bool[] GenerateRandomArray(int size)
    {
        bool[] randomArray = new bool[size];
        for (int i = 0; i < size; i++)
        {
            randomArray[i] = Random.Range(0, 3) < 2;
        }

        return randomArray;
    }

    private void ConfigureSlot(Slot slot, bool isActive)
    {
        slot.box.localScale = new Vector3(_scaleBox, _scaleBox, 1);
        slot.box.gameObject.SetActive(isActive);
        slot.boxCollider.enabled = true;
    }


    [Button]
    public void SetSlots()
    {
        int i = 0;
        for (int j = 0; j < _lines[i].childCount; j++)
        {
            for (; i < _lines.Length; i++)
            {
                var slot = new Slot();
                slot.box = _lines[i].GetChild(j);
                slot.box.name = $"{i}{j}";
                slot.boxCollider = slot.box.GetComponent<BoxCollider2D>();
                slot.line = slot.box.GetChild(0);
                slot.spritesList = new List<SpriteRenderer>
                {
                    slot.line.GetChild(0).GetComponent<SpriteRenderer>(),
                    slot.line.GetChild(1).GetComponent<SpriteRenderer>()
                };
                _slots.Add(slot);
            }

            i = 0;
        }
    }

    [Button, HideInEditorMode]
    public void MoveLines()
    {
        _gameManager.gameStatus = GameStatus.Stop;
        int randomIndexWay = _rangeWay.RandomInRange();
        float duration = randomIndexWay / _rangeDuration.RandomInRange();
        AudioManager.instance.PlaySoundEffect(SoundType.Slot);
        
        enableSlots.ForEach(x => x.Move(_scaleItem, randomIndexWay, duration, _heightMove, _spriteProcentModels));
        Observable.Timer(TimeSpan.FromSeconds(duration+.1f)).Subscribe(_ =>
        {
            _gameManager.taskSubjectsManager.SetTaskItems();
            _gameManager.blockManager.SetTasksCount(_gameManager.taskSubjectsManager.selectedTypeItems);
            _gameManager.timerGame.StartTimer();
            _gameManager.gameStatus = GameStatus.Play;
        });
    }
}

[Serializable]
public class Slot
{
    public Transform box;

    public BoxCollider2D boxCollider;
    public Transform line;
    [HideInInspector] public int indexWay;
    public List<SpriteRenderer> spritesList;
    public TypeItem typeItem;

    public void Move(float scale, int randomIndexWay, float duration, float heightMove,
        SpriteProcentModel[] spriteModels)
    {
        AlignSlots(heightMove);
        indexWay = 0;

        line.DOLocalMoveY(-(randomIndexWay * heightMove), duration).OnUpdate(() =>
        {
            if (line.localPosition.y + indexWay * heightMove < .01f)
            {
                if (indexWay != randomIndexWay)
                    ChangePositionSlots(scale, heightMove, spriteModels);
                indexWay++;
            }
        });
    }

    private void ChangePositionSlots(float scale, float heightMove, SpriteProcentModel[] spriteModels)
    {
        float rand = Random.Range(0f, 100f);
        for (int i = 0; i < spriteModels.Length; i++)
        {
            if (rand > spriteModels[i].range.Min && rand <= spriteModels[i].range.Max)
            {
                typeItem = (TypeItem)i;
                ChangePositionSlotsDefault(scale, spriteModels[i].sprite, heightMove);
                break;
            }
        }
    }

    private void ChangePositionSlotsDefault(float scale, Sprite sprite, float heightMove)
    {
        var last = spritesList[^1];
        last.transform.localScale = new Vector3(scale, scale, 1);
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
public class SpriteProcentModel
{
    public Sprite sprite;
    public RangeFloat range;
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
    WoodShield,

    //
    Heart,
    Coin,
    Star,
    Time
}