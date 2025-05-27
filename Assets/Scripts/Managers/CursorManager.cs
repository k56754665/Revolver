using UnityEngine;

public class CursorManager
{
    public void Init()
    {
        CursorOn();

        Managers.GameManager.OnEnterEquipEvent += CursorOn;
        Managers.GameManager.OnEnterShopEvent += CursorOn;
        Managers.GameManager.OnEnterShootEvent += CursorOff;
        Managers.GameManager.OnGameClearEvent += CursorOn;
        Managers.GameManager.OnGameOverEvent += CursorOn;
    }

    public void Clear()
    {
        Managers.GameManager.OnEnterEquipEvent -= CursorOn;
        Managers.GameManager.OnEnterShopEvent -= CursorOn;
        Managers.GameManager.OnEnterShootEvent -= CursorOff;
        Managers.GameManager.OnGameClearEvent -= CursorOn;
        Managers.GameManager.OnGameOverEvent -= CursorOn;
    }

    void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
