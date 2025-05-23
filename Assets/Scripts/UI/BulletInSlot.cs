using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BulletInSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int SlotNum;
    public int Id => _id;
    int _id = -1;

    TMP_Text _text;
    Image _image;
    CanvasGroup _canvasGroup;
    GameObject _dragPrefab;

    GameObject _dragClone;
    RectTransform _dragRect;
    RectTransform _rootRect;
    UI_Bullet _bulletUI;

    void Start()
    {
        _dragPrefab = Resources.Load<GameObject>("Prefabs/UI_Bullet");
        _text = GetComponentInChildren<TMP_Text>();
        _image = GetComponentInChildren<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;

        Managers.EquipManager.OnSetBulletInSlot += HandleSetBulletInSlot;
    }

    void OnDestroy()
    {
        Managers.EquipManager.OnSetBulletInSlot -= HandleSetBulletInSlot;
    }

    void HandleSetBulletInSlot(int slot, int id)
    {
        if (slot != SlotNum) return;

        _id = id;
        Show();
    }

    void Show()
    {
        if (_id != -1)
        {
            BulletData bullet = Managers.BulletManager.GetBullet(_id);
            _text.text = bullet.title;
            _image.color = bullet.color;
            _canvasGroup.alpha = 1f;
        }
        else
        {
            _canvasGroup.alpha = 0f;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_id == -1) return;

        Managers.EquipManager.OnBulletInfoEvent?.Invoke(_id, Managers.EquipManager.slotLevel[SlotNum], true);
        Managers.EquipManager.OnBulletLevelInfoEvent?.Invoke(_id, Managers.EquipManager.slotLevel[SlotNum], true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_id == -1) return;

        Managers.EquipManager.OnBulletInfoEvent?.Invoke(_id, Managers.EquipManager.slotLevel[SlotNum], false);
        Managers.EquipManager.OnBulletLevelInfoEvent?.Invoke(_id, Managers.EquipManager.slotLevel[SlotNum], false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_id != -1)
        {
            _canvasGroup.alpha = 0.5f;
            _dragClone = Instantiate(_dragPrefab, null);
            _rootRect = _dragClone.GetComponent<RectTransform>();
            _dragRect = _dragClone.transform.GetChild(0).GetComponent<RectTransform>();
            _bulletUI = _dragClone.GetComponent<UI_Bullet>();
            _bulletUI.InitBullet(_id);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localMousePos;

        if (_rootRect == null) return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootRect,
            Managers.InputManager.MousePos,
            null,
            out localMousePos
        );

        _dragRect.anchoredPosition = localMousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_id != -1)
        {
            _canvasGroup.alpha = 1f;
        }
        Destroy(_dragClone);
    }
}
