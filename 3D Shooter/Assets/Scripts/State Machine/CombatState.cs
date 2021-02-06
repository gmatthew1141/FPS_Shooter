using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CombatState : BaseState {

    private EnemyController eController;

    public CombatState(EnemyController eController) : base(eController.gameObject) {
        this.eController = eController;
    }

    public override Type Tick() {
        Debug.Log("Enter combat state");

        if (enemyStatus.IsDead()) {
            return typeof(DeadState);
        }

        var enemyType = enemyStatus.GetEnemyType();
        float checkRange = 0;

        if (enemyType == EnemyType.SKELETON_AXE || enemyType == EnemyType.ARMORED_SKELETON_AXE) {
            checkRange = 4f;
        } else if (enemyType == EnemyType.SKELETON_BOMBER || enemyType == EnemyType.ARMORED_SKELETON_BOMBER) {
            checkRange = 7f;
        } else {
            checkRange = 30f;
        }

        var inRange = CheckForRange(checkRange);
        var animator = eController.GetEnemyAnimator();
        var target = eController.GetTarget();

        // Agent cannot move while attacking
        agent.isStopped = true;
        
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));


        if (inRange) {
            // player in range, attack
            eController.Attack(0);
        } else {
            // player not in range, reset attack param and enter chase state
            eController.Attack(1);
            return typeof(ChaseState);
        }
        
        return null;
    }

    private Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
    private Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

    private bool CheckForRange(float range) {

        RaycastHit hit;
        var angle = transform.rotation * startingAngle;
        var direction = angle * Vector3.forward;
        var pos = new Vector3(transform.position.x, 1.7f, transform.position.z);

        //create 24 lines to emulate NPC's vision
        for (var i = 0; i < 24; i++) {
            if (Physics.Raycast(pos, direction, out hit, range, 1 << LayerMask.NameToLayer("Player"))) {
                var inRange = hit.collider.tag == "Player";
                
                if (inRange) {
                    Debug.DrawRay(pos, direction * hit.distance, Color.red);                        // create red ray if interacts with npc/player
                    return inRange;
                } else {
                    Debug.DrawRay(pos, direction * hit.distance, Color.yellow);                     // yellow ray if interact with environment
                }
            } else {
                Debug.DrawRay(pos, direction * range, Color.white);                           // white ray if there's no obstacle infront
            }

            direction = stepAngle * direction;
        }

        return false;
    }

}
