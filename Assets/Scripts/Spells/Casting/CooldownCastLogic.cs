using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CooldownCastLogic : SpellCastLogic {

    public float cooldown = 1.0f;
    public Sprite icon;

    [System.NonSerialized]
    private float lastCastTime = 0.0f;

    [System.NonSerialized]
    private Image cooldownIcon;

    public override void Update() {

        base.Update();

        if (cooldownIcon.fillAmount <= 0.0f) return;

        float progress = (Time.time - lastCastTime) / cooldown;
        cooldownIcon.fillAmount = 1.0f - progress;

        if (progress >= 1.0f)
            cooldownIcon.fillAmount = 0.0f;

    }

    public override void OnCastAttempted(Spell spell) {

        if (Time.time < lastCastTime + cooldown) return;

        base.OnCastAttempted(spell);

        lastCastTime = Time.time;
        cooldownIcon.fillAmount = 1.0f;

    }

    public void HookUI(Image cooldownIcon) {
        
        this.cooldownIcon = cooldownIcon.transform.GetChild(0).GetComponent<Image>();
        cooldownIcon.sprite = icon;

    }

}