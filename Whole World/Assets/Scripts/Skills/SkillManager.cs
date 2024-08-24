using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {
    public Object skillSlot;
    public List<Skill> skillList = new List<Skill>();

    private GameObject[] skillSlots;
    private Image backdrop;
    private bool isOpen = false;

    // UI grid
    private int spacing = 48;
    private int margin = 8;

    void OnEnable() {
        foreach (Skill s in skillList) {
            print(s.name);
            s.onSkillUnlocked += SkillUnlockedListener;
            s.onSkillLevelup += SkillLevelupListener;
        }
    }

    void OnDisable() {
        foreach (Skill s in skillList) {
            s.onSkillUnlocked -= SkillUnlockedListener;
            s.onSkillLevelup -= SkillLevelupListener;
        }
    }

    void Start() {
        backdrop = this.gameObject.GetComponent<Image>();

        createGOitems();
        setVisible(false);
    }

    void Update() {

        // TEST: toggle crafting menu
        if (Input.GetKeyDown(KeyCode.F3))
            setVisible(!isOpen);

        if (isOpen) {
            updateUI();
        }
    }

    public Skill getSkill(Skill.Type typ) {
        return skillList.Find(x => x.skillType == typ);
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
        skillSlots = new GameObject[skillList.Count];

        RectTransform rect = this.gameObject.GetComponent<RectTransform>();

        for (int i = 0; i < skillList.Count; i++) {
            skillSlots[i] = (GameObject)Object.Instantiate(skillSlot);
            skillSlots[i].transform.SetParent(this.transform, false);
            
            skillSlots[i].GetComponentsInChildren<Image>()[1].fillAmount = 0;
            skillSlots[i].GetComponentInChildren<Text>().text = skillList[i].skillName;

            skillSlots[i].SetActive(false);
        }
    }

    private void updateUI() {
        for (int i = 0; i < skillSlots.Length; i++) {
            if (isOpen && skillList[i].isUnlocked()) {
                skillSlots[i].GetComponentsInChildren<Image>()[1].fillAmount = (float)skillList[i].exp / (float)skillList[i].maxEXP;
                skillSlots[i].GetComponentInChildren<Text>().text = skillList[i].skillName + ", Level " + skillList[i].level;

                skillSlots[i].SetActive(true);
            } else {
                skillSlots[i].SetActive(false);
            }
        }
    }

    private void updateGOitemsPosition() {
        RectTransform rect = this.gameObject.GetComponent<RectTransform>();

        int rGOind = 0;
        for (int i = 0; i < skillSlots.Length; i++) {
            if (skillList[i].isUnlocked()) {
                skillSlots[i].transform.localPosition = new Vector3(margin, - margin, 0) + new Vector3(margin, rect.sizeDelta.y - rGOind * (spacing + 2 * margin), 0);
                skillSlots[i].transform.localRotation = Quaternion.identity;
                skillSlots[i].SetActive(isOpen);

                rGOind++;
            } else {
                skillSlots[i].SetActive(false);
            }
        }
    }

    private void unlockSkill(Skill.Type skillType) {
        getSkill(skillType).unlock();
    }

    private void SkillUnlockedListener(Skill skill) {
        updateGOitemsPosition();

        CustomEvent.SendMessage("Unlocked new skill", null, skill.skillName, CustomEvent.MessageType.INFO);
    }

    private void SkillLevelupListener(Skill skill) {
        CustomEvent.SendMessage(skill.skillName, null, "Level " + skill.level + " reached", CustomEvent.MessageType.INFO);
    }
}


