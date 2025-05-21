using UnityEngine;
using System.Collections.Generic;

public class UI_BulletInventory : MonoBehaviour
{
    [SerializeField] RectTransform _rect;
    [SerializeField] float _fastVelocity;
    [SerializeField] float _middleVelocity;
    [SerializeField] float _slowVelocity;
    [SerializeField] float _damping = 0.99f;

    float _angularVelocity = 0f;
    bool _isDragging = false;
    bool _isRotating = false;

    Vector2 _prevMousePos;
    bool _foundValidHistory = false;

    Queue<(float time, Vector2 pos)> _mouseHistory = new();

    void OnEnable()
    {
        Managers.InputManager.OnLeftClickEvent += HandleLeftClickEvent;
    }

    void OnDisable()
    {
        Managers.InputManager.OnLeftClickEvent -= HandleLeftClickEvent;
    }

    void Update()
    {
        Vector2 centerScreen = RectTransformUtility.WorldToScreenPoint(null, _rect.position);
        Vector2 currMousePos = Managers.InputManager.MousePos;
        float currentTime = Time.time;

        if (_isDragging)
        {
            float prevAngle = Mathf.Atan2(_prevMousePos.y - centerScreen.y, _prevMousePos.x - centerScreen.x) * Mathf.Rad2Deg;
            float currAngle = Mathf.Atan2(currMousePos.y - centerScreen.y, currMousePos.x - centerScreen.x) * Mathf.Rad2Deg;

            float deltaAngle = Mathf.DeltaAngle(prevAngle, currAngle);
            float adjustedDelta = deltaAngle * 0.1f;

            _rect.Rotate(0, 0, adjustedDelta);
            _angularVelocity = adjustedDelta / Time.deltaTime;

            _prevMousePos = currMousePos;

            // 0.1초 전 위치 추적용 저장
            _mouseHistory.Enqueue((currentTime, currMousePos));
            while (_mouseHistory.Count > 0 && currentTime - _mouseHistory.Peek().time > 0.2f)
                _mouseHistory.Dequeue();

            // 0.1초 전 위치 확보
            _foundValidHistory = false;
            float targetTime = currentTime - 0.1f;
            var historyArray = _mouseHistory.ToArray();
            for (int i = historyArray.Length - 1; i >= 0; i--)
            {
                if (historyArray[i].time <= targetTime)
                {
                    _prevMousePos = historyArray[i].pos;
                    _foundValidHistory = true;
                    break;
                }
            }
        }

        if (_isRotating)
        {
            if (Mathf.Abs(_angularVelocity) > 0.1f)
            {
                _rect.Rotate(0, 0, _angularVelocity * Time.deltaTime);
                _angularVelocity *= _damping;
            }
            else
            {
                _angularVelocity = 0f;
                _isRotating = false;
            }
        }
    }

    void HandleLeftClickEvent(bool isPressed)
    {
        if (isPressed)
        {
            _isDragging = true;
            _mouseHistory.Clear();
            _prevMousePos = Managers.InputManager.MousePos;
        }
        else
        {
            _isDragging = false;
            MakeRotation();
        }
    }

    void MakeRotation()
    {
        if (!_foundValidHistory)
        {
            Debug.Log("0.1초 전 위치 없음 → 회전 안 함");
            return;
        }

        Vector2 currentMousePos = Managers.InputManager.MousePos;
        float distance = Vector2.Distance(_prevMousePos, currentMousePos);

        if (distance >= 100f)
        {
            _angularVelocity = _fastVelocity;
            _isRotating = true;
            Debug.Log("빠름");
        }
        else if (distance >= 50f)
        {
            _angularVelocity = _middleVelocity;
            _isRotating = true;
            Debug.Log("중간");
        }
        else if (distance >= 1f)
        {
            _angularVelocity = _slowVelocity;
            _isRotating = true;
            Debug.Log("느림");
        }
    }
}
