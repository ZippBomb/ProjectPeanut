using UnityEngine;

[CreateAssetMenu(fileName = "SummonSpell", menuName = "Spells/Summon", order = 3)]
public class SummonSpell : Spell {

    public GameObject summon;

    public override void Cast() {

        SummonManager.instance.StartSummon(summon);

    }

}
