using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour {
    public CustomMouseManager mouseManager;
    public InventoryManager inventoryManager;
    public List<Recipe> recipeList = new List<Recipe>();

    private recipeSlotUI[] recipeSlots;
    private Image backdrop;
    private bool isOpen = false;

    // UI grid
    private int slotWidth = 48;
    private int margin = 16;

    void OnEnable() {
        mouseManager.onItemCollected += unlockRecipes;
    }

    void OnDisable() {
        mouseManager.onItemCollected -= unlockRecipes;
    }

    void Start() {
        backdrop = this.gameObject.GetComponent<Image>();

        createGOitems();
        setVisible(false);
    }

    void Update() {

        // TEST: toggle crafting menu
        if (Input.GetKeyDown(KeyCode.F2))
            setVisible(!isOpen);
    }

    public void setVisible(bool val) {
        isOpen = val;

        backdrop.enabled = isOpen;

        updateUI();
    }

    public bool isVisible() {
        return isOpen;
    }

    private void createGOitems() {
        recipeSlots = new recipeSlotUI[recipeList.Count];

        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        float step = slotWidth + margin;

        for (int r = 0; r < recipeList.Count; r++) {
            List<ItemUnit> tmpItemList = new List<ItemUnit>(recipeList[r].ingredientList);
            tmpItemList.Add(recipeList[r].outcome);

            // slot GO (per recipe, parent for all ingredients)
            recipeSlots[r].mainGO = (GameObject)Object.Instantiate((GameObject)Resources.Load("Prefabs/UI/RecipeSlotParent"));
            recipeSlots[r].mainGO.name = "recipeGO_" + recipeList[r].outcome.unitName;
            recipeSlots[r].mainGO.transform.SetParent(this.transform, false);
            recipeSlots[r].ingredientsParent = recipeSlots[r].mainGO.GetComponentsInChildren<RectTransform>()[1].transform;

            int index = 0;

            foreach (ItemUnit item in tmpItemList) {
                GameObject ingreGO = (GameObject)Object.Instantiate((GameObject)Resources.Load("Prefabs/UI/RecipeSlot"));
                ingreGO.name = "ingreGO_r_" + recipeList[r].outcome.unitName + "_" + item.unitName;
                ingreGO.transform.SetParent(recipeSlots[r].ingredientsParent, false);

                ingreGO.GetComponentsInChildren<Image>()[1].sprite = item.icon;
                ingreGO.GetComponentInChildren<Text>().text = "";

                ingreGO.transform.localPosition = new Vector3(margin, -margin, 0) + new Vector3(index * step, 0, 0);
                ingreGO.transform.localRotation = Quaternion.identity;

                index++;
            }

            // craft button
            recipeSlots[r].craftButton = recipeSlots[r].mainGO.GetComponentInChildren<Button>();
            // shift to the right, behind outcome
            recipeSlots[r].craftButton.transform.localPosition += new Vector3(50 + margin + index * step, 0, 0);
            // add listener for crafting
            int craftItemInd = r;
            recipeSlots[r].craftButton.onClick.AddListener(() => {
                GameObject newOutcome = (GameObject)Object.Instantiate(recipeList[craftItemInd].outcome.gameObject, GameObject.Find(WorldConstants.objName_world_items).transform);
                inventoryManager.addItem(newOutcome.GetComponent<ItemUnit>());
            });

            recipeSlots[r].mainGO.SetActive(false);
        }
    }

    private void updateUI() {
        for (int r = 0; r < recipeSlots.Length; r++) {
            if (isOpen && recipeList[r].isUnlocked) {
                recipeSlots[r].mainGO.SetActive(true);
            } else {
                recipeSlots[r].mainGO.SetActive(false);
            }
        }
    }

    private void updateGOitemsPosition() {
        RectTransform rect = this.gameObject.GetComponent<RectTransform>();
        float step = slotWidth + margin;

        int rGOind = 0;
        for (int r=0; r<recipeSlots.Length; r++) {
            if (recipeList[r].isUnlocked) {
                recipeSlots[r].mainGO.transform.localPosition = new Vector3(margin, rect.sizeDelta.y - rGOind * step, 0);
                recipeSlots[r].mainGO.transform.localRotation = Quaternion.identity;
                recipeSlots[r].mainGO.SetActive(isOpen);

                rGOind++;
            } else {
                recipeSlots[r].mainGO.SetActive(false);
            }
        }
    }

    private void unlockRecipes(ItemUnit eventUnit) {
        for (int i = 0; i < recipeList.Count; i++) {
            if (!recipeList[i].isUnlocked) {
                bool canCook = recipeList[i].checkIngredientsTypeOnly(inventoryManager.getItemsAll());
                if (canCook) {
                    recipeList[i].isUnlocked = true;
                }
            }
        }

        updateGOitemsPosition();

        isCookable();
    }

    private void isCookable() {
        for (int i = 0; i < recipeList.Count; i++) {
            if (recipeList[i].isUnlocked) {
                bool isCookable = recipeList[i].checkIngredientsFull(inventoryManager.getItemsAll());
                recipeSlots[i].craftButton.enabled = isCookable;
            }
        }

        updateGOitemsPosition();
    }

    struct recipeSlotUI {
        public GameObject mainGO;
        public string description;
        public Transform ingredientsParent;
        public Button craftButton;
    }
}
