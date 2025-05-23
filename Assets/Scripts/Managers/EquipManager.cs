using System;
using UnityEngine;

public class EquipManager
{
    /// <summary>
    /// 1번째 칸에, 2번째 id 총알 넣기(총알 선택 ui)
    /// </summary>
    public Action<int, int> OnSetRandomBulletEvent;

    /// <summary>
    /// id, level, isVisible
    /// </summary>
    public Action<int, int, bool> OnBulletInfoEvent;

    /// <summary>
    /// id, level, isVisible
    /// </summary>
    public Action<int, int, bool> OnBulletLevelInfoEvent;

    /// <summary>
    /// 현재 탄창의 총알 내용입니다. 12시부터 시계방향 순서
    /// </summary>
    public BulletData[] bullets = new BulletData[8];

    /// <summary>
    /// 탄창의 레벨입니다. 12시부터 시계방향 순서
    /// </summary>
    public int[] slotLevel = new int[8];

    public Action OnSlotLevelEvent;

    /// <summary>
    /// 총 8번 선택하게 됨
    /// </summary>
    public int SelectBulletCount;

    /// <summary>
    /// UI_BulletSelect 표시/끄기 (isVisible)
    /// </summary>
    public Action<bool> OnBulletSelectEvent;

    /// <summary>
    /// 1번째 칸에, 2번째 id 총알 넣기(탄창 내부)
    /// </summary>
    public Action<int, int> OnSetBulletInSlot;

    public void Init()
    {
        SelectBulletCount = 0;
        for (int i = 0; i < 8; i++)
        {
            slotLevel[i] = 0;
            bullets[i] = null;
        }
    }

    public void SetBulletInSlot(int slotNum, int id)
    {
        OnSetBulletInSlot?.Invoke(slotNum, id);

        if (id == -1)
        {
            bullets[slotNum] = null;
        }
        else
        {
            bullets[slotNum] = Managers.BulletManager.GetBullet(id);
        }

        CalculateLevel();

        OnSlotLevelEvent?.Invoke();
    }

    void CalculateLevel()
    {
        for (int i = 0; i < 8; i++)
        {
            slotLevel[i] = 0;
        }

        // 전체 bullets 순회하면서, 각각 한바퀴씩 더해주기

        for (int i = 0; i < 8; i++)
        {
            if (bullets[i] == null)
                continue;

            int slotIdx = i;

            for (int j = 0; j < 8; j++)
            {
                slotLevel[slotIdx] += bullets[i].upgrades[j];
                slotIdx = (slotIdx + 1) % 8;
            }
        }
    }
}
