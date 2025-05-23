using UnityEngine;
using UnityEngine.EventSystems;

public class BulletInventoryRotate : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    RectTransform _rectTransform;
    Vector2 _startDir;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 center = _rectTransform.position;
        _startDir = (eventData.position - center).normalized;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 center = _rectTransform.position;
        Vector2 currentDir = (eventData.position - center).normalized;

        float angle = Vector2.SignedAngle(_startDir, currentDir);
        _rectTransform.Rotate(0, 0, angle);

        _startDir = currentDir;
    }
}
