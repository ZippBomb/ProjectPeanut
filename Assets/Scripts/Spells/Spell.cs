using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell", order = 0)]
public class Spell : ScriptableObject {

    [SerializeReference]
    public SpellCastLogic castLogic = new SpellCastLogic();
    [HideInInspector]
    public SpellCastLogic.Type castLogicType = SpellCastLogic.Type.Instant;

    public virtual void Update() {
        
        castLogic.Update();

    }

    public virtual void Cast() {

        Debug.Log("Spell '" + name + "' was casted.");

    }
    public void AttemptCast() {

        Game.Assert(castLogic != null, "Spell '" + name + "' has no cast logic.");

        castLogic.OnCastAttempted(this);

    }

    public void HookUI(Image cooldownIcon) {
        
        if (castLogicType != SpellCastLogic.Type.Cooldown) return;

        CooldownCastLogic cooldownLogic = (CooldownCastLogic) castLogic;
        cooldownLogic.HookUI(cooldownIcon);

    }

}