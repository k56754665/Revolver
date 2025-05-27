using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    TMP_Text _text;
    Button _button;
    int _id;

    public void Init(int id)
    {
        _id = id;
        _text = GetComponentInChildren<TMP_Text>();
        _button = GetComponentInChildren<Button>();
        _text.text = Managers.ItemManager.GetItem(_id).Name;
        _button.onClick.AddListener(SetButton);
    }

    void SetButton()
    {
        Managers.ItemManager.OwnItem(_id);
        Managers.GameManager.StartEquip();
    }
}
