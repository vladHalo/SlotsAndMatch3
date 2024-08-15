using _1Core.Scripts.Game;
using Core.Scripts.Tools.Tools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private RangeFloat _rangeTime;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private float[] _timeLevel;

    private bool _isStartTimer;
    private float _time;
    private GameManager _gameManager;

    public UnityEvent onFinishTime;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (!_isStartTimer) return;

        _time -= Time.deltaTime;
        SetTextTimer();
        if (!(_time <= 0)) return;

        _isStartTimer = false;
        _time = 0;
        SetTextTimer();
        _gameManager.gameStatus = GameStatus.Stop;
        AudioManager.instance.PlaySoundEffect(SoundType.Lose);
        onFinishTime?.Invoke();
    }

    public void AddTime(float value)
    {
        _time += value;
        SetTextTimer();
    }

    [Button]
    public void StartTimer()
    {
        _isStartTimer = true;
        var level = _gameManager.statsManager.GetStats(StatsType.Level);
        if (level < _timeLevel.Length)
        {
            _time = _timeLevel[level];
        }
        else
        {
            _time = _rangeTime.RandomInRange();
        }

        SetTextTimer();
    }

    public void StopTimer() => _isStartTimer = false;
    public void ResetTimer() => _timerText.text = "0.0";

    private void SetTextTimer() => _timerText.text = _time.ToHumanTimeFormat();
}