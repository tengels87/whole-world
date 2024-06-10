using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill_stoneprocessing : Skill {
    public List<LevelBonus> levels = new List<LevelBonus>();

    public Skill_stoneprocessing() {

    }

    [Serializable]
    public class LevelBonus {
        [SerializeField]
        public List<LevelBonusElement> bonus = new List<LevelBonusElement>();
    }

    [Serializable]
    public struct LevelBonusElement {
        [SerializeField]
        UnityEngine.Object obj;
        [SerializeField]
        int count;
        [Range(0, 1), SerializeField]
        float propability;

    }
}
