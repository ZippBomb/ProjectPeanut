using UnityEngine;

[CreateAssetMenu(fileName = "StatSpell", menuName = "Spells/Stat", order = 2)]
public class StatSpell : Spell {

    public Stat.Type stat;
    public float amount;

    public override void Cast() {

        Player.GetStat(stat).Replenish(amount);

    }

}