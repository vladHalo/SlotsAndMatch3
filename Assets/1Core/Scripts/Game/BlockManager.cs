using System;
using System.Collections.Generic;
using Core.Scripts.Tools.Extensions;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private BlockItemFactory _blockItemFactory;
    [SerializeField] private List<Sprite> _crashSprite;
    [SerializeField] private List<SpriteBlockModel> _spriteBlockModels;
    [SerializeField] private List<BlockModel> _blockModels;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    public void SetDesign(int index)
    {
        _crashSprite.Clear();
        _crashSprite = new List<Sprite>(_spriteBlockModels[index].sprites);
    }

    private void Add(SpriteRenderer spriteRenderer, Slot slot)
    {
        var block = new BlockModel
        {
            indexCrash = 0,
            blockSprite = spriteRenderer,
            target = slot
        };
        slot.boxCollider.enabled = false;
        block.blockSprite.sprite = _crashSprite[0];
        _blockModels.Add(block);
    }

    [Button]
    public void SetTasksCount(List<TypeItem> typeItems)
    {
        var slotsType = new List<Slot>();
        _gameManager.slotsManager.enableSlots.ForEach(x =>
        {
            if (typeItems.Contains(x.typeItem))
            {
                slotsType.Add(x);
            }
        });

        var countBlockCreate = Random.Range(slotsType.Count / 2, slotsType.Count);

        for (int i = 0; i < countBlockCreate; i++)
        {
            var slot = _gameManager.slotsManager.enableSlots.Random();
            if (!slotsType.Contains(slot)) slotsType.Add(slot);
        }

        for (int i = 0; i < countBlockCreate; i++)
        {
            var target = slotsType.Random();
            slotsType.Remove(target);
            Add(_blockItemFactory.Create(target.box), target);
        }
    }

    [Button]
    public void Crash(GameObject blockSlot)
    {
        for (int i = 0; i < _blockModels.Count; i++)
        {
            if (_blockModels[i].blockSprite.gameObject == blockSlot)
            {
                _blockModels[i].indexCrash++;
                if (_blockModels[i].indexCrash >= _crashSprite.Count)
                {
                    _blockModels[i].target.boxCollider.enabled = true;
                    LeanPool.Despawn(_blockModels[i].blockSprite.gameObject);
                    _blockModels.RemoveAt(i);
                    return;
                }

                _blockModels[i].blockSprite.sprite = _crashSprite[_blockModels[i].indexCrash];
                return;
            }
        }
    }

    public void DespawnAll()
    {
        _blockModels.ForEach(x => LeanPool.Despawn(x.blockSprite.gameObject));
        _blockModels.Clear();
    }
}

[Serializable]
public class BlockModel
{
    public int indexCrash;
    public SpriteRenderer blockSprite;
    public Slot target;
}

[Serializable]
public class SpriteBlockModel
{
    public List<Sprite> sprites;
}