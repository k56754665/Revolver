using static Define;

public class SameComboItem : Item
{
    float multiplier = 1.5f;

    public SameComboItem()
    {
        Id = 0;
        Price = 5;
        Name = "SameCombo";
    }

    public override float ExecuteOnShoot(ItemContext context)
    {
        int chain = 0;

        int id = context.bullets[context.bulletIdx].Id; // 현재 총알 아이디 구하기

        for (int i = context.bulletIdx; i >= 0; i--)
        {
            if (id == context.bullets[i].Id)
            {
                chain++;
            }
            else
            {
                break;
            }
        }

        return (chain * multiplier);
    }
}
