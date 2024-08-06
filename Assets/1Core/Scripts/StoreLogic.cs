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

                if (i == 0)
                {
                    //gameManager.firstPlace = .2f * (_indexStep[i] - 1);
                }
                else if (i == 1)
                {
                    //gameManager.multiplierX = .05f * (_indexStep[i] - 1);
                }
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
                if (index == 0)
                {
                    //gameManager.firstPlace = .2f * (_indexStep[index] - 1);
                }
                else if (index == 1)
                {
                    //gameManager.multiplierX = .05f * (_indexStep[index] - 1);
                }
            }
            else
            {
                _moneyManager.MoveTween();
                Debug.Log("Не вистачає грошей!");
            }
        }
    }
}