using Core.Scripts.Tools.Extensions;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class InfoText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _textInfo;
    [SerializeField] private Transform _target;
    [SerializeField] private string _text;
    [SerializeField] private float _height, _duration;

    [SerializeField] private Color _color;
    
    [Button]
    public void EnableInfoText()
    {
        EnableInfoText(_target.position, _text, _height, _duration, _color);
    }

    public void EnableInfoText(Vector3 position, string text, float height, float duration, Color color)
    {
        gameObject.SetActive(true);
        _textInfo.DOKill(true);
        _textInfo.text = text;
        _textInfo.color = color;
        _textInfo.transform.position = position.AddY(0);
        _textInfo.transform.DOMoveY(position.y + height, duration)
            .OnComplete(() => gameObject.SetActive(false));
    }
}