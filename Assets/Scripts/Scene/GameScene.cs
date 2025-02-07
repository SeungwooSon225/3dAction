using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    Define.PlayerClass _playerClass;

    protected override void Init()
    {
        base.Init();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, _playerClass.ToString());
        Managers.Game.PlayerCalss = _playerClass;
        Managers.Game.PlayerStat = player.GetComponent<PlayerStat>();

        Camera.main.gameObject.GetOrAddCompoenet<CameraController>().SetPlayer(player);
        player.transform.position = new Vector3(39f, 0f, 26f);

        GameObject monster = Managers.Game.Spawn(Define.WorldObject.Monster, "CrystalGuardian/CrystalGuardian");
        monster.transform.position = new Vector3(31f, 0f, 43f);
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
