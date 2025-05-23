using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Level : MonoBehaviour
{
    int _slotNum;
    TMP_Text _text;
    Image _image;

    void Start()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _image = GetComponentInChildren<Image>();
        _slotNum = GetComponentInParent<UI_Slot>().SlotNum;
        Hide();
        Managers.EquipManager.OnSlotLevelEvent += UpdateVisual;
    }

    void OnDestroy()
    {
        Managers.EquipManager.OnSlotLevelEvent -= UpdateVisual;
    }

    void UpdateVisual()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_slotNum == i)
            {
                int level = Managers.EquipManager.slotLevel[i];
                if (level > 0)
                {
                    _image.enabled = true;
                    _text.text = $"{level}";
                    _text.color = Color.green;
                }
                else if (level < 0)
                {
                    _image.enabled = true;
                    _text.text = $"{level}";
                    _text.color = Color.red;
                }
                else
                {
                    Hide();
                }
            }
        }
    }

    void Hide()
    {
        _text.text = "";
        _image.enabled = false;
    }
}
