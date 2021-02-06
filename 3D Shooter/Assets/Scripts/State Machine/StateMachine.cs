using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, BaseState> availableStates;

    public BaseState currentState { get; private set; }
    public event Action<BaseState> OnStateChanged;

    public void SetStates(Dictionary<Type, BaseState> states) {
        availableStates = states;
    }

    // Update is called once per frame
    void Update() {
        // check if we have set a state
        // if not set state to the first state, ChaseState
        if (currentState == null) {
            currentState = availableStates.Values.First();
        }

        // ?. make sure the currentState is not null
        // returns back the type of the new state
        var nextState = currentState?.Tick();

        if (nextState != null && nextState != currentState?.GetType()) {
            SwitchToNewState(nextState);
        }
    }

    private void SwitchToNewState(Type nextState) {
        currentState = availableStates[nextState];
        OnStateChanged?.Invoke(currentState);
    }
}
