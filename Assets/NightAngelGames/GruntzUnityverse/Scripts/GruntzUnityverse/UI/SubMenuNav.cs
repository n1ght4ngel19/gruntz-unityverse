using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace GruntzUnityverse.UI {
public class SubMenuNav : MonoBehaviour {
    public Button button => GetComponent<Button>();

    public TMP_Text text => GetComponent<TMP_Text>();

    public SubMenu menuToEnable;

    public SubMenu menuToDisable => GetComponentInParent<SubMenu>();

    private MainMenuNav mainMenuNav => FindFirstObjectByType<MainMenuNav>();

    public bool overrideBackButtonWarpAround;

    private void OnValidate() {
        if (!overrideBackButtonWarpAround) {
            return;
        }

        Navigation newNav = button.navigation;
        newNav.mode = Navigation.Mode.Explicit;
        newNav.selectOnUp = transform.parent.GetComponentsInChildren<Button>().First(btn => btn.gameObject.name.EndsWith("Level4"));
        newNav.selectOnDown = transform.parent.GetComponentsInChildren<Button>().First(btn => btn.gameObject.name.EndsWith("Level1"));

        button.navigation = newNav;
    }

    private void Start() {
        button.onClick.AddListener(Navigate);
    }

    public void Navigate() {
        menuToEnable.gameObject.SetActive(true);
        mainMenuNav.backButton = menuToEnable.GetComponentsInChildren<SubMenuNav>().FirstOrDefault(nav => nav.CompareTag("BackButton"));
        EventSystem.current.SetSelectedGameObject(menuToEnable.GetComponentsInChildren<Button>().First().gameObject);

        menuToDisable.gameObject.SetActive(false);
    }
}
}
