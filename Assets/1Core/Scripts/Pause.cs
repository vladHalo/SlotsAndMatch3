using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Button[] _pauseButton;

    private void Start()
    {
        _pauseButton[0].onClick.AddListener(() => Time.timeScale = 0);
        _pauseButton[1].onClick.AddListener(() => Time.timeScale = 1);
    }

    public void PauseGame() => Time.timeScale = 1;
}