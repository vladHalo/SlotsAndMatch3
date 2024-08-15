using System.Collections.Generic;
using _1Core.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SelectAndBuyDesign : MonoBehaviour
{
    [SerializeField] private BlockManager _blockManager;
    [SerializeField] private MoneyManager _moneyManager;
    [SerializeField] private Button[] _btnBuy, _btnSelect;
    [SerializeField] private Text[] _textSelect;
    [SerializeField] private List<int> _prices;

    private int _selectedBtn;

    private void Start()
    {
        if (ES3.KeyExists($"{Str.SelectedDesign}"))
            _selectedBtn = ES3.Load<int>(Str.SelectedDesign);

        SelectedDesign(_selectedBtn);

        for (int i = 0; i < _btnBuy.Length; i++)
        {
            if (ES3.KeyExists($"{Str.IsBuy}{i}"))
                _btnBuy[i].gameObject.SetActive(false);
        }
    }

    public void CheckMoney(int index)
    {
        if (_moneyManager.CanMinus(_prices[index]))
        {
            ES3.Save($"{Str.IsBuy}{index}", true);
            _btnBuy[index].gameObject.SetActive(false);
        }
        else
        {
            _moneyManager.MoveTween();
            Debug.Log("Не вистачає грошей!");
        }
    }

    public void SelectedDesign(int index)
    {
        for (int i = 0; i < _textSelect.Length; i++)
        {
            _textSelect[i].color = Color.white;
            _textSelect[i].text = "Select";
            _btnSelect[i].interactable = true;
        }

        _textSelect[index].color = Color.green;
        _textSelect[index].text = "Selected";
        _btnSelect[index].interactable = false;
        _selectedBtn = index;
        ES3.Save($"{Str.SelectedDesign}", _selectedBtn);
        _blockManager.SetDesign(_selectedBtn);
    }
}