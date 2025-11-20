using UnityEngine;

public class Rifter : Summon {

    [Header("Rifter")]
    [SerializeField] private float riftDamageRate = 1.0f;

    public override void UpdateSummon() {

        base.UpdateSummon();

        if (!inWallRange) return;
        Wall.instance.DamageRifts(riftDamageRate * Time.deltaTime);

    }

}
