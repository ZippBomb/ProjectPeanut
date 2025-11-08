using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class Fridge : Stareable {

    [SerializeField] private float spawnInterval = 10.0f;
    [SerializeField] private List<Item> spawnables = new List<Item>();

    [SerializeField] private List<Item> inStock = new List<Item>();
    [SerializeField] private int capacity;

    private float lastSpawnTime = 0.0f;

    private void Start() {

        Game.input.Player.Interact.performed += Interact;

    }
    private void Update() {

        if (Time.time < lastSpawnTime + spawnInterval) return;
        if (inStock.Count >= capacity) return;

        int index = Random.Range(0, spawnables.Count);
        inStock.Add(spawnables[index]);

        lastSpawnTime = Time.time;

    }

    private void Interact(InputAction.CallbackContext ctx) {

        if (inStock.Count == 0) return;

        Item item = inStock[0];
        if (Player.instance.GetLeftItem() == null)
            Player.instance.SetLeftItem(item);
        else if (Player.instance.GetRightItem() == null)
            Player.instance.SetRightItem(item);
        else return;

        inStock.RemoveAt(0);

    }

}
