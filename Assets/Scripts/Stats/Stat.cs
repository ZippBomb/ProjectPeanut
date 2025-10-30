using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stat {

    public enum Type {

        Health = 0,
        Satiety,
        Thirst,
        Exhaustion,
        Toilet,

    }

    public string name = "Stat";
    public float value = 100.0f;
    public float maxValue = 100.0f;
    public float replenishRate = 1.0f;

    [Header("Indicator")]
    public Color fillColor = Color.grey;
    public Color backgroundColor = Color.black;

    private Slider indicator;

    public void HookUI(GameObject indicator) {

        Game.Assert(indicator.GetComponent<Slider>() != null, "Stat HookUI indicator parameter does not have a slider component.");

        this.indicator = indicator.GetComponent<Slider>();
        this.indicator.value = value / maxValue;

        Game.Assert(this.indicator.fillRect.GetComponent<Image>() != null, "Stat indicators fill rect does not have an Image component.");
        Game.Assert(this.indicator.transform.GetChild(0).GetComponent<Image>() != null, "Stat indicators first child does not have an Image component.");

        this.indicator.fillRect.GetComponent<Image>().color = fillColor;
        this.indicator.transform.GetChild(0).GetComponent<Image>().color = backgroundColor;

    }

    public virtual void Update() {

        value += replenishRate * Time.deltaTime;
        value = Mathf.Clamp(value, 0.0f, maxValue);

        UpdateIndicator();

    }

    public virtual void Replenish(float amount) {

        value += amount;
        value = Mathf.Clamp(value, 0.0f, maxValue);

        UpdateIndicator();

    }

    protected void UpdateIndicator() {

        indicator.value = value / maxValue;

    }

}