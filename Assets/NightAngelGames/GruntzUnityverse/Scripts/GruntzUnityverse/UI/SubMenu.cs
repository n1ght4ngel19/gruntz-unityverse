using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;


namespace GruntzUnityverse.UI {
public class SubMenu : MonoBehaviour {
    public List<NavItem> navItemz => GetComponentsInChildren<NavItem>().ToList();

    public NavItem firstNavItem => navItemz.OrderByDescending(nav => nav.GetComponent<RectTransform>().position.y).First();

    [ReadOnly]
    public NavItem activeNavItem;

    // private void OnEnable() {
    //     firstNavItem.text.color = firstNavItem.button.enabled ? GameColors.enabledButton : GameColors.disabledButton;
    //     activeNavItem = firstNavItem;
    // }
}
}
