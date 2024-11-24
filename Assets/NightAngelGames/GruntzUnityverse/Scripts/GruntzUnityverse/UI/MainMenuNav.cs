using UnityEngine;


namespace GruntzUnityverse.UI {
public class MainMenuNav : MonoBehaviour {
    public SubMenuNav backButton;

    private void OnCancel() {
        if (backButton == null) {
            return;
        }

        backButton.button.onClick.Invoke();
    }

    // private void OnSubmit() {
    //     EventSystem.current.gameObject.GetComponent<Button>().onClick.Invoke();
    // }
}
}
