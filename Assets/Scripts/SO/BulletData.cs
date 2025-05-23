using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "Bullets/Bullet")]
public class BulletData : ScriptableObject
{
    public int Id;              // 아이디
    public string title;        // 이름
    public string description;  // 설명
    public List<int> upgrades;  // 나부터 시작하는 시계 방향 석판 업그레이드. 8칸
    public List<float> damages; // 레벨에 따른 데미지
    public Color color;
    //public Sprite bulletIcon;   // 아이콘
}
