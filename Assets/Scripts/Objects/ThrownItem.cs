using UnityEngine;

public class ThrownItem : Stareable {

    public Item item;
    
    public override void Interact() {
        
        if (!isStaredAt) return;

        if (Player.instance.GetLeftItem() == null)
            Player.instance.SetLeftItem(item);
        else if (Player.instance.GetRightItem() == null)
            Player.instance.SetRightItem(item);
        else return;

        Destroy(gameObject);

    }

}