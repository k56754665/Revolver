using System.Collections.Generic;
using UnityEngine;

public class ItemManager
{
    Dictionary<int, Item> _itemDic = new();

    List<int> OwnedItems = new();

    public void Init()
    {
        LoadAllItems();
        OwnedItems.Clear();
    }

    void LoadAllItems()
    {
        SameComboItem sameComboItem = new SameComboItem();
        _itemDic[sameComboItem.Id] = sameComboItem;
    }

    public List<Item> GetRandomItems(int count = 3)
    {
        if (_itemDic.Count == 0)
        {
            Debug.LogWarning("아이템 데이터가 없음");
            return new List<Item>();
        }

        List<Item> itemList = new List<Item>(_itemDic.Values);
        List<Item> result = new();

        for (int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, itemList.Count);
            result.Add(itemList[randIndex]); // 중복 허용
        }

        return result;
    }

    public void OwnItem(int id)
    {
        OwnedItems.Add(id);
    }

    public Item GetItem(int id)
    {
        _itemDic.TryGetValue(id, out var item);
        return item;
    }
}
