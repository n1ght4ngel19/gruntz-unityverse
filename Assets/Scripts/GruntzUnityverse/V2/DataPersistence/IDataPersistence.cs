namespace GruntzUnityverse.V2.DataPersistence {
public interface IDataPersistence {
	string Guid { get; set; }

	void Load(GameData data);

	void Save(ref GameData data);

	void GenerateGuid();
}
}
