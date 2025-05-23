using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BulletLevel : MonoBehaviour
{
    [SerializeField] RectTransform _rect;

    RectTransform _rootRect;
    Canvas _canvas;
    TMP_Text _text;
    Image _image;
    bool _isVisible = false;


    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _rootRect = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TMP_Text>();
        _image = GetComponentInChildren<Image>();
        _canvas.enabled = false;

        Managers.EquipManager.OnBulletLevelInfoEvent += HandleBulletInfoEvent;
    }

    void OnDestroy()
    {
        Managers.EquipManager.OnBulletLevelInfoEvent -= HandleBulletInfoEvent;
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
        StringBuilder sb = new StringBuilder();
        sb.Append($"{bullet.title}\n");
        sb.Append($"[Lv]\t데미지\n\n");
        for (int i = 0; i < bullet.damages.Count; i++)
        {
            sb.Append($"[{i}]\t{bullet.damages[i]}\n");
        }
        string result = sb.ToString();
        _text.text = result;

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
