using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    // ½Ì±ÛÅæÀ¸·Î Managers¸¦ ±¸Çö
    static Managers s_instance;
    static Managers Instance { get { Init();  return s_instance; } }

    GameManager _game = new GameManager();
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    AStarManager _aStar = new AStarManager();
    UIManager _ui = new UIManager();

    public static GameManager Game { get { return Instance._game; } }
    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static AStarManager AStar { get { return Instance._aStar; } }
    public static UIManager UI { get { return Instance._ui; } }


    void Start()
    {
        s_instance = this;
    }

    void Update()
    {
        _input.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject managers = GameObject.Find("[Managers]");

            if (managers == null)
            {
                managers = new GameObject { name = "[Managers]" };
                managers.AddComponent<Managers>();
            }

            DontDestroyOnLoad(managers);
            s_instance = managers.GetComponent<Managers>();

            s_instance._data.Init();
            s_instance._aStar.Init();
            s_instance._pool.Init();
            s_instance._game.Init();
        }
    }
}
