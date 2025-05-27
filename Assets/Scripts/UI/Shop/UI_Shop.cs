using System.Collections.Generic;
using UnityEngine;

public class UI_Shop : MonoBehaviour
{
    Canvas _canvas;
    [SerializeField] Transform _itemSlots;
    GameObject _itemUIPrefab;

    void Start()
    {
        _itemUIPrefab = Resources.Load<GameObject>("Prefabs/UI_Item");
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        Managers.GameManager.OnEnterShopEvent += EnterShop;
        Managers.GameManager.OnEnterEquipEvent += ExitShop;
    }

    void OnDestroy()
    {
        Managers.GameManager.OnEnterShopEvent -= EnterShop;
        Managers.GameManager.OnEnterEquipEvent -= ExitShop;
    }

    void EnterShop()
    {
        List<Item> items = Managers.ItemManager.GetRandomItems();

        foreach (Item item in items)
        {
            GameObject go = Instantiate(_itemUIPrefab, _itemSlots);
            UI_Item itemUI = go.GetComponent<UI_Item>();
            itemUI.Init(item.Id);
        }

        _canvas.enabled = true;
    }

    void ExitShop()
    {
        _canvas.enabled = false;
    }
}
