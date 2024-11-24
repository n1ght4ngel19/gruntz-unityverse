using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace GruntzUnityverse.UI {
public class NavItem : MonoBehaviour {
    public NavItem next;

    public NavItem previous => transform.parent.GetComponentsInChildren<NavItem>().FirstOrDefault(b => b.next == this);

    public Button button => GetComponent<Button>();

    public TMP_Text text => GetComponent<TMP_Text>();

    public void Activate() {
        text.color = button.enabled ? GameColors.enabledButton : GameColors.disabledButton;

        if (FindFirstObjectByType<Button>().IsActive()) {
            GetComponent<Button>().onClick.Invoke();
        }

        if (TryGetComponent(out LevelLoader loader)) {
            loader.LoadLevel();

            return;
        }

        if (TryGetComponent(out SubMenuNav navigator)) {
            navigator.Navigate();
        }
    }

    public void Deactivate() {
        text.color = GameColors.disabledButton;
    }
}
}
