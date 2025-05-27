using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public event Action<float> OnHpChangeEvent;

    public float MaxHp => _maxHp;
    public float Hp => _hp;

    float _maxHp;
    float _hp;
    float _moveSpeed;
    bool _isMoving = false;

    Player _player;
    Text_EnemyHp _enemyText;
    Coroutine _dieCoroutine;

    public void Init(float maxHp, float moveSpeed)
    {
        _maxHp = maxHp;
        _hp = maxHp;
        _moveSpeed = moveSpeed;
        _enemyText = GetComponentInChildren<Text_EnemyHp>();
        _enemyText.Init();
        _player = FindAnyObjectByType<Player>();
        if (_player != null)
            Managers.GameManager.OnEnterShootEvent += StartMove;
    }

    void Update()
    {
        if (_player == null || !_isMoving)
            return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            _player.transform.position,
            _moveSpeed * Time.deltaTime
        );
    }

    void StartMove()
    {
        _isMoving = true;
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
        _dieCoroutine = null;
        Managers.GameManager.EnemyDie();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Managers.GameManager.OnEnterShootEvent -= StartMove;
    }
}
