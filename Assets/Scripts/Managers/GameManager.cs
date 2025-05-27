using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public event Action OnEnterEquipEvent;
    public event Action OnEnterShopEvent;
    public event Action OnEnterShootEvent;

    public event Action<float> OnDamageEvent;

    public event Action OnGameOverEvent;
    public event Action OnGameClearEvent;

    public event Action OnEnemyDieEvent;

    public void Init()
    {

    }

    public void StartShop()
    {
        OnEnterShopEvent?.Invoke();
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

    public void Shoot(Enemy enemy, float damage)
    {
        if (enemy != null)
            enemy.TakeDamage(damage);
        OnDamageEvent?.Invoke(damage);
    }

    public void EnemyDie()
    {
        Managers.StageManager.EnemyNum--;
        OnEnemyDieEvent?.Invoke();
    }

    /*public void CheckRestart()
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
    }*/

    public void GameOver()
    {
        OnGameOverEvent?.Invoke();
        Debug.Log("GameOver");
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
