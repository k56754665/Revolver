using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public event Action OnEnterEquipEvent;
    public event Action OnEnterShootEvent;

    public event Action<float> OnDamageEvent;

    public event Action OnGameOverEvent;
    public event Action OnGameClearEvent;

    public void Init()
    {

    }

    public void StartEquip()
    {
        OnEnterEquipEvent?.Invoke();
        Managers.EquipManager.OnBulletSelectEvent?.Invoke(true);
    }

    public void StartShoot()
    {
        OnEnterShootEvent?.Invoke();
    }

    public void Shoot(float damage)
    {
        // TODO : 레이캐스트로 받아옴
        Enemy enemy = GameObject.FindAnyObjectByType<Enemy>();
        if (enemy != null)
            enemy.TakeDamage(damage);
        OnDamageEvent?.Invoke(damage);
    }

    public void CheckRestart()
    {
        // Enemy가 존재하면 게임 오버
        Enemy enemy = GameObject.FindAnyObjectByType<Enemy>();
        if (enemy != null)
        {
            OnGameOverEvent?.Invoke();
            Debug.Log("GameOver");
        }
        else
        {
            OnGameClearEvent?.Invoke();
            Debug.Log("GameClear");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
