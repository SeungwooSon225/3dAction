using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        Managers.BGM.Play("Lobby");
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
