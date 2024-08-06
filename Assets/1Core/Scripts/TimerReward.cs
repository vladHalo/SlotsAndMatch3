using System;
using System.Collections;
using UnityEngine;

namespace _1Core.Scripts
{
    public class TimerReward : MonoBehaviour
    {
        [SerializeField] private float _startTimeReward;
        private WaitForSeconds _waitForSeconds;
        private DateTime _exitTime;

        public float timeReward;
        public Reward reward;

        private void Start()
        {
            _waitForSeconds = new WaitForSeconds(1);
            if (ES3.KeyExists(Str.ExitTime))
            {
                long exitTimeTicks = ES3.Load<long>(Str.ExitTime);
                _exitTime = new DateTime(exitTimeTicks, DateTimeKind.Utc);
                timeReward = ES3.Load<float>(Str.RewardTime);

                // Обчислюємо час, який пройшов з моменту виходу з гри
                TimeSpan elapsedTime = DateTime.UtcNow - _exitTime;
                // Віднімаємо цей час з часу винагороди
                timeReward -= (float)elapsedTime.TotalSeconds;

                // Якщо винагорода вже мала б бути відкрита, відкриваємо її
                if (timeReward <= 0 && reward != null)
                {
                    reward.OpenBtnReward(true, 1);
                }
                else
                {
                    SetTimeText();
                    StartCoroutine(UpdateTime());
                }
            }
            else
            {
                StartTimer();
            }
        }

        public void StartTimer()
        {
            timeReward = _startTimeReward;
            SetTimeText();
            StartCoroutine(UpdateTime());
        }

        private IEnumerator UpdateTime()
        {
            while (timeReward > 0)
            {
                yield return _waitForSeconds;
                timeReward--;
                SetTimeText();
            }

            if (reward != null)
            {
                reward.OpenBtnReward(true, 1);
            }
        }

        private void SetTimeText()
        {
            if (reward != null)
            {
                reward.SetTime(timeReward);
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            // При виході з програми або включенні режиму паузи зберігаємо дані
            if (pauseStatus)
            {
                ES3.Save(Str.ExitTime, DateTime.UtcNow.Ticks);
                ES3.Save(Str.RewardTime, timeReward);
            }
        }
    }
}
