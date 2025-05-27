using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static Managers Instance => _instance;
    static Managers _instance;

    public static GameManager GameManager => _gameManager;
    static GameManager _gameManager = new GameManager();

    public static InputManager InputManager => _inputManager;
    static InputManager _inputManager = new InputManager();

    public static BulletManager BulletManager => _bulletManager;
    static BulletManager _bulletManager = new BulletManager();

    public static EquipManager EquipManager => _equipManager;
    static EquipManager _equipManager = new EquipManager();

    public static BattleManager Battlemanager => _battleManager;
    static BattleManager _battleManager = new BattleManager();

    public static CursorManager CursorManager => _cursorManager;
    static CursorManager _cursorManager = new CursorManager();

    public static StageManager StageManager => _stageManager;
    static StageManager _stageManager = new StageManager();

    public static ItemManager ItemManager => _itemManager;
    static ItemManager _itemManager = new ItemManager();

    public static GameLogger GameLogger => _gameLogger;
    static GameLogger _gameLogger = new GameLogger();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _inputManager.Init();
            _gameLogger.Init();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitManagers()
    {
        _bulletManager.Init();
        _equipManager.Init();
        _gameManager.Init();
        _cursorManager.Init();
        _stageManager.Init();
        _itemManager.Init();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("GameStart");
        InitManagers();
    }

    void OnDestroy()
    {
        if (_instance == this)
        {
            _inputManager.Clear();
            _gameLogger.Clear();
            _cursorManager.Clear();
            _stageManager.Clear();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
