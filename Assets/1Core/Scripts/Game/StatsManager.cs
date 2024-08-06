using System;
using TMPro;
using UnityEngine;

namespace _1Core.Scripts.Game
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private StatsModel[] _stats;

        public int GetStats(StatsType type) => _stats[(int)type].currentIndex;
        public void AddStats(StatsType type, int value) => _stats[(int)type].Add(value);

        public void ResetStats(StatsType type) => _stats[(int)type].Reset();
        public void ResetStats(int index) => _stats[index].Reset();
    }

    [Serializable]
    public struct StatsModel
    {
        [SerializeField] private StatsType _type;
        public int currentIndex;
        [SerializeField] private int _startIndex;
        [SerializeField] private TMP_Text _textStat;

        public void Reset()
        {
            currentIndex = _startIndex;
            if (_textStat != null) _textStat.text = currentIndex.ToString();
        }

        public void Add(int value)
        {
            currentIndex += value;
            if (_textStat != null) _textStat.text = currentIndex.ToString();
        }
    }

    [Serializable]
    public enum StatsType
    {
        Hearts,
        Stars
    }
}