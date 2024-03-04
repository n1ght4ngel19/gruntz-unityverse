using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.UI;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Fort : MonoBehaviour {
	public BoxCollider2D boxCollider2D;

	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		Debug.Log("Colliding with Grunt");

		if (grunt.equippedTool.toolName == "Warpstone") {
			Debug.Log("Colliding with Warpstone Grunt");

			FindObjectsByType<Grunt>(FindObjectsSortMode.None)
				.Where(g => g.CompareTag("Dizgruntled"))
				.ToList()
				.ForEach(g1 => g1.enabled = false);


			// FindObjectsByType<Grunt>(FindObjectsSortMode.None)
			// 	.Where(g => !g.CompareTag("Dizgruntled"))
			// 	.ToList()
			// 	.ForEach(g1 => g1.Animancer.Play(g1.exitAnimation));

			Time.timeScale = 0f;

			Level.Instance.gameObject.GetComponent<Grid>().enabled = false;
			GameObject.Find("Actorz").SetActive(false);
			GameObject.Find("Postz").SetActive(false);
			GameObject.Find("Itemz").SetActive(false);
			GameObject.Find("Objectz").SetActive(false);

			StatzMenu.Instance.Activate();
		}
	}
}
}
