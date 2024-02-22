namespace GruntzUnityverse.DataPersistence {
public interface IDataPersistence {
	string Guid { get; set; }

	void Load(GameData data);

	void Save(ref GameData data);

	void GenerateGuid();
}
}
