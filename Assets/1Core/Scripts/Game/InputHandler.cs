using System.Linq;
using UnityEngine;
using DG.Tweening;

public class InputHandler : MonoBehaviour
{
    private bool _isPressed;
    private Slot _selectedSlot;
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
                _gameManager.blockManager.Crash(hit.collider.gameObject);
                return;
            }

            _selectedSlot = _gameManager.slotsManager.slots
                .FirstOrDefault(x =>
                    x.box.gameObject == hit.collider.gameObject &&
                    _gameManager.taskSubjectsManager.selectedTypeItems.Contains(x.typeItem));

            if (_selectedSlot != null)
            {
                _gameManager.taskSubjectsManager.MinusTask(_selectedSlot.typeItem);
                _selectedSlot.box.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutBounce);
            }
        }
    }

    private void HandleButtonUp()
    {
        _isPressed = false;
    }
}