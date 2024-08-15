using System;
using System.Collections;
using _1Core.Scripts.Game;
using Core.Scripts.Tools.Tools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class HeartTimer : MonoBehaviour
{
    [SerializeField] private float _startTimeHeart = 300f;
    [SerializeField] private StatsManager _statsManager;
    [SerializeField] private TextMeshProUGUI _textTime;

    private WaitForSeconds _waitForSeconds;
    private DateTime _exitTime;
    public float timeHeart;

    private Coroutine _currentCoroutine;

    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(1);
        _statsManager.Load();
        if (ES3.KeyExists(Str.ExitTimeHeart))
        {
            _exitTime = new DateTime(ES3.Load<long>(Str.ExitTimeHeart), DateTimeKind.Utc);
            if (ES3.KeyExists(Str.HeartTime)) timeHeart = ES3.Load<float>(Str.HeartTime);

            float elapsedTime = (float)(DateTime.UtcNow - _exitTime).TotalSeconds;
            float totalElapsedTime = elapsedTime - timeHeart;

            if (totalElapsedTime > 0)
            {
                int heartsToAdd = (int)(totalElapsedTime / _startTimeHeart);
                timeHeart = _startTimeHeart - totalElapsedTime % _startTimeHeart;

                _statsManager.AddStats(StatsType.Hearts, heartsToAdd);
            }
            else
            {
                timeHeart -= elapsedTime;
            }

            if (!_statsManager.IsBiggerThanMaxStats(StatsType.Hearts))
            {
                _textTime.text = timeHeart.ToHumanTimeFormat();
                _currentCoroutine ??= StartCoroutine(UpdateTime());
            }
        }
        else
        {
            StartHeartTimerIfNeeded();
        }
    }

    [Button]
    public void RemoveHeart()
    {
        _statsManager.AddStats(StatsType.Hearts, -1);
        StartHeartTimerIfNeeded();
    }

    private void StartHeartTimerIfNeeded()
    {
        if (_statsManager.IsBiggerThanMaxStats(StatsType.Hearts)) return;

        if (_currentCoroutine == null)
        {
            timeHeart = _startTimeHeart;
            _currentCoroutine = StartCoroutine(UpdateTime());
        }
    }

    private IEnumerator UpdateTime()
    {
        while (timeHeart > 0)
        {
            _textTime.text = timeHeart.ToHumanTimeFormat();
            yield return _waitForSeconds;
            timeHeart--;
        }

        _statsManager.AddStats(StatsType.Hearts, 1);
        _textTime.text = "";

        _currentCoroutine = null;
        StartHeartTimerIfNeeded();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            ES3.Save(Str.ExitTimeHeart, DateTime.UtcNow.Ticks);
            ES3.Save(Str.HeartTime, timeHeart);
        }
    }
}