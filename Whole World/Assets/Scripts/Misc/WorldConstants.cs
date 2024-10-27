using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class WorldConstants {
	public static string objName_camera_main = "MainCamera";
	public static string objName_player = "Player";
	public static string objName_world = "World";
	public static string objName_world_items = "World/Items";
	public static string objName_canvas_main = "MainCanvas";
	public static string objName_inventory_main = "Inventory_backpack";
	public static string objName_inventory_belt = "Inventory_belt";
	public static string objName_mouseManager = "MouseManager";
	public static string objName_skillmanager = "SkillManager";

	public Camera cameraMain;
	public NpcUnit player;
	public Canvas canvasMain;
	public InventoryManager playerInventory;
	public CustomMouseManager mouseManager;
	public SkillManager skillManager;

	static readonly WorldConstants _instance = new WorldConstants();
	public static WorldConstants Instance {
		get {
			return _instance;
		}
	}

	private WorldConstants() {

	}

	public Camera getMainCamera() {
		if (cameraMain == null) {
			cameraMain = GameObject.Find(objName_camera_main).GetComponent<Camera>();
		}

		return cameraMain;
	}

	public NpcUnit getPlayer() {
		if (player == null) {
			player = GameObject.Find(objName_player).GetComponentInChildren<NpcUnit>();
		}

		return player;
	}

	public Canvas getMainCanvas() {
		if (canvasMain == null) {
			canvasMain = GameObject.Find(objName_canvas_main).GetComponent<Canvas>();
		}

		return canvasMain;
	}

	public InventoryManager getPlayerInventory() {
		if (playerInventory == null) {
			playerInventory = GameObject.Find(objName_inventory_main).GetComponent<InventoryManager>();
		}

		return playerInventory;
	}

	public CustomMouseManager getMouseManager() {
		if (mouseManager == null) {
			mouseManager = GameObject.Find(objName_mouseManager).GetComponent<CustomMouseManager>();
		}

		return mouseManager;
	}

	public SkillManager getSkillManager() {
		if (skillManager == null) {
			skillManager = GameObject.Find(objName_skillmanager).GetComponent<SkillManager>();
		}

		return skillManager;
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
