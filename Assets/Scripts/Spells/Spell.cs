using UnityEngine;

[CreateAssetMenu(fileName = "Spell", menuName = "Spells/Spell", order = 0)]
public class Spell : ScriptableObject {

    public virtual void Cast() {

        Debug.Log("Spell '" + name + "' was casted.");

    }

}