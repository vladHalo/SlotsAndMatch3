using System;
using Core.Scripts.Tools.Tools;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _1Core.Scripts
{
    public class MoneyManager : MonoBehaviour
    {
        public float money;

        [SerializeField] private Leadboard _leadboard;

        [SerializeField] private float _bestWinMoney;
        [SerializeField] private TextMeshProUGUI _moneyTextWin, _moneyTextLose, _moneyBestTextWin;
        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Transform _panelMoneyTexts;

        private bool _canMoveTween;
        private float _winPrize;
        private GameManager _gameManager;

        private void OnValidate()
        {
            _moneyText.text = $"{money}";
        }

        private void Start()
        {
            _gameManager = GameManager.instance;

            _canMoveTween = true;
            if (ES3.KeyExists(Str.Money))
            {
                money = ES3.Load<float>(Str.Money);
            }

            _moneyText.text = money.ToIdleFormat(2, 0);

            if (ES3.KeyExists(Str.BestWinMoney))
            {
                _bestWinMoney = ES3.Load<float>(Str.BestWinMoney);
                _moneyBestTextWin.text = $"Best Score: {_bestWinMoney.ToIdleFormat(2, 0)}";
            }
        }

        public void ShowWin()
        {
            
        }

        public void ShowLose()
        {
            
        }

        public void AddMoney()
        {
            money += _winPrize;
            money = (float)Math.Round(money, 2);
            _moneyText.text = money.ToIdleFormat(2, 0);
            ES3.Save(Str.Money, money);
        }

        public void MoveTween()
        {
            if (!_canMoveTween) return;
            _canMoveTween = false;
            _moneyText.color = new Color(0.4f, 0, 0);
            _panelMoneyTexts.DOScale(new Vector3(1.3f, 1.3f, 1.3f), .4f)
                .OnComplete(() =>
                    {
                        _panelMoneyTexts.DOScale(Vector3.one, .4f).OnComplete(() => _canMoveTween = true);
                        _moneyText.color = Color.white;
                    }
                );
        }

        public bool CanMinus(int count)
        {
            if (money >= count)
            {
                money -= count;
                money = (float)Math.Round(money, 2);
                _moneyText.text = money.ToIdleFormat(2, 0);
                ES3.Save(Str.Money, money);
                return true;
            }

            return false;
        }
    }
}