using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "Items/Drink", order = 2)]
public class DrinkItem : Item {

    [Header("Drink")]
    public float satiety = 1.0f;
    public ContainerItem container;

    public override void Use(Hand hand) {

        Game.Assert(container != null, "Tried to use a drink item with no container item.");

        Stat stat = Player.GetStat(Stat.Type.Thirst);
        Game.Assert(stat != null, "Could not get thirst stat from Player.");

        stat.Replenish(satiety);
        Player.instance.SetItem(hand, container);

    }
    
}