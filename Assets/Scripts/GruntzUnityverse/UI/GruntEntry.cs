using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace GruntzUnityverse.UI {
public class GruntEntry : MonoBehaviour {
	public int entryId => int.Parse(gameObject.name.Split("_").Last());
	private Grunt connectedGrunt => GameManager.instance.allGruntz.First(gr => gr.gruntId == entryId);

	[Header("Slotz")]
	public GameObject slotz;

	public Sprite simpleSlotzSprite;
	public Sprite highlightedSlotzSprite;

	public Image headSlot;
	public Sprite greenHead;
	public Sprite yellowHead;
	public Sprite redHead;

	public Image toolSlot;
	public Image toySlot;
	public Image powerupSlot;
	public Image healSlot;

	public Sprite blankSlotIcon;
	public Sprite blackAttributeBar;

	[Header("Attribute Barz")]
	public Image healthBar;

	public List<Sprite> healthFrames;

	public Image staminaBar;

	public List<Sprite> staminaFrames;

	public void SelectConnected() {
		Camera.main.transform.position = new Vector3(
			connectedGrunt.transform.position.x,
			connectedGrunt.transform.position.y,
			Camera.main.transform.position.z
		);

		connectedGrunt.Select();
	}

	public void HighLight(bool highlight = true) {
		slotz.GetComponent<Image>().sprite = highlight ? highlightedSlotzSprite : simpleSlotzSprite;
	}

	public void Initialize(Grunt grunt) {
		SetHealth(grunt.statz.health);
		SetStamina(grunt.statz.stamina);
		SetTool(grunt.equippedTool.toolName);
		SetToy(grunt.equippedToy.toyName);
		// SetPowerup(grunt.equippedPowerupz.First().powerupName);
		// SetHeal();
	}

	public void Clear() {
		healthBar.sprite = blackAttributeBar;
		staminaBar.sprite = blackAttributeBar;
		headSlot.sprite = blankSlotIcon;
		toolSlot.sprite = blankSlotIcon;
		toySlot.sprite = blankSlotIcon;
		powerupSlot.sprite = blankSlotIcon;
		healSlot.sprite = blankSlotIcon;
	}

	public void ClearSlot(string slotName) {
		switch (slotName) {
			case "Tool":
				toolSlot.sprite = blankSlotIcon;

				break;
			case "Toy":
				toySlot.sprite = blankSlotIcon;

				break;
			case "Powerup":
				powerupSlot.sprite = blankSlotIcon;

				break;
			case "Heal":
				healSlot.sprite = blankSlotIcon;

				break;
		}
	}

	public void SetHealth(int value) {
		healthBar.sprite = healthFrames[value];
		headSlot.sprite = value >= 11 ? greenHead : value >= 6 ? yellowHead : redHead;
	}

	public void SetStamina(int value) {
		staminaBar.sprite = staminaFrames[value];
	}

	public void SetTool(string toolName) {
		if (toolName is "BareHandz" or "") {
			toolSlot.sprite = blankSlotIcon;

			return;
		}

		Addressables.LoadAssetAsync<Sprite>($"ToolSlot_{toolName}").Completed += handle => {
			toolSlot.sprite = handle.Result;
		};
	}

	public void SetToy(string toyName) {
		if (toyName is "") {
			toolSlot.sprite = blankSlotIcon;

			return;
		}

		Addressables.LoadAssetAsync<Sprite>($"ToySlot_{toyName}").Completed += handle => {
			toySlot.sprite = handle.Result;
		};
	}

	public void SetPowerup(string powerupName) {
		if (powerupName is "") {
			toolSlot.sprite = blankSlotIcon;

			return;
		}

		Addressables.LoadAssetAsync<Sprite>($"PowerupSlot_{powerupName}").Completed += handle => {
			powerupSlot.sprite = handle.Result;
		};
	}

	public void SetHeal(string healName) {
		if (healName is "") {
			toolSlot.sprite = blankSlotIcon;

			return;
		}

		Addressables.LoadAssetAsync<Sprite>($"HealSlot_{healName}").Completed += handle => {
			healSlot.sprite = handle.Result;
		};
	}
}
}
