using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Scripts.Views
{
    public class LoadBar : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _progress;
        [SerializeField] private Image _image;

        //[SerializeField] private Gradient _gradient;
        [ShowIf("_image")] [SerializeField] private TextMeshProUGUI _textMesh;
        [ShowIf("_textMesh")] [SerializeField] private string _suffix;

        private void OnValidate()
        {
            if (_image != null)
            {
                SetProgress(_progress);
            }
        }

        [Button]
        public void SetProgress(float value)
        {
            _progress = value;
            _image.fillAmount = _progress;
            SetValue();
        }

        private void SetValue()
        {
            //_image.color = _gradient.Evaluate(_progress.value);
            if (_textMesh != null)
                _textMesh.text = $"{_image.fillAmount * 100:0}{_suffix}";
        }
    }
}