using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Oven : Stareable {

    [SerializeField] private List<Cookable> cookables;

    private void Start() {

        Game.input.Player.Interact.performed += Interact;
        
    }

    private void Interact(InputAction.CallbackContext ctx) {

        if (!isStaredAt) return;
        
        foreach (Cookable cookable in cookables) {
            
            Hand hand = Hand.None;
            if (Player.instance.HasItemInLeft(cookable.input))
                hand = Hand.Left;
            else if (Player.instance.HasItemInRight(cookable.input))
                hand = Hand.Right;

            if (hand == Hand.None) continue;

            Player.instance.RemoveItem(hand);
            Player.instance.SetItem(hand, cookable.output);

            return;

        }

    }

    [System.Serializable]
    private struct Cookable {
        
        public Item input;
        public Item output;

    }
    
}