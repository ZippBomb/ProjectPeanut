using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell", order = 0)]
public class Spell : ScriptableObject {

    [SerializeReference]
    public SpellCastLogic castLogic = new SpellCastLogic();
    [HideInInspector]
    public SpellCastLogic.Type castLogicType = SpellCastLogic.Type.Instant;

    public virtual void Cast() {

        Debug.Log("Spell '" + name + "' was casted.");

    }
    public void AttemptCast() {

        Game.Assert(castLogic != null, "Spell '" + name + "' has no cast logic.");

        castLogic.OnCastAttempted(this);

    }

}