using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnit : ItemUnit {
    public int[] attackRange = new int[2];

    private System.Random rnd = new System.Random();

    public override void Start() {
        base.Start();
    }

    public override void Update() {
        base.Update();
    }

    public override int getActionValue() {
        int attackVal = rnd.Next(attackRange[0], attackRange[1]+1);

        return attackVal;
    }
}