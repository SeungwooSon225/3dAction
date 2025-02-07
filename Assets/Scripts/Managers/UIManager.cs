using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public UI_Stat UI_PlayerStat;

    public GameObject Root
    {
        get 
        {
            GameObject root = GameObject.Find("@UI_Root");

            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public void SetCanvas(GameObject go)
    {
        Canvas canvas = Util.GetOrAddCompoenet<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 0;
    }

    UI_StatusPopup StatusPopupUI;

    public void InstantiateStatusPopupUI()
    {
        StatusPopupUI = Managers.Resource.Instantiate($"UI/UI_StatusPopup").GetComponent<UI_StatusPopup>();
    }

    public void ShowStatusPopup()
    {
        StatusPopupUI.gameObject.SetActive(true);
        StatusPopupUI.GetStat();
    }

    public void HideStatusPopup()
    {
        StatusPopupUI.gameObject.SetActive(false);
    }

    public T ShowUI<T>(string name = null) where T : MonoBehaviour
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/{name}");

        T ui = Util.GetOrAddCompoenet<T>(go);
        go.transform.SetParent(Root.transform);

        if (ui.GetType() == typeof(UI_Stat))
        {
            UI_PlayerStat = ui as UI_Stat;
        }

        return ui;
    }
}
