using System;
using System.Collections;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class MoveSpriteToUI : MonoBehaviour
{
    [SerializeField] private MoveSpriteToUIModel[] _moveSpriteToUIModels;
    [SerializeField] private Factory<SpriteRenderer> _factorySprite;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private Camera _mainCamera;

    [Button]
    public void StartMove(Transform start, TypeItem typeItem)
    {
        foreach (var x in _moveSpriteToUIModels)
        {
            if (x.typeItem == typeItem)
            {
                var coinSprite = _factorySprite.Create(start.position, Quaternion.identity);
                coinSprite.sprite = x.sprite;
                //Vector3 uiWorldPosition = _mainCamera.ScreenToWorldPoint(x.target.position);
                StartCoroutine(MoveSprite(coinSprite.transform, start.position, x.target.position));
                break;
            }
        }
    }

    private IEnumerator MoveSprite(Transform sprite, Vector3 start, Vector3 end)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _duration);

            sprite.position = Vector3.Lerp(start, end, t);
            yield return null;
        }

        LeanPool.Despawn(sprite);
    }
}

[Serializable]
public class MoveSpriteToUIModel
{
    public TypeItem typeItem;
    public Sprite sprite;
    public Transform target;
}