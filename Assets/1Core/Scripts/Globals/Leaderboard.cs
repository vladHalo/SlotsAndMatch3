using UnityEngine;

public class Leadboard : MonoBehaviour
{
    public void ShowLeaderboard()
    {
        // if (!KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
        // {
        //     KTGameCenter.SharedCenter().Authenticate();
        // }
        //
        // KTGameCenter.SharedCenter().ShowLeaderboard("com.best.scores.leaderboard");
    }

    public void SubmitScoreFloat(float score)
    {
        // if (!KTGameCenter.SharedCenter().IsGameCenterAuthenticated())
        // {
        //     KTGameCenter.SharedCenter().Authenticate();
        // }
        //
        // KTGameCenter.SharedCenter().SubmitFloatScore(score, 2, "com.best.scores.leaderboard");
    }
}