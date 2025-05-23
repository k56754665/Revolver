using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<float> OnHpChangeEvent;

    public float MaxHp => _maxHp;
    public float Hp => _hp;

    [SerializeField] float _maxHp = 100f;
    float _hp = 100f;

    Coroutine _dieCoroutine;

    void Start()
    {
        _hp = _maxHp;
    }

    public void TakeDamage(float damage)
    {
        _hp = Mathf.Clamp(_hp - damage, 0, _maxHp);
        OnHpChangeEvent?.Invoke(_hp);
        Die();
    }

    void Die()
    {
        if ((int)_hp <= 0)
        {
            if (_dieCoroutine == null)
                StartCoroutine(DieCoroutine());
        }
    }

    IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
        _dieCoroutine = null;
        //Managers.GameManager.Restart();
    }
}
