using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    float time = 0.0f;
    [SerializeField]
    float duration = 0.0f;

    public delegate void GenericDelegate();
    public GenericDelegate onTick = null;
    bool started = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(started && (time += Time.deltaTime) >= duration)
        {
            if(onTick != null)
                onTick();
            time -= duration;
        }
        if(!started)
        {
            time = 0.0f;
        }
	}

    public void StartTimer()
    {
        started = true;
    }

    public void StopTimer()
    {
        started = false;
    }

    public void RegisterOnTick(GenericDelegate tick)
    {
        onTick += tick;
    }

    public void SetDuration(float newDuration)
    {
        duration = newDuration;
    }
}
