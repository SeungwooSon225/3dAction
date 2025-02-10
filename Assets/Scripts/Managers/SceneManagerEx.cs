using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    AsyncOperation _asyncOperation;
    public AsyncOperation Operation { get { return _asyncOperation; } }

    public void LoadScene(Define.Scene sceneType)
    {
        //Managers.Clear();
        _asyncOperation = SceneManager.LoadSceneAsync(sceneType.ToString());
    }


    public void Clear()
    {
        CurrentScene.Clear();
    }
}
