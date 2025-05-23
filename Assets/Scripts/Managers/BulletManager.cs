using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    Dictionary<int, BulletData> _bulletDict = new();

    public void Init()
    {
        LoadAllBullets();
    }

    void LoadAllBullets()
    {
        BulletData[] bullets = Resources.LoadAll<BulletData>("SO/Bullets");

        foreach (var bullet in bullets)
        {
            if (!_bulletDict.ContainsKey(bullet.Id))
            {
                _bulletDict.Add(bullet.Id, bullet);
            }
            else
            {
                //Debug.LogWarning($"중복된 Bullet ID 감지: {bullet.Id}, 이름: {bullet.name}");
            }
        }

        //Debug.Log($"총 {_bulletDict.Count}개의 Bullet SO 등록 완료");
    }

    public List<BulletData> GetRandomBullets(int count = 3)
    {
        if (_bulletDict.Count == 0)
        {
            Debug.LogWarning("총알 데이터가 없음");
            return new List<BulletData>();
        }

        List<BulletData> bulletList = new List<BulletData>(_bulletDict.Values);
        List<BulletData> result = new();

        for (int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, bulletList.Count);
            result.Add(bulletList[randIndex]); // 중복 허용
        }

        return result;
    }


    public BulletData GetBullet(int id)
    {
        _bulletDict.TryGetValue(id, out var bullet);
        return bullet;
    }

    public void RegisterBullet(int id, BulletData instance)
    {
        if (!_bulletDict.ContainsKey(id))
            _bulletDict.Add(id, instance);
    }

    public void UnregisterBullet(int id)
    {
        if (_bulletDict.ContainsKey(id))
            _bulletDict.Remove(id);
    }
}
