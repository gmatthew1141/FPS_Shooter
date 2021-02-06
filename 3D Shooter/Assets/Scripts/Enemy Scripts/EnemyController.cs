using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    private GameSetting gameSetting;
    private EnemyStatus enemyStatus;
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private Transform target;
    private bool attacking;
    private float timer = 0f;

    private void Awake() {
        gameSetting = FindObjectOfType<GameSetting>();
        stateMachine = GetComponent<StateMachine>();
        enemyStatus = GetComponent<EnemyStatus>();
        agent = GetComponent<NavMeshAgent>();
        target = gameSetting.player;
        InitializeStateMachine();
    }

    private void InitializeStateMachine() {
        Dictionary<Type, BaseState> states;

        states = new Dictionary<Type, BaseState>() {
                {typeof(ChaseState), new ChaseState(this) },
                {typeof(CombatState), new CombatState(this) },
                {typeof(DeadState), new DeadState(this) }
            };

        GetComponent<StateMachine>().SetStates(states);
    }

    public void Attack(int mode) {
        var animator = enemyStatus.GetAnimator();
        var enemyType = enemyStatus.GetEnemyType();

        if (enemyType == EnemyType.SKELETON_AXE || enemyType == EnemyType.ARMORED_SKELETON_AXE) {
            if (mode == 0) {
                if (!attacking) {
                    attacking = true;

                    animator.SetBool("Attacking", true);
                    animator.Play("Attack00");
                }
            } else {
                animator.SetBool("Attacking", false);
                attacking = false;
            }
        } else if (enemyType == EnemyType.SKELETON_BOMBER || enemyType == EnemyType.ARMORED_SKELETON_BOMBER) {
            Debug.Log("Bomber attacking");
            // play explotion particle system
            var particle = transform.GetChild(0);
            particle.gameObject.SetActive(true);
            // damage player in radius
            if (Vector3.Distance(target.position, transform.position) <= gameSetting.bombRadius) {
                target.GetComponent<Status>().TakeDamage(gameSetting.bomberDamage, gameSetting.bomberKnockbackForce);
                enemyStatus.Dead();
            }
        } else {
            Transform throwPoint1 = transform.GetChild(0);
            Transform throwPoint2 = transform.GetChild(1);

            GameObject throwingAxe = gameSetting.throwingAxePrefab;


            if (timer <= 0 && !attacking) {
                // play attack animation
                animator.Play("Attack02");
                animator.SetBool("Waiting", false);

                attacking = true;

                StartCoroutine(ThrowAxe(throwingAxe, throwPoint1, throwPoint2, animator));
                
            } else {
                animator.SetBool("Waiting", true);
                timer -= Time.deltaTime;
            }
        }
    }


    private IEnumerator ThrowAxe(GameObject throwingAxe, Transform throwPoint1, Transform throwPoint2, Animator animator) {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Enum ThrowAxe is called");
        var axe1 = Instantiate(throwingAxe, throwPoint1.position, throwPoint1.rotation);
        var axe2 = Instantiate(throwingAxe, throwPoint2.position, throwPoint2.rotation);

        var axe1Script = axe1.GetComponent<ThrowingAxeScript>();
        var axe2Script = axe2.GetComponent<ThrowingAxeScript>();

        axe1Script.SetAxedamage(gameSetting.throwingAxeDamage);
        axe2Script.SetAxedamage(gameSetting.throwingAxeDamage);

        axe1Script.ThrowForward(transform.rotation);
        axe2Script.ThrowForward(transform.rotation);

        timer = 2f;

        attacking = false;
    }

    public EnemyStatus GetEnemyStatus() {
        return enemyStatus;
    }

    public NavMeshAgent GetNavAgent() {
        return agent;
    }

    public Transform GetTarget() {
        return target;
    }

    public Animator GetEnemyAnimator() {
        return enemyStatus.GetAnimator();
    }

    public void SetAttacking(bool attacking) {
        this.attacking = attacking;
    }

    /**
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, gameSetting.bombRadius);
    }
    **/
}
