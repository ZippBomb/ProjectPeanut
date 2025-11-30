using UnityEngine;

[CreateAssetMenu(fileName = "SummonSpell", menuName = "Spells/Summon", order = 3)]
public class SummonSpell : Spell {

    public GameObject summon;
    public int cap = 1000;

    public override void Cast() {

        if (SummonManager.instance.GetUnitCount(summon.GetComponent<Summon>().GetID()) >= cap) return;
        SummonManager.instance.StartSummon(summon);

    }

}
