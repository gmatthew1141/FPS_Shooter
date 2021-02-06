using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState {

    public BaseState(GameObject gameObject) {
        EnemyController eController = gameObject.GetComponent<EnemyController>();
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
        this.agent = eController.GetNavAgent();
        this.enemyStatus = eController.GetEnemyStatus();
    }

    protected GameObject gameObject;
    protected Transform transform;
    protected NavMeshAgent agent;
    protected EnemyStatus enemyStatus;

    public abstract Type Tick();
}
