using System;
using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    ResolutionPhase
}

public class TurnManager : MonoBehaviour
{
    public TurnState CurrentState { get; private set; }

    // Event System
    public static event Action<TurnState> OnTurnBegin;
    public static event Action<TurnState> OnTurnEnd;

    void Start()
    {
        ChangeState(TurnState.PlayerTurn);
    }

    public void ChangeState(TurnState newState)
    {
        // End current turn if it's changing, except at the very start
        if (CurrentState != newState || CurrentState == TurnState.PlayerTurn) 
        {
            OnTurnEnd?.Invoke(CurrentState);

            CurrentState = newState;
            Debug.Log($"Starting Phase: {CurrentState}");

            OnTurnBegin?.Invoke(CurrentState);
        }
    }

    public void EndTurn()
    {
        if (CurrentState == TurnState.PlayerTurn)
        {
            ChangeState(TurnState.EnemyTurn);
        }
        else if (CurrentState == TurnState.EnemyTurn)
        {
            ChangeState(TurnState.ResolutionPhase);
        }
        else if (CurrentState == TurnState.ResolutionPhase)
        {
            ChangeState(TurnState.PlayerTurn);
        }
    }
}
