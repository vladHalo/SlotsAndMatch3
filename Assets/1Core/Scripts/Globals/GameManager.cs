using System;
using _1Core.Scripts;
using _1Core.Scripts.Game;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject btnStart;

    public MoneyManager moneyManager;
    public BlockManager blockManager;
    public StatsManager statsManager;
    public SlotsManager slotsManager;
    public InputHandler inputHandler;
    public TimerGame timerGame;
    public TaskSubjectsManager taskSubjectsManager;

    public GameStatus gameStatus;

    public BonusModel[] bonuses;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void EnableBtnStart() =>
        btnStart.gameObject.SetActive(statsManager.GetStats(StatsType.Hearts) > 0);

    [Button]
    public void AddLevel() => statsManager.AddStats(StatsType.Level, 1);
}

[Serializable]
public class BonusModel
{
    public TypeBonus typeBonus;
    public int index;
}

public enum TypeBonus
{
    Time,
    Gold,
    Star
}