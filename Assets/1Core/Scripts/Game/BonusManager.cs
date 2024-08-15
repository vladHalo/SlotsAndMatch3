using Core.Scripts.Tools.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private MoveSpriteToUI _moveSpriteToUI;
    [SerializeField] private int _multipleIndex = 5;
    [SerializeField] private float _startTime;
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    private int _index;
    private float _time;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    [Button]
    public void StartBonus()
    {
        _time = _startTime;
        _image.gameObject.SetActive(true);
        _index++;
        _text.text = $"{_index}X";
        if (_index % _multipleIndex == 0)
        {
            var indexAdd = _index / 5;
            bool rand = new[] { true, false }.Random();
            if (rand)
            {
                _gameManager.moneyManager.AddMoney(indexAdd * 10); //Set count money
                _moveSpriteToUI.StartMove(_image.transform, TypeItem.Coin);
            }
            else
            {
                _gameManager.timerGame.AddTime(indexAdd);
                _moveSpriteToUI.StartMove(_image.transform, TypeItem.Time);
            }
        }
    }

    private void Update()
    {
        if (_image.gameObject.activeSelf)
        {
            _time -= Time.deltaTime;
            _image.fillAmount = _time / _startTime;
            if (_time <= 0)
            {
                _image.gameObject.SetActive(false);
                _index = 0;
            }
        }
    }
}