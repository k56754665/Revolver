using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UI_BulletInventory : MonoBehaviour
{
    Canvas _canvas;
    GameObject _buttonObject;
    Button _button;

    [SerializeField] Image _background;
    [SerializeField] TMP_Text _headerText;
    [SerializeField] RectTransform _bulletInven;

    Tween _rotateTween;
    Tween _moveTween;

    Vector3 _initPosition = Vector3.zero;
    Vector3 _targetPosition = new Vector3(1009f, 0, 0);

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
        _button = GetComponentInChildren<Button>();
        _buttonObject = _button.gameObject;
        _buttonObject.SetActive(false);
        _button.onClick.AddListener(StartRotate);

        Managers.GameManager.OnEnterEquipEvent += Show;
        Managers.GameManager.OnEnterShootEvent += Hide;
        Managers.EquipManager.OnBulletSelectEvent += HandleBattleButton;
        Managers.Battlemanager.OnFirstBulletEvent += RotateGun;
        Managers.Battlemanager.OnFireEvent += RotateByFire;
    }

    void OnDestroy()
    {
        Managers.GameManager.OnEnterEquipEvent -= Show;
        Managers.GameManager.OnEnterShootEvent -= Hide;
        Managers.EquipManager.OnBulletSelectEvent -= HandleBattleButton;
        Managers.Battlemanager.OnFirstBulletEvent -= RotateGun;
        Managers.Battlemanager.OnFireEvent -= RotateByFire;
    }

    public void StartRotate()
    {
        _background.enabled = false;
        _headerText.enabled = false;
        _buttonObject.SetActive(false);
        _bulletInven.rotation = Quaternion.identity;

        Managers.Battlemanager.GetFirstBulletIdx();
    }

    void RotateGun(int targetIdx)
    {
        float anglePerSlot = 360f / 8f;
        float targetOffsetAngle = anglePerSlot * targetIdx;

        float fullRotation = 360f * 5f;
        float totalRotation = fullRotation + targetOffsetAngle;

        _rotateTween = _bulletInven
            .DOLocalRotate(new Vector3(0, 0, totalRotation), 3f, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutSine)
            .OnComplete(OnRotationComplete);

        _moveTween = _bulletInven
            .DOLocalMove(_targetPosition, 2.5f)
            .SetEase(Ease.InOutSine);
    }

    void RotateByFire(int bulletIdx)
    {
        float anglePerSlot = 360f / 8f;
        float currentZ = _bulletInven.localEulerAngles.z;
        float targetOffsetAngle = currentZ + anglePerSlot;

        _bulletInven
            .DOLocalRotate(new Vector3(0, 0, targetOffsetAngle), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutSine);
    }

    void Show()
    {
        _bulletInven.anchoredPosition = _initPosition;
        _background.enabled = true;
        _headerText.enabled = true;
        _buttonObject.SetActive(true);
        _canvas.enabled = true;
    }

    void Hide()
    {
        _background.enabled = false;
        _headerText.enabled = false;
        _buttonObject.SetActive(false);
    }

    void OnRotationComplete()
    {
        Managers.GameManager.StartShoot();
    }


    void HandleBattleButton(bool isVisible)
    {
        if (!isVisible && (Managers.EquipManager.SelectBulletCount >= 8))
        {
            _buttonObject.SetActive(true);
        }
    }
}
