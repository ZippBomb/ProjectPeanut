[System.Serializable]
public class SpellCastLogic {

    public enum Type { Instant, Cooldown }

    public virtual void Update() {}

    public virtual void OnCastAttempted(Spell spell) {

        Game.Assert(spell != null, "No spell provided.");
        spell.Cast();

    }

}