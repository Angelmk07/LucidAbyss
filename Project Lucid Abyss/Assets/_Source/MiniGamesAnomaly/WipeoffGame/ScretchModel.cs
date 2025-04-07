using UnityEngine;
public class ScretchModel
{
    public float CounterForce { get; private set; }
    public float NeedMoreThan { get; private set; }
    public float HoldTime { get; private set; }

    public float Timer { get; private set; }
    public bool IsTimerRunning { get; private set; }

    public bool IsCompleted => Timer >= HoldTime;
    public float TimeLeft => Mathf.Max(HoldTime - Timer, 0f);

    public void Initialize(float counterForce, float needMoreThan, float holdTime)
    {
        CounterForce = counterForce;
        NeedMoreThan = needMoreThan;
        HoldTime = holdTime;
        Reset();
    }

    public void TryStartTimer(float percent)
    {
        if (!IsTimerRunning && percent > NeedMoreThan)
        {
            IsTimerRunning = true;
            Timer = 0f;
        }
    }

    public void UpdateTimer(float deltaTime)
    {
        if (IsTimerRunning)
        {
            Timer += deltaTime;
        }
    }

    public void Reset()
    {
        Timer = 0f;
        IsTimerRunning = false;
    }
}

