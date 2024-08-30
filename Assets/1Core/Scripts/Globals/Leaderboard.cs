using UnityEngine;

public class Leadboard : MonoBehaviour
{
    private void Start()
    {
        KTGameCenter.SharedCenter().Authenticate();
    }

    public void ShowLeaderboard()
    {
        if (!KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
        {
            KTGameCenter.SharedCenter().Authenticate();
        }
        
        KTGameCenter.SharedCenter().ShowLeaderboard("com.vv.highscores.board");
    }

    public void SubmitScore(int score)
    {
        if (!KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
        {
            KTGameCenter.SharedCenter().Authenticate();
        }
        
        KTGameCenter.SharedCenter().SubmitScore(score, "com.vv.highscores.board");
    }
}