using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill_woodworking : Skill {
    public List<LevelBonus> levels = new List<LevelBonus>();

    public Skill_woodworking() {

    }
    
    [Serializable]
    public struct LevelBonus {
        [SerializeField]
        int extraLogs;
        [SerializeField]
        int probabilityForExtra;
        [SerializeField]
        UnityEngine.Object bonusObject;
    }
}
