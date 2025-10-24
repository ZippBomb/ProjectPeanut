using UnityEngine;

[System.Serializable]
public class HealthStat : Stat {

    public void Damage(float amount) {
        
        value -= amount;

        if (value <= 0.0f)
            Player.instance.Die();

    }

}