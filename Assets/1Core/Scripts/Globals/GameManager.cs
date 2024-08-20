using System;
using _1Core.Scripts;
using _1Core.Scripts.Game;
using Core.Scripts.Views;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool startGame;
    public Button btnStart;
    public Button btnShop;

    public Leadboard leadboard;
    public LoadBar loadBar;
    public MoneyManager moneyManager;
    public BlockManager blockManager;
    public StatsManager statsManager;
    public SlotsManager slotsManager;
    public InputHandler inputHandler;
    public TimerGame timerGame;
    public HeartTimer heartTimer;
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

    public void StartGame(bool enable) => startGame = enable;

    public void EnableBtnStart() =>
        btnStart.interactable = statsManager.GetStats(StatsType.Hearts) > 0;

    [Button]
    public void AddLevel() => statsManager.AddStats(StatsType.Level, 1);

    public void Reset()
    {
        ES3.DeleteFile();
        moneyManager.SetMoney(100);
        statsManager.ResetStats(StatsType.Hearts);
        statsManager.ResetStats(StatsType.Stars);
        statsManager.ResetStats(StatsType.Level);
        heartTimer.Reset();
        timerGame.StopTimer();
        timerGame.ResetTimer();
        loadBar.SetProgress(0);
        btnStart.interactable = true;
        btnShop.interactable = true;
        taskSubjectsManager.OffTaskImage();
        slotsManager.RandomSlot();
        slotsManager.EnableSlots();
    }
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