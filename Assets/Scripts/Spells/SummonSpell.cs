using UnityEngine;

[CreateAssetMenu(fileName = "SummonSpell", menuName = "Spells/Summon", order = 3)]
public class SummonSpell : Spell {

    public GameObject summon;

    public override void Cast() {

        Debug.Log("Summon spell: " + name);
        SummonManager.instance.StartSummon(summon);

    }

}
