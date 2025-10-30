using UnityEngine;

[System.Serializable]
public class HealthStat : Stat {

    private float lastEatenTime = 0.0f;
    private float healDuration = 0.0f;

    private bool lowHealth = false;

    public override void Update() {

        if (!lowHealth && value < maxValue / 2.0f) {

            lowHealth = true;

            PlayerMovement.instance.speed /= 2.0f;
            PlayerLook.instance.sensitivity /= 2.0f;

        } else if (lowHealth && value >= maxValue / 2.0f) {

            lowHealth = false;

            PlayerMovement.instance.speed *= 2.0f;
            PlayerLook.instance.sensitivity *= 2.0f;

        }

        if (Time.time >= lastEatenTime + healDuration) return;

        value += replenishRate * Time.deltaTime;
        value = Mathf.Clamp(value, 0.0f, maxValue);

        UpdateIndicator();
        
    }

    public void Damage(float amount) {
        
        value -= amount;

        if (value <= 0.0f)
            Player.instance.Die();

        UpdateIndicator();

    }

    public void OnEaten(float satiety) {
        
        lastEatenTime = Time.time;
        healDuration = satiety / 2.0f;

    }

}
