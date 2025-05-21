using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    public Action<bool> OnLeftClickEvent;

    InputSystem_Actions _inputActions;

    InputAction _leftClickAction;
    InputAction _rightClickAction;
    InputAction _mousePosAction;

    public Vector2 MousePos = Vector2.zero;  // 스크린 기준 마우스 위치

    public void Init()
    {
        _inputActions = new InputSystem_Actions();

        _leftClickAction = _inputActions.Player.LeftClick;
        _rightClickAction = _inputActions.Player.RightClick;
        _mousePosAction = _inputActions.Player.MousePos;

        _inputActions.Player.Enable();

        _leftClickAction.Enable();
        _rightClickAction.Enable();
        _mousePosAction.Enable();

        _leftClickAction.performed += OnLeftClick;
        _leftClickAction.canceled += OnLeftClick;
        _mousePosAction.performed += OnMousePos;
    }

    void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnLeftClickEvent?.Invoke(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            OnLeftClickEvent?.Invoke(false);
        }
    }

    void OnMousePos(InputAction.CallbackContext context)
    {
        MousePos = context.ReadValue<Vector2>();
    }

    public void Clear()
    {
        _leftClickAction.Disable();
        _rightClickAction.Disable();
        _mousePosAction.Disable();

        _inputActions.Player.Disable();

        _leftClickAction.performed -= OnLeftClick;
        _leftClickAction.canceled -= OnLeftClick;
        _mousePosAction.performed -= OnMousePos;
    }
}
