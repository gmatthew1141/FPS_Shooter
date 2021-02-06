using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DeadState : BaseState
{

    private EnemyController eController;

    public DeadState(EnemyController eController) : base(eController.gameObject) {
        this.eController = eController;
    }

    public override Type Tick() {
        Debug.Log(eController.transform.name + " Enter dead state");
        
        return null;
    }

}
