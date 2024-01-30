using System.Collections;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    private bool active = false;
    private float time = 0;
    private float currentTime = 0;

    public bool Active { get => active; set => active = value; }

    private Action onTimeOver;


    // Static method for create a new timer instance
    public static Timer CreateNewTimer(float time, GameObject invoker, Action callback, bool init = true)
    {
        // Create a new instance and add to the invoker of this timer
        var timerContainer = new GameObject("Timer_Untaged");
        timerContainer.transform.SetParent(FindParent(invoker.transform));

        var timer = timerContainer.AddComponent<Timer>();
        timer.onTimeOver = callback;
        timer.time = time;

        // Init the timer
        return init ? timer.Init(time, callback) : timer;
    }

    public static void CreateOrRestartTimer(ref Timer timer, float time, GameObject invoker, Action callback, bool init = true) {
        if (!timer) {
            timer = CreateNewTimer(time, invoker, callback, init);
        }
        else timer.RestartTimer();
    }

    private void Update()
    {
        if (active && currentTime < time)
        {
            currentTime = Mathf.Clamp(currentTime + Time.deltaTime, 0, time);
        }
    }

    public static Timer CreateNewTimer(float time, GameObject invoker, string timerName, Action callback, bool init = true)
    {
        // Create a new instance and add to the invoker of this timer
        var timerContainer = new GameObject($"Timer_{timerName}");
        timerContainer.transform.SetParent(FindParent(invoker.transform));

        var timer = timerContainer.AddComponent<Timer>();
        
        timer.onTimeOver = callback;
        timer.time = time;

        // Init the timer
        return init ? timer.Init(time, callback) : timer;

    }

    static Transform FindParent(Transform invoker)
    {
        for (int i = 0; i < invoker.childCount; i++)
        {
            if (invoker.GetChild(i).name == "TimerContainer")
            {
                return invoker.GetChild(i);
            }
        }
        var container = new GameObject("TimerContainer");
        container.transform.SetParent(invoker);
        return container.transform;
    }

    // Init method for initialize the timer with a coroutine
    Timer Init(float time, Action callback)
    {
        this.currentTime = 0;
        this.time = time;
        active = true;

        StartCoroutine(TimerCoroutine());
        return this;
    }

    public Timer RestartTimer(float time, Action callback = null)
    {
        if (callback != null) callback();

        active = true;
        return Init(time, this.onTimeOver);
    }

    public void Stop() {
        StopAllCoroutines();
    }

    public Timer RestartTimer(Action callback = null)
    {
        if (callback != null) callback();

        active = true;
        return Init(time, this.onTimeOver);
    }

    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(time);

        onTimeOver();
        active = false;

    }
}
