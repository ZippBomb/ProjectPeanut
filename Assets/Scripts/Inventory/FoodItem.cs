using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Items/Food", order = 1)]
public class FoodItem : Item {

    [Header("Food")]
    public float satiety = 1.0f;
    public bool edible = true;

    public override void Use(Hand hand) {

        if (!edible) return;

        // Replenish stat

        SatietyStat satietyStat = (SatietyStat) Player.GetStat(Stat.Type.Satiety);
        Game.Assert(satietyStat != null, "Could not get hunger stat from Player.");

        satietyStat.Replenish(satiety);

        // The player heals for some time, which is directly proportional to the satiety.

        HealthStat healthStat = (HealthStat) Player.GetStat(Stat.Type.Health);
        Game.Assert(healthStat != null, "Could not get health stat from Player.");

        healthStat.OnEaten(satiety);

        Player.instance.RemoveItem(hand);

    }
    
}
