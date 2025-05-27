using UnityEngine;
using static Define;

public class Item
{
    public int Price;
    public int Id;
    public string Name;

    public virtual float GetPrice() { return Price; }
    public virtual float ExecuteOnShoot(ItemContext context) { return 0f; }
}
