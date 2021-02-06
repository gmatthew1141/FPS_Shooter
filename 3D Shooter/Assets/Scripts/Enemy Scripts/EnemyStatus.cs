using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatus : Status {

    [SerializeField] private float destroyAfterDead;
    [SerializeField] private EnemyType enemyType;
    private Animator animator;
    private EnemyController eController;
    private Rigidbody enemyRB;
    private NavMeshAgent agent;
    private GameManager gameManager;
    private GameSetting gameSetting;
    private float pointsWorth;

    private void Awake() {
        gameSetting = FindObjectOfType<GameSetting>();
    }

    private void Start() {
        eController = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (health <= 0) {
            if (!dead) {
                Dead();
            }
        }
    }

    public override void TakeDamage(float damage, float knockbackForce) {
        // play take damage animation
        int rand = Random.Range(0, 10);
        eController.SetAttacking(false);
        animator.SetBool("Attacking", false);

        //enemyRB.AddForce(Vector3.forward * knockbackForce);
        if (!dead) {
            if (rand < 5) {
                animator.Play("Damage00");
            } else {
                animator.Play("Damage01");
            }
        }

        health -= damage;
        // decrease health bar
    }

    public override void Dead() {
        Debug.Log("Enemy is dead");
        dead = true;
        animator.Play("Dead00");
        gameManager.ReportDead(gameObject);
        Invoke("DestroyEnemyAfterDead", destroyAfterDead);
    }

    private void DestroyEnemyAfterDead() {
        // Wait for set amount of time
        // Destroy enemy corpse
        Destroy(gameObject);
    }

    public Animator GetAnimator() {
        return animator;
    }

    public EnemyType GetEnemyType() {
        return enemyType;
    }

    public GameSetting GetGameSetting() {
        return gameSetting;
    }
}
