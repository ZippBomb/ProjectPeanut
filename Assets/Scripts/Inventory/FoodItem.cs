using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Items/Food", order = 1)]
public class FoodItem : Item {

    public float satiety = 1.0f;
    public bool edible = true;

    public override void Use(Hand hand) {

        if (!edible) return;

        Stat stat = Player.GetStat(Stat.Type.Satiety);
        Game.Assert(stat != null, "Could not get hunger stat from Player.");

        stat.Replenish(satiety);
        Player.instance.RemoveItem(hand);

    }
    
}