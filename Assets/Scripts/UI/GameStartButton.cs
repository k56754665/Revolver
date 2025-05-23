using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    Canvas _canvas;
    Button _button;
    
    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = true;
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(StartEquip);
    }

    void StartEquip()
    {
        Managers.GameManager.StartEquip();
        _canvas.enabled = false;
    }
}
