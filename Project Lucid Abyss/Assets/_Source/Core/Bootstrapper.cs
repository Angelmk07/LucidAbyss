using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private InteractionSystem interaction;
    [SerializeField] private ScretchController scretch;
    [SerializeField] private BallSpawner ballSpawner;
    [SerializeField] private LockPickingGame lockPicking;

    private void Awake()
    {
        interaction.Constructor(scretch, gameTimer, ballSpawner,lockPicking) ;
    }
}
