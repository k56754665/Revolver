using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    bool _available = false;
    int _bulletIdx = 0;
    Coroutine _checkCoroutine;

    void Start()
    {
        Managers.GameManager.OnEnterShootEvent += CanShoot;
        Managers.GameManager.OnEnterEquipEvent += CantShoot;
    }

    private void OnDestroy()
    {
        Managers.GameManager.OnEnterShootEvent -= CanShoot;
        Managers.GameManager.OnEnterEquipEvent -= CantShoot;
    }

    void Update()
    {
        if (_available)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
        }
    }

    void Fire()
    {
        if (_bulletIdx > 7)
        {
            return;
        }

        int level = Mathf.Clamp(Managers.EquipManager.slotLevel[_bulletIdx], 0, Managers.EquipManager.bullets[_bulletIdx].damages.Count - 1);
        float damage = Managers.EquipManager.bullets[_bulletIdx].damages[level];
        Debug.Log($"{Managers.EquipManager.bullets[_bulletIdx].title}, DMG : {damage}");
        Managers.GameManager.Shoot(damage);
        _bulletIdx++;
        if (_bulletIdx > 7)
        {
            _checkCoroutine = StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        _checkCoroutine = null;
        Managers.GameManager.CheckRestart();
    }

    void CanShoot()
    {
        _available = true;
    }

    void CantShoot()
    {
        _available = false;
    }
}
