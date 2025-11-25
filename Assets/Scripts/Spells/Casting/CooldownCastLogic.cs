using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CooldownCastLogic : SpellCastLogic {

    public float cooldown = 1.0f;

    [System.NonSerialized]
    private float lastCastTime = 0.0f;

    [System.NonSerialized]
    private Image cooldownIcon;

    public override void Update() {

        base.Update();

        if (!cooldownIcon.enabled) return;

        float progress = (Time.time - lastCastTime) / cooldown;
        cooldownIcon.fillAmount = 1.0f - progress;

        if (progress >= 1.0f)
            cooldownIcon.enabled = false;

    }

    public override void OnCastAttempted(Spell spell) {

        if (Time.time < lastCastTime + cooldown) return;

        base.OnCastAttempted(spell);

        lastCastTime = Time.time;
        cooldownIcon.enabled = true;

    }

    public void HookUI(Image cooldownIcon) {
        
        this.cooldownIcon = cooldownIcon;

    }

}