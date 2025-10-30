using UnityEngine;

[System.Serializable]
public class HealthStat : Stat {

    private float lastEatenTime = 0.0f;
    private float healDuration = 0.0f;

    public override void Update() {

        if (Time.time >= lastEatenTime + healDuration) return;

        value += replenishRate * Time.deltaTime;
        value = Mathf.Clamp(value, 0.0f, maxValue);

        UpdateIndicator();
        
    }

    public void Damage(float amount) {
        
        value -= amount;

        if (value <= 0.0f)
            Player.instance.Die();

    }

    public void OnEaten(float satiety) {
        
        lastEatenTime = Time.time;
        healDuration = satiety / 2.0f;

    }

}
