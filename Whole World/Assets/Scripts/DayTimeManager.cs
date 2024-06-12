using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class DayTimeManager : MonoBehaviour {
    public enum DaytimeType {
        MORNING,
        NOON,
        PRE_NIGHT,
        NIGHT
    };

    public Light sunLight;
    public float daylength = 30; // in secounds

    private DaytimeType daytime;
    private float time_daystart;
    private float time_current;

    public delegate void DaytimeChangedEvemt(DaytimeType daytimeType);
	public event DaytimeChangedEvemt onDaytimeChanged;

    void Start() {
        time_current = daylength * 0.35f; // start on bright day
    }

    void Update() {
        time_current += Time.deltaTime;

        if (time_current >= daylength) {
            time_current = 0;
        }

        calcDaytime();
        updateSunColor();

        // rotate sun
        sunLight.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 360.0f * (time_current / daylength) ));
    }

    private void calcDaytime() {
        if (time_current <= daylength * 0.2f && daytime != DaytimeType.NIGHT) {
            setDaytime(DaytimeType.NIGHT);
        } else if (time_current <= daylength * 0.3f && daytime != DaytimeType.MORNING) {
            setDaytime(DaytimeType.MORNING);
        } else if (time_current <= daylength * 0.8f && daytime != DaytimeType.NOON) {
            setDaytime(DaytimeType.NOON);
        } else if (time_current <= daylength * 0.85f && daytime != DaytimeType.PRE_NIGHT) {
            setDaytime(DaytimeType.PRE_NIGHT);
        } else if (daytime != DaytimeType.NIGHT) {
            setDaytime(DaytimeType.NIGHT);
        }
    }

    private void setDaytime(DaytimeType dtime) {
        daytime = dtime;

        updateSunColor();

        // fire datetime change event
        if (onDaytimeChanged != null)
			onDaytimeChanged(daytime);
    }

    private void updateSunColor() {
        Color sunColor;

        if (daytime == DaytimeType.MORNING) {
            sunColor = new Color(0.6f, 0.3f, 0.3f);
        } else if (daytime == DaytimeType.NOON) {
            sunColor = new Color(1.0f, 0.9f, 1.0f);
        } else if (daytime == DaytimeType.PRE_NIGHT) {
            sunColor = new Color(1.0f, 0.6f, 0.3f);
        } else {
            sunColor = new Color(0.0f, 0.0f, 0.3f);
        }
        sunLight.color = sunColor;
    }
}