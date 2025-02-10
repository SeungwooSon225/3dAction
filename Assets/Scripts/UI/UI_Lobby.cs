using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Lobby : UI_Base
{ 
    [SerializeField]
    GameObject _classSelectUI;
    [SerializeField]
    GameObject _loadingUI;

    [SerializeField]
    TMPro.TMP_Text _noticeText;
    [SerializeField]
    GameObject _warriorImage;
    [SerializeField]
    GameObject _wizardImage;
    [SerializeField]
    GameObject _gameStartButton;

    [SerializeField]
    TMPro.TMP_Text _loadingText;
    [SerializeField]
    Slider _loadingBar;

    bool _isSelected;
    float _minimumLoadingTime = 1f;

    public override void Init()
    {
        BindEvent(_warriorImage, OnWarriorIamgeClicked, Define.UIEvent.Click);
        BindEvent(_wizardImage, OnWizardIamgeClicked, Define.UIEvent.Click);
        BindEvent(_gameStartButton, OnGameStartButtonClicked, Define.UIEvent.Click);
    }

    void OnWarriorIamgeClicked(PointerEventData data)
    {
        _isSelected = true;
        Managers.Game.PlayerClass = Define.PlayerClass.Warrior;
        _warriorImage.GetComponent<Image>().color = Color.red;
        _wizardImage.GetComponent<Image>().color = Color.white;
    }

    void OnWizardIamgeClicked(PointerEventData data)
    {
        _isSelected = true;
        Managers.Game.PlayerClass = Define.PlayerClass.Wizard;
        _warriorImage.GetComponent<Image>().color = Color.white;
        _wizardImage.GetComponent<Image>().color = Color.blue;
    }

    IEnumerator BlinkNoticeTextCo()
    {
        for (int i = 0; i < 2; i++)
        {
            _noticeText.color = Color.gray;
            yield return new WaitForSeconds(0.2f);
            _noticeText.color = Color.white;
            yield return new WaitForSeconds(0.2f);
        }
    }

    void OnGameStartButtonClicked(PointerEventData data)
    {
        if (!_isSelected)
        {
            StartCoroutine(BlinkNoticeTextCo());
            return;
        }
        _gameStartButton.GetComponent<Image>().color = Color.gray;

        StartCoroutine(LoadSceneCo());
        //Managers.Scene.LoadScene(Define.Scene.GameScene);
    }

    IEnumerator LoadSceneCo()
    {
        _classSelectUI.SetActive(false);
        _loadingUI.SetActive(true);

        Managers.Scene.LoadScene(Define.Scene.GameScene);
        AsyncOperation operation = Managers.Scene.Operation;
        operation.allowSceneActivation = false;

        float _elapsedTime = 0f;
        while (_elapsedTime < _minimumLoadingTime || operation.progress < 0.9f)
        {
            _elapsedTime += Time.deltaTime;

            _loadingBar.value = Mathf.Min(_elapsedTime / _minimumLoadingTime, Mathf.Clamp01(operation.progress / 0.9f));
            _loadingText.text = $"{(int)(_loadingBar.value * 100)}%";

            yield return null;
        }

        operation.allowSceneActivation = true;


        //while (!operation.isDone)
        //{
        //    // 로드 진행 상태를 UI에 반영 (0 ~ 1)
        //    float progress = Mathf.Clamp01(operation.progress / 0.9f) - 0.9f;

        //    _loadingBar.value = 0.9f + progress;
        //    _loadingText.text = $"{_loadingBar.value * 100}%";

        //    yield return null;  
        //}
        //yield return new WaitForSeconds(5f);
    }
}
