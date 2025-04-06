using UnityEngine;

public class ScretchModel
{
    public float CounterForce { get; private set; }
    public float NeedMoreThan { get; private set; }
    public float HoldTime { get; private set; }

    public float Timer { get; private set; }
    public bool IsHolding { get; private set; }

    public bool IsCompleted => Timer >= HoldTime;

    public void Initialize(float counterForce, float needMoreThan, float holdTime)
    {
        CounterForce = counterForce;
        NeedMoreThan = needMoreThan;
        HoldTime = holdTime;
    }

    public void UpdateHold(float percent, float deltaTime)
    {
        if (percent > NeedMoreThan)
        {
            if (!IsHolding)
            {
                Timer = 0f;
                IsHolding = true;
            }

            Timer += deltaTime;
        }
        else
        {
            Reset();
        }
    }

    public void Reset()
    {
        Timer = 0f;
        IsHolding = false;
    }
}
