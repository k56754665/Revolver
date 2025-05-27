using System;
using UnityEngine;

public class BattleManager
{
    /// <summary>
    /// 리볼버 돌려서 첫번째 총알이 무엇인지 결정해주는 액션
    /// </summary>
    public event Action<int> OnFirstBulletEvent;

    /// <summary>
    /// 몇 번째 슬롯의 총알을 쐈는지 보내주는 액션
    /// </summary>
    public event Action<int> OnFireEvent;

    public void GetFirstBulletIdx()
    {
        int randIdx = UnityEngine.Random.Range(0, 8); // 0~7
        OnFirstBulletEvent?.Invoke(randIdx);
    }
    
    public void Fire(int bulletIdx)
    {
        OnFireEvent?.Invoke(bulletIdx);
    }
}
