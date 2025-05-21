using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance => _instance;
    static Managers _instance;

    public static InputManager InputManager => _inputManager;
    static InputManager _inputManager = new InputManager();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitManagers()
    {
        _inputManager.Init();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitManagers();
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _inputManager.Clear();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
