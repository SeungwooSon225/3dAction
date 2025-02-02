using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "Wizard");
        Managers.Game.PlayerCalss = Define.PlayerClass.Wizard;
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
