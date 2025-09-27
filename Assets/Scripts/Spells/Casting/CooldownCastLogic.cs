using UnityEngine;

[System.Serializable]
public class CooldownCastLogic : SpellCastLogic {

    public float cooldown = 1.0f;

    [System.NonSerialized]
    float lastCastTime = 0.0f;

    public override void OnCastAttempted(Spell spell) {

        if (Time.time < lastCastTime + cooldown) return;

        base.OnCastAttempted(spell);
        lastCastTime = Time.time;

    }

}