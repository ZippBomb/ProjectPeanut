using UnityEngine;

[CreateAssetMenu(fileName = "AttackSpell", menuName = "Spells/Attack", order = 1)]
public class AttackSpell : Spell {

    public float damage = 1.0f;

    public override void Cast() {

        Debug.Log("Attack spell '" + name + "' casted.");

    }
    
}