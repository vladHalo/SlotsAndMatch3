using DG.Tweening;
using UnityEngine;

#pragma warning disable 0649
namespace Core.Scripts.Tools.Extensions
{
    public static class DOTweenExtensions
    {
        public static Tweener DOVolume(this AudioSource audioSource, float endValue, float duration)
        {
            return DOVirtual.Float(audioSource.volume, endValue, duration, value => audioSource.volume = value);
        }
    }
}