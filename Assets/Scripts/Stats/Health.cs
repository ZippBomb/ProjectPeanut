using UnityEngine;

[System.Serializable]
public class HealthStat : Stat {

    public float test = 3.0f;

    public override void Update() {

        base.Update();

        if (Time.time < test) return;

        value -= 10.0f;
        test += 3.0f;

    }

}