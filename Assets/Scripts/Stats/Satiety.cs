using UnityEngine;

[System.Serializable]
public class SatietyStat : Stat {

    private float lowSatietyThreshold = 0.3f;

    private float damage = 3.0f;
    private float damageIntervalMin = 2.0f;
    private float damageIntervalMax = 6.0f;

    private float nextDamageTime = 0.0f;

    public override void Update() {

        base.Update();

        if (value / maxValue > lowSatietyThreshold) return;
        if (Time.time < nextDamageTime) return;

        HealthStat health = (HealthStat) Player.GetStat(Stat.Type.Health);
        Game.Assert(health != null, "Could not get health stat.");

        health.Damage(damage);

        nextDamageTime = Time.time + Random.Range(damageIntervalMin, damageIntervalMax);

    }

}
