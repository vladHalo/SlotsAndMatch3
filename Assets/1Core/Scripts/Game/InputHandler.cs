using System.Linq;
using _1Core.Scripts.Game;
using UnityEngine;
using DG.Tweening;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private MoveSpriteToUI _moveSpriteToUI;
    [SerializeField] private BonusManager _bonusManager;

    private bool _isPressed;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (_gameManager.gameStatus == GameStatus.Stop) return;

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            HandleButtonDown();
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            HandleButtonUp();
        }
    }

    private void HandleButtonDown()
    {
        Vector3 clickPosition = Input.mousePosition;
        if (Input.touchCount > 0)
        {
            clickPosition = Input.GetTouch(0).position;
        }

        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(clickPosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider)
        {
            _isPressed = true;

            if (hit.collider.gameObject.layer == 3)
            {
                AudioManager.instance.PlaySoundEffect(SoundType.Crash);
                _gameManager.blockManager.Crash(hit.collider.gameObject);
                return;
            }

            foreach (var slot in _gameManager.slotsManager.enableSlots)
            {
                if (slot.box.gameObject != hit.collider.gameObject) continue;
                switch (slot.typeItem)
                {
                    case TypeItem.Heart:
                        _gameManager.statsManager.AddStats(StatsType.Hearts, 1);
                        DisableBox(slot.box);
                        AudioManager.instance.PlaySoundEffect(SoundType.Bonus);
                        _moveSpriteToUI.StartMove(slot.box, TypeItem.Heart);
                        break;
                    case TypeItem.Coin:
                        _gameManager.moneyManager.AddMoney(20 * _gameManager.bonuses[(int)TypeBonus.Gold].index);
                        DisableBox(slot.box);
                        AudioManager.instance.PlaySoundEffect(SoundType.Bonus);
                        _moveSpriteToUI.StartMove(slot.box, TypeItem.Coin);
                        break;
                    case TypeItem.Star:
                        _gameManager.statsManager.AddStats(StatsType.Stars,
                            _gameManager.bonuses[(int)TypeBonus.Star].index);
                        DisableBox(slot.box);
                        AudioManager.instance.PlaySoundEffect(SoundType.Bonus);
                        _moveSpriteToUI.StartMove(slot.box, TypeItem.Star);
                        break;
                    case TypeItem.Time:
                        _gameManager.timerGame.AddTime(2 * _gameManager.bonuses[(int)TypeBonus.Time].index);
                        DisableBox(slot.box);
                        AudioManager.instance.PlaySoundEffect(SoundType.Bonus);
                        _moveSpriteToUI.StartMove(slot.box, TypeItem.Time);
                        break;
                    default:
                        if (_gameManager.taskSubjectsManager.selectedTypeItems.Contains(slot.typeItem))
                        {
                            AudioManager.instance.PlaySoundEffect(SoundType.Point);
                            _gameManager.taskSubjectsManager.MinusTask(slot.typeItem);
                            _bonusManager.StartBonus();
                            DisableBox(slot.box);
                        }

                        break;
                }

                break;
            }
        }
    }

    private void DisableBox(Transform box)
    {
        box.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBounce)
            .OnComplete(() => box.gameObject.SetActive(false));
    }

    private void HandleButtonUp()
    {
        _isPressed = false;
    }
}