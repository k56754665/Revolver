using UnityEngine;
using UnityEngine.UI;

public class UI_BulletInventory : MonoBehaviour
{
    Canvas _canvas;
    GameObject _buttonObject;
    Button _button;

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _button = GetComponentInChildren<Button>();
        _buttonObject = _button.gameObject;
        _buttonObject.SetActive(false);
        _button.onClick.AddListener(BattleStart);

        Managers.GameManager.OnEnterEquipEvent += Show;
        Managers.GameManager.OnEnterShootEvent += Hide;
        Managers.EquipManager.OnBulletSelectEvent += HandleBattleButton;
    }

    void OnDestroy()
    {
        Managers.GameManager.OnEnterEquipEvent -= Show;
        Managers.GameManager.OnEnterShootEvent -= Hide;
        Managers.EquipManager.OnBulletSelectEvent -= HandleBattleButton;
    }

    void BattleStart()
    {
        Managers.GameManager.StartShoot();
    }

    void Show()
    {
        _canvas.enabled = true;
    }

    void Hide()
    {
        _canvas.enabled = false;
    }
    
    void HandleBattleButton(bool isVisible)
    {
        if (!isVisible && (Managers.EquipManager.SelectBulletCount >= 8))
        {
            _buttonObject.SetActive(true);
        }
    }
}
