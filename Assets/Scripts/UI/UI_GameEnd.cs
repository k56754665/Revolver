using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameEnd : MonoBehaviour
{
    TMP_Text _text;
    Canvas _canvas;
    Button _button;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = false;
        _text = GetComponentInChildren<TMP_Text>();
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(RestartGame);
        Managers.GameManager.OnGameClearEvent += Clear;
        Managers.GameManager.OnGameOverEvent += GameOver;
    }

    void OnDestroy()
    {
        Managers.GameManager.OnGameClearEvent -= Clear;
        Managers.GameManager.OnGameOverEvent -= GameOver;
    }

    void RestartGame()
    {
        Managers.GameManager.Restart();
    }

    void Clear()
    {
        _text.text = "클리어!\n축하합니다.";
        _canvas.enabled = true;
    }

    void GameOver()
    {
        _text.text = "게임오버";
        _canvas.enabled = true;
    }
}
