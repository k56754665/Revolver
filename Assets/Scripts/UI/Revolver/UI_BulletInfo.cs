using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UI_BulletInfo : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] TMP_Text _title;
    [SerializeField] List<TMP_Text> _levelDamage;
    [SerializeField] List<TMP_Text> _upgrades;

    RectTransform _rootRect;
    Canvas _canvas;
    bool _isVisible = false;

    Color _translucent = new Color(1, 1, 1, 0.2f);
    Color _transparent = new Color(0, 0, 0, 0);

    void Start()
    {
        _rootRect = GetComponent<RectTransform>();
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;

        Managers.EquipManager.OnBulletInfoEvent += HandleBulletInfoEvent;
    }

    void Update()
    {
        if (!_isVisible) return;

        Vector2 localMousePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _rootRect,
            Managers.InputManager.MousePos,
            null,
            out localMousePos
        );

        Vector2 clampedPos = ClampToScreen(localMousePos);
        _rect.anchoredPosition = clampedPos;
    }

    private void OnDestroy()
    {
        Managers.EquipManager.OnBulletInfoEvent -= HandleBulletInfoEvent;
    }

    void HandleBulletInfoEvent(int id, int level, bool isVisible)
    {
        _isVisible = isVisible;

        if (isVisible)
        {
            BulletData bullet = Managers.BulletManager.GetBullet(id);
            ShowBulletInfo(bullet, level);
        }
        else
        {
            HideBulletInfo();
        }
    }

    void ShowBulletInfo(BulletData bullet, int level)
    {
        _title.text = bullet.title;

        if (level <= 0)
        {
            int minLevel = Mathf.Max(level, 0);
            _levelDamage[0].text = $"[Lv {minLevel}] 데미지 {bullet.damages[minLevel]}";
            _levelDamage[1].text = $"[Lv {minLevel + 1}] 데미지 {bullet.damages[minLevel + 1]}";
            _levelDamage[2].text = $"[Lv {minLevel + 2}] 데미지 {bullet.damages[minLevel + 2]}";

            _levelDamage[0].color = Color.red;
            _levelDamage[1].color = _translucent; 
            _levelDamage[2].color = _transparent;
        }
        else if (level < bullet.damages.Count - 1)
        {
            _levelDamage[0].text = $"[Lv {level - 1}] 데미지 {bullet.damages[level - 1]}";
            _levelDamage[1].text = $"[Lv {level}] 데미지 {bullet.damages[level]}";
            _levelDamage[2].text = $"[Lv {level + 1}] 데미지 {bullet.damages[level + 1]}";

            _levelDamage[0].color = _translucent;
            _levelDamage[1].color = Color.red;
            _levelDamage[2].color = _translucent;
        }
        else
        {
            int maxLevel = Mathf.Min(level, bullet.damages.Count - 1);
            _levelDamage[0].text = $"[Lv {maxLevel - 1}] 데미지 {bullet.damages[maxLevel - 1]}";
            _levelDamage[1].text = $"[Lv {maxLevel}] 데미지 {bullet.damages[maxLevel]}";

            _levelDamage[0].color = _translucent;
            _levelDamage[1].color = Color.red;
            _levelDamage[2].color = _transparent;
        }

        for (int i = 0; i < _upgrades.Count; i++)
        {
            int upgrade = bullet.upgrades[i];
            _upgrades[i].text = $"{upgrade}";
            if (upgrade > 0)
            {
                _upgrades[i].color = Color.green;
            }
            else if (upgrade < 0)
            {
                _upgrades[i].color = Color.red;
            }
            else if (upgrade == 0)
            {
                _upgrades[i].color = _transparent;
            }
        }

        _canvas.enabled = true;
    }

    void HideBulletInfo()
    {
        _canvas.enabled = false;
    }

    Vector2 ClampToScreen(Vector2 pos)
    {
        Vector2 canvasSize = _rootRect.rect.size;
        Vector2 halfUI = _rect.rect.size * 0.5f;

        // 좌상단 (0,0) 기준으로, UI가 안 나가게 제한
        float minX = -canvasSize.x * 0.5f + halfUI.x;
        float maxX = canvasSize.x * 0.5f - halfUI.x;

        float minY = -canvasSize.y * 0.5f + halfUI.y;
        float maxY = canvasSize.y * 0.5f - halfUI.y;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        return pos;
    }

}
