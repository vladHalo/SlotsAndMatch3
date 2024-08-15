using System;
using _1Core.Scripts.Game;
using Core.Scripts.Tools.Extensions;
using Core.Scripts.Tools.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _1Core.Scripts
{
    public class MoneyManager : MonoBehaviour
    {
        public float money;

        //[SerializeField] private float _bestWinMoney;
        [SerializeField] private TextMeshProUGUI[] _moneyTexts;
        [SerializeField] private TextMeshProUGUI _moneyTextWin;
        [SerializeField] private TextMeshProUGUI _starTextWin;
        [SerializeField] private Transform _panelMoneyText;

        private bool _canMoveTween;

        //private float _winPrize;
        private GameManager _gameManager;

        private void OnValidate()
        {
            _moneyTexts.ForEach(x => x.text = $"{money}");
        }

        private void Start()
        {
            _gameManager = GameManager.instance;

            _canMoveTween = true;
            if (ES3.KeyExists(Str.Money))
            {
                money = ES3.Load<float>(Str.Money);
            }

            _moneyTexts.ForEach(x => x.text = money.ToIdleFormat(2, 0));

            // if (ES3.KeyExists(Str.BestWinMoney))
            // {
            //     _bestWinMoney = ES3.Load<float>(Str.BestWinMoney);
            //     _moneyBestTextWin.text = $"Best Score: {_bestWinMoney.ToIdleFormat(2, 0)}";
            // }
        }

        public void AddMoney(float value)
        {
            money += value;
            money = (float)Math.Round(money, 2);
            _moneyTexts.ForEach(x => x.text = money.ToIdleFormat(2, 0));
            ES3.Save(Str.Money, money);
        }

        public void AddMoney()
        {
            var level = _gameManager.statsManager.GetStats(StatsType.Level);
            var winPrize = level * 10;
            if (level >= 30) winPrize = 300;
            money += winPrize;
            money = (float)Math.Round(money, 2);
            _moneyTexts.ForEach(x => x.text = money.ToIdleFormat(2, 0));
            _moneyTextWin.text = winPrize.ToIdleFormat(2, 0);
            ES3.Save(Str.Money, money);
            _gameManager.statsManager.AddStats(StatsType.Stars, 3);
            _starTextWin.text = "3";
        }

        public void MoveTween()
        {
            if (!_canMoveTween) return;
            _canMoveTween = false;
            _moneyTexts.ForEach(x => x.color = new Color(0.4f, 0, 0));
            _panelMoneyText.DOScale(new Vector3(1.3f, 1.3f, 1.3f), .4f)
                .OnComplete(() =>
                    {
                        _panelMoneyText.DOScale(Vector3.one, .4f).OnComplete(() => _canMoveTween = true);
                        _moneyTexts.ForEach(x => x.color = Color.white);
                    }
                );
        }

        public bool CanMinus(int count)
        {
            if (money >= count)
            {
                money -= count;
                money = (float)Math.Round(money, 2);
                _moneyTexts.ForEach(x => x.text = money.ToIdleFormat(2, 0));
                ES3.Save(Str.Money, money);
                return true;
            }

            return false;
        }
    }
}