using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager
{
    public int StageIdx => _stageIdx;
    int _stageIdx = 0;

    GameObject _enemyPrefab;

    float _xPosMax = 8f;
    float _xPosMin = -8f;
    float _zPosMax = 9f;
    float _zPosMin = 7f;

    public int EnemyNum = 0;

    public void Init()
    {
        _enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        Managers.GameManager.OnEnterEquipEvent += NextStage;
        Managers.GameManager.OnEnemyDieEvent += CheckEnemyDead;
    }

    public void Clear()
    {
        Managers.GameManager.OnEnterEquipEvent -= NextStage;
        Managers.GameManager.OnEnemyDieEvent -= CheckEnemyDead;
    }

    public void NextStage()
    {
        _stageIdx++;
        Debug.Log($"Stage {_stageIdx} 시작");
        if (_stageIdx == 1) // 첫 스테이지는 적 1마리
        {
            EnemyNum = 1;
            Enemy enemy = SpawnEnemy();
            enemy.Init(10f, 1f);
        }
        else if (_stageIdx % 4 == 0) // 보스전
        {
            EnemyNum = 1;
            Enemy boss = SpawnEnemy();
            float bossHp = 50 + 10 * (_stageIdx / 4);  // 보스 체력 증가
            float bossSpeed = 1f + _stageIdx / 4f;       // 보스 속도도 소폭 증가
            boss.Init(bossHp, bossSpeed);
        }
        else // 일반 스테이지
        {
            EnemyNum = Random.Range(1, 4); // 적 수 1 ~ 3
            float totalHp = _stageIdx * 15; // 스테이지에 비례한 총 체력
            float remainingHp = totalHp;

            float baseSpeed = 1f + (_stageIdx * 0.3f); // 스테이지에 비례한 기본 스피드

            for (int i = 0; i < EnemyNum; i++)
            {
                Enemy enemy = SpawnEnemy();

                // 마지막 적은 남은 HP 전부 할당
                float enemyHp;
                if (i == EnemyNum - 1)
                {
                    enemyHp = remainingHp;
                }
                else
                {
                    // 체력 편차 적용 (±20%)
                    float avgHp = (float)remainingHp / (EnemyNum - i);
                    float variance = Random.Range(0.8f, 1.2f);
                    enemyHp = Mathf.RoundToInt(avgHp * variance);
                    remainingHp -= enemyHp;
                }

                // 속도는 ±10% 랜덤 편차 적용
                float speedVariance = Random.Range(0.9f, 1.1f);
                float enemySpeed = baseSpeed * speedVariance;

                enemy.Init(enemyHp, enemySpeed);
            }
        }

        //Managers.GameManager.StartEquip();
    }

    Enemy SpawnEnemy()
    {
        float xPos = Random.Range(_xPosMin, _xPosMax);
        float zPos = Random.Range(_zPosMin, _zPosMax);
        GameObject go = GameObject.Instantiate(_enemyPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        return enemy; 
    }

    public void CheckEnemyDead()
    {
        if (EnemyNum <= 0)
        {
            Debug.Log($"Stage {_stageIdx} 완료 → 상점으로");
            Managers.GameManager.StartShop();
            // TODO : 상점 열기
            //Managers.GameManager.OpenShop();
        }
    }
}
