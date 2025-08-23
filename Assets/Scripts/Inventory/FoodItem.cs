using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Items/Food", order = 1)]
public class FoodItem : Item {

    public float satiety = 1.0f;

    public override void Use(Hand hand) {

        Stat stat = Player.GetHungerStat();
        Game.Assert(stat != null, "Could not get hunger stat from Player.");

        stat.Replenish(satiety);
        Player.instance.RemoveItem(hand);

    }
    
}