using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float rotSpeed = 200f;
    public float inputDelay = 0.1f;

    float mx;
    float my;
    float inputLockTimer;

    bool _available = false;
    int _bulletIdx = -1;
    int _fireCount = 0;
    Coroutine _checkCoroutine;
    LayerMask _enemyLayer = 1 << 6;

    [Header("Recoil Settings")]
    public AnimationCurve recoilCurve = AnimationCurve.EaseInOut(0, 0, 0.2f, 0);
    public float recoilDuration = 0.2f;
    public float recoilAmount = 5f;

    float recoilTimer = 0f;
    bool isRecoiling = false;

    void Start()
    {
        inputLockTimer = inputDelay;

        Managers.GameManager.OnEnterShootEvent += CanShoot;
        Managers.GameManager.OnEnterEquipEvent += CantShoot;
        Managers.Battlemanager.OnFirstBulletEvent += SetFirstBullet;
        Managers.Battlemanager.OnFireEvent += GunFire;
    }

    void OnDestroy()
    {
        Managers.GameManager.OnEnterShootEvent -= CanShoot;
        Managers.GameManager.OnEnterEquipEvent -= CantShoot;
        Managers.Battlemanager.OnFirstBulletEvent -= SetFirstBullet;
        Managers.Battlemanager.OnFireEvent -= GunFire;
    }

    void Update()
    {
        if (_available && Input.GetMouseButtonDown(0))
        {
            if (_fireCount > 7 || _bulletIdx == -1) return;
            Managers.Battlemanager.Fire(_bulletIdx);
        }

        if (_available)
            Rotate();
    }

    void SetFirstBullet(int bulletIdx)
    {
        _bulletIdx = bulletIdx;
    }

    void GunFire(int bulletIdx)
    {
        int level = Mathf.Clamp(Managers.EquipManager.slotLevel[_bulletIdx], 0, Managers.EquipManager.bullets[_bulletIdx].damages.Count - 1);
        float damage = Managers.EquipManager.bullets[_bulletIdx].damages[level];

        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2f, Screen.height / 2f));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1.0f);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, _enemyLayer))
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            Managers.GameManager.Shoot(enemy, damage);
            Debug.Log($"[Hit] {Managers.EquipManager.bullets[_bulletIdx].title}, DMG : {damage}");
        }
        else
        {
            Debug.Log($"[Miss] {Managers.EquipManager.bullets[_bulletIdx].title}, DMG : {damage}");
        }

        StartRecoil();

        _bulletIdx = (_bulletIdx + 1) % 8;
        _fireCount++;
        /*if (_fireCount > 7 && _checkCoroutine == null)
            _checkCoroutine = StartCoroutine(Wait());*/
    }

    void StartRecoil()
    {
        isRecoiling = true;
        recoilTimer = 0f;
    }

    /*IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        _checkCoroutine = null;
        Managers.GameManager.CheckRestart();
    }*/

    void CanShoot()
    {
        _available = true;
    }
    void CantShoot()
    {
        _available = false;
        _bulletIdx = -1;
        _fireCount = 0;
    }

    void Rotate()
    {
        if (inputLockTimer > 0f)
        {
            inputLockTimer -= Time.deltaTime;
            return;
        }

        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");

        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90, 90);

        float recoilOffset = 0f;
        if (isRecoiling)
        {
            recoilTimer += Time.deltaTime;
            float t = recoilTimer / recoilDuration;
            recoilOffset = recoilCurve.Evaluate(t) * recoilAmount;

            if (t >= 1f)
            {
                isRecoiling = false;
                recoilTimer = 0f;
            }
        }

        float totalPitch = my + recoilOffset;
        transform.eulerAngles = new Vector3(-totalPitch, mx, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Managers.GameManager.GameOver();
        }
    }
}
