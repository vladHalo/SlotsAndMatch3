using System.Collections.Generic;
using _1Core.Scripts;
using Core.Scripts.Views;
using UnityEngine;

namespace Core.Scripts.Store
{
    public class StoreLogic : MonoBehaviour
    {
        public ActionButtonUpgradeManager ActionButtonUpgradeManager;
        [SerializeField] private MoneyManager _moneyManager;
        [SerializeField] private int _priceAdd;
        [SerializeField] private List<int> _prices;
        [SerializeField] private List<int> _indexStep;

        private GameManager gameManager;

        private void Start()
        {
            gameManager = GameManager.instance;
            ActionButtonUpgradeManager.AddListener(CheckMoney);

            for (int i = 0; i < _prices.Count; i++)
            {
                if (ES3.KeyExists($"{Str.Price}{i}"))
                    _indexStep[i] = ES3.Load<int>($"{Str.Price}{i}");
                ActionButtonUpgradeManager.ChangePriceText(i, _prices[i] * _indexStep[i]);
                gameManager.bonuses[i].index = _indexStep[i];
            }
        }

        public void CheckMoney(int index)
        {
            if (_moneyManager.CanMinus(_prices[index]))
            {
                _indexStep[index]++;
                _prices[index] += _priceAdd * _indexStep[index];
                ES3.Save($"{Str.Price}{index}", _indexStep[index]);
                ActionButtonUpgradeManager.NextStep(index);
                ActionButtonUpgradeManager.ChangePriceText(index, _prices[index]);
                gameManager.bonuses[index].index = _indexStep[index];
            }
            else
            {
                _moneyManager.MoveTween();
                Debug.Log("Не вистачає грошей!");
            }
        }
    }
}