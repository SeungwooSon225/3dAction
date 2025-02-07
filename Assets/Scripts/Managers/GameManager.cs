using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    GameObject _player;
    GameObject _monster;

    public Define.PlayerClass PlayerCalss { get; set; }

    public Action<int> OnSpawnEvent;

    public GameObject Player { get { return _player; } }
    public PlayerStat PlayerStat { get; set; }

    public GameObject Monster { get { return _monster; } }

    public void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Managers.UI.ShowStatusPopup();
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Managers.UI.HideStatusPopup();
            }
        }
    }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);

        switch (type)
        {
            case Define.WorldObject.Monster:
                _monster = go;
                break;
            case Define.WorldObject.Player:
                _player = go;
                break;
        }

        return go;
    }

    public void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Managers.UI.InstantiateStatusPopupUI();
        Managers.UI.HideStatusPopup();
    }
}
