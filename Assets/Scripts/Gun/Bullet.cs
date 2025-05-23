using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bullet : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int posNum;
    public int Id => _id;
    int _id;

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
        _text = GetComponentInChildren<TMP_Text>();
        _image = GetComponentInChildren<Image>();

        _canvasGroup = GetComponent<CanvasGroup>();
        _dragPrefab = Resources.Load<GameObject>("Prefabs/UI_Bullet");

        Managers.EquipManager.OnSetRandomBulletEvent += HandleSetBullet;
    }

    void OnDestroy()
    {
        Managers.EquipManager.OnSetRandomBulletEvent -= HandleSetBullet;
    }

    void HandleSetBullet(int pos, int id)
    {
        if (posNum == pos)
        {
            _id = id;
            UpdateVisual();
        }
    }

    void UpdateVisual()
    {
        _canvasGroup.alpha = 1f;
        BulletData bullet = Managers.BulletManager.GetBullet(_id);
        _text.text = bullet.title;
        _image.color = bullet.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_id == -1) return;

        Managers.EquipManager.OnBulletInfoEvent?.Invoke(_id, 0, true);
        Managers.EquipManager.OnBulletLevelInfoEvent?.Invoke(_id, 0, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_id == -1) return;

        Managers.EquipManager.OnBulletInfoEvent?.Invoke(_id, 0, false);
        Managers.EquipManager.OnBulletLevelInfoEvent?.Invoke(_id, 0, false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.5f;
        _dragClone = Instantiate(_dragPrefab, null);
        _rootRect = _dragClone.GetComponent<RectTransform>();
        _dragRect = _dragClone.transform.GetChild(0).GetComponent<RectTransform>();
        _bulletUI = _dragClone.GetComponent<UI_Bullet>();
        _bulletUI.InitBullet(_id);
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
        _canvasGroup.alpha = 1f;
        Destroy(_dragClone);
    }

    public void OnDropInSlot()
    {
        _canvasGroup.alpha = 0f;
        Destroy(_dragClone);

        Managers.EquipManager.OnBulletSelectEvent?.Invoke(false);

        if (Managers.EquipManager.SelectBulletCount <= 7)
        {
            Managers.EquipManager.OnBulletSelectEvent?.Invoke(true);
        }
    }
}
