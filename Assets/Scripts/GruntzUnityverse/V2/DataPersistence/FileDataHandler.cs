using System;
using System.IO;
using UnityEngine;

namespace GruntzUnityverse.V2.DataPersistence {
public class FileDataHandler {
	private string directoryPath;

	private string dataFileName;

	public FileDataHandler(string directoryPath, string dataFileName) {
		this.directoryPath = directoryPath;
		this.dataFileName = dataFileName;
	}

	public GameData LoadGameData() {
		string filePath = Path.Combine(directoryPath, dataFileName);

		if (!File.Exists(filePath)) {
			return new GameData();
		}

		try {
			string dataAsJson = File.ReadAllText(filePath);

			return JsonUtility.FromJson<GameData>(dataAsJson);
		} catch (Exception exception) {
			Debug.LogError(exception);

			throw;
		}
	}

	public void SaveGameData(GameData gameData) {
		string dataAsJson = JsonUtility.ToJson(gameData, true);
		string filePath = Path.Combine(directoryPath, dataFileName);

		try {
			File.WriteAllText(filePath, dataAsJson);
		} catch (Exception exception) {
			Debug.LogError("Error saving game data to file: " + filePath);
			Debug.LogError(exception);

			throw;
		}
	}
}
}
