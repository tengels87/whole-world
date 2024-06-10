using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class WorldConstants {
	public static string objName_world = "World";
	public static string objName_world_items = "World/Items";

	static readonly WorldConstants _instance = new WorldConstants();
	public static WorldConstants Instance {
		get {
			return _instance;
		}
	}

	private WorldConstants() {

	}

	public static string getLocalConfigPath() {
		string appDataPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
		appDataPath += "\\architecture_experience\\configs\\";

		return appDataPath;
	}

	public static string getLocalLogPath() {
		string appDataPath = Application.dataPath; // aka StreamingAssets
		appDataPath += "\\..\\log\\";

		return appDataPath;
	}
}
