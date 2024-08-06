using _1Core.Scripts;
using _1Core.Scripts.Game;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MoneyManager moneyManager;
    public BlockManager blockManager;
    public StatsManager statsManager;
    public SlotsManager slotsManager;
    public InputHandler inputHandler;
    public TimerGame timerGame;
    public TaskSubjectsManager taskSubjectsManager;

    public GameStatus gameStatus;
    
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
}