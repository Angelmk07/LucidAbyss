using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Anomaly",menuName = "CreateAnomaly")]
public class AnomalySO : ScriptableObject
{
    [field:SerializeField] public Sprite Sprite { private set; get; }
    [field: SerializeField] public Sprite AnomalySprite { private set; get; }
    [field: SerializeField] public float TimeToScretch { private set; get; }
    [field: SerializeField] public float NeedPowerMoreThan { private set; get; }
    [field: SerializeField] public float CounterForce { private set; get; }
    [field: SerializeField] public int[] LvlAccess { private set; get; }
    

}
