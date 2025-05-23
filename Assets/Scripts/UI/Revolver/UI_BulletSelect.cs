using System.Collections.Generic;
using UnityEngine;

public class UI_BulletSelect : MonoBehaviour
{
    Canvas _canvas;

    void Start()
    {
        _canvas = GetComponent<Canvas>();

        Managers.EquipManager.OnBulletSelectEvent += HandleBulletSelect;
    }

    void OnDestroy()
    {
        Managers.EquipManager.OnBulletSelectEvent -= HandleBulletSelect;
    }

    void HandleBulletSelect(bool isVisible)
    {
        if (isVisible) Show();
        else Hide();
    }

    void SetBullets()
    {
        List<BulletData> bullets = Managers.BulletManager.GetRandomBullets();

        for (int i = 0; i < 3; i++)
        {
            Managers.EquipManager.OnSetRandomBulletEvent?.Invoke(i, bullets[i].Id);
        }

        Managers.EquipManager.SelectBulletCount += 1;
    }

    void Show()
    {
        SetBullets();
        _canvas.enabled = true;
    }

    void Hide()
    {
        _canvas.enabled = false;
    }
}
