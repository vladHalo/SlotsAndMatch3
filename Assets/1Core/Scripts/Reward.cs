using Core.Scripts.Tools.Extensions;
using Core.Scripts.Tools.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RangeInt = Core.Scripts.Tools.Tools.RangeInt;

namespace _1Core.Scripts
{
    public class Reward : MonoBehaviour
    {
        [SerializeField] private TimerReward _timerReward;
        [SerializeField] private MoneyManager _moneyManager;
        [SerializeField] private TextMeshProUGUI _timeRewardText, _rewardText;
        [SerializeField] private Button _btnOpen, _btnAddMoney;
        [SerializeField] private Image _btnOpenImage;
        [SerializeField] private RangeInt _rangeInt;

        private int _reward;

        private void Start()
        {
            _timeRewardText.gameObject.SetActive(true);
            _btnOpen.onClick.AddListener(() =>
            {
                OpenBtnReward(false, .1f);
                _reward = _rangeInt.RandomInRange();
                _rewardText.text = _reward.ToString();
            });

            _btnAddMoney.onClick.AddListener(() =>
            {
                //_moneyManager.ChangeMoney(_reward, false);
                _timerReward.StartTimer();
                _timeRewardText.gameObject.SetActive(true);
            });
        }

        private void OnDestroy()
        {
            _btnOpen.onClick.RemoveAllListeners();
            _btnAddMoney.onClick.RemoveAllListeners();
        }

        public void SetTime(float value) => _timeRewardText.text = value.ToHumanTimeFormat();

        public void OpenBtnReward(bool interactBtn, float alpha)
        {
            _btnOpen.interactable = interactBtn;
            _btnOpenImage.color = _btnOpenImage.color.SetAlpha(alpha);
            _timeRewardText.gameObject.SetActive(false);
        }
    }
}