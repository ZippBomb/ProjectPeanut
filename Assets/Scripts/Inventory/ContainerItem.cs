using UnityEngine;

[CreateAssetMenu(fileName = "Container", menuName = "Items/Container", order = 3)]
public class ContainerItem : Item {

    [Header("Container")]
    public DrinkItem drink;

    public override void Use(Hand hand) {

        Game.Assert(drink != null, "Tried to use a container item with no drink item.");

        // Refill logic here
        //Player.instance.SetItem(hand, drink);

    }
    
}
