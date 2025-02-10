using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    //[SerializeField]
    //Define.PlayerClass _playerClass;

    public AStarManager AStar;

    public void Update()
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


    protected override void Init()
    {
        base.Init();

        Managers.AStar.Init();

        Cursor.lockState = CursorLockMode.Locked;
        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, Managers.Game.PlayerClass.ToString());
        Managers.Game.PlayerStat = player.GetComponent<PlayerStat>();

        Camera.main.gameObject.GetOrAddCompoenet<CameraController>().SetPlayer(player);
        player.transform.position = new Vector3(39f, 0f, 26f);

        GameObject monster = Managers.Game.Spawn(Define.WorldObject.Monster, "CrystalGuardian/CrystalGuardian");
        monster.transform.position = new Vector3(31f, 0f, 43f);

        Managers.UI.InstantiateStatusPopupUI(Managers.Game.PlayerClass);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
