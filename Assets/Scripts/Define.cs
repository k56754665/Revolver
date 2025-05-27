public static class Define
{
    public enum TriggerType
    {
        OnBattleStart,
        OnShoot,
        OnReload,
        OnHit,
        OnKill,
        Always, // 항상 발동 (상시 패시브)
    }

    public class ItemContext
    {
        public Player player;
        public Enemy enemy;
        public Bullet currentBullet;
        public int bulletIdx;
        public int[] slotLevel;
        public BulletData[] bullets;
    }
}
