using Core.Scripts.Tools.Tools;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimerGame : MonoBehaviour
{
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
        onFinishTime?.Invoke();
    }

    [Button]
    public void StartTimer(int levelIndex)
    {
        _isStartTimer = true;
        _time = _timeLevel[levelIndex];
        SetTextTimer();
    }

    public void StopTimer() => _isStartTimer = false;

    private void SetTextTimer() => _timerText.text = _time.ToHumanTimeFormat();
}