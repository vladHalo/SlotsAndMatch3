using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;

namespace _1Core.Scripts.Game
{
    public class StatsManager : MonoBehaviour
    {
        [SerializeField] private StatsModel[] _stats;

        public void Load() => _stats.ForEach(x => x.Load());

        public int GetStats(StatsType type) => _stats[(int)type].currentIndex;
        public void AddStats(StatsType type, int value) => _stats[(int)type].Add(value);

        public void ResetStats(StatsType type) => _stats[(int)type].Reset();
        public bool IsBiggerThanMaxStats(StatsType type) => _stats[(int)type].IsBiggerThanMax();
    }

    [Serializable]
    public class StatsModel
    {
        [SerializeField] private StatsType _type;
        public int currentIndex;
        [SerializeField] private int _startIndex;
        [SerializeField] private TMP_Text _textStat;
        [SerializeField] private bool _isMaxIndex;

        [ShowIf("_isMaxIndex"), SerializeField]
        private int _maxIndex;

        private const string _suffix = "Stats";

        public void Load()
        {
            if (!ES3.KeyExists(_type + _suffix)) return;
            currentIndex = ES3.Load<int>(_type + _suffix);
            if (_textStat != null) _textStat.text = currentIndex.ToString();
            if (_type == StatsType.Level) _textStat.text = "LEVEL: " + (currentIndex + 1);
        }

        public void Reset()
        {
            currentIndex = _startIndex;
            if (_textStat != null) _textStat.text = currentIndex.ToString();
            if (_type == StatsType.Level) _textStat.text = "LEVEL: " + (currentIndex + 1);
        }

        public void Add(int value)
        {
            currentIndex += value;
            if (_isMaxIndex && currentIndex > _maxIndex) currentIndex = _maxIndex;
            if (_textStat != null) _textStat.text = currentIndex.ToString();
            if (_type == StatsType.Level) _textStat.text = "LEVEL: " + (currentIndex + 1);
            if (_type == StatsType.Stars) GameManager.instance.leadboard.SubmitScore(currentIndex);
            ES3.Save(_type + _suffix, currentIndex);
        }

        public bool IsBiggerThanMax() => currentIndex >= _maxIndex;
    }

    [Serializable]
    public enum StatsType
    {
        Hearts,
        Stars,
        Level
    }
}