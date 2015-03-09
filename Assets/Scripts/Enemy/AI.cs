using UnityEngine;
using System.Collections;

public class AI : Character {

    public static WaveManager waveManager;

    // Navigation
    public Player player;
    public Transform barricadeTarget;
    private Transform playerTarget;
    private Transform target;
    private NavMeshAgent agent;
    private float defaultStoppingDistance;

    // AI
    private enum State { TargetBarricade, AttackBarricade, GetInside, ChasePlayer, AttackPlayer }
    private State state;
    private Barricade barricade; // the barricade the AI will target/attack
    private bool isInside = false; // true if the AI has gotten passed the barricade already
    private bool stop = false;
    private bool paused = false;

    // Attack
    public float attackPower = 20f;
    public float attackSpeed = 3f;
    private float timeToAttack = 0f;
    private float attackDistance;
    private float attackOffset = 1.5f;
    private float moveSpeed;

    // Animation
    private Animator anim;
    int speedHash = Animator.StringToHash("Speed");

    public float initialHealth = 100f;

    void Start() {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        playerTarget = player.transform;
        TransitionTargetBarricade();
        defaultStoppingDistance = agent.stoppingDistance;
        attackDistance = defaultStoppingDistance;
        health = initialHealth;
        InvokeRepeating("StateMachine", 0f, .3f);
    }

    public void Init(Player player, Transform barricadeTarget, float moveSpeed) {
        this.player = player;
        this.barricadeTarget = barricadeTarget;
        this.moveSpeed = moveSpeed;
        this.gameObject.SetActive(true);
    }

    void StateMachine() {
        if (GameEngine.paused) return;
        switch (state) {
            case State.TargetBarricade:
                TargetBarricade();
                break;

            case State.AttackBarricade:
                AttackBarricade();
                break;

            case State.GetInside:
                GetInside();
                break;

            case State.ChasePlayer:
                ChasePlayer();
                break;

            case State.AttackPlayer:
                AttackPlayer();
                break;
        }

        anim.SetFloat(speedHash, agent.velocity.magnitude);

        agent.SetDestination(target.position);
    }

    void Update() {
        if (GameEngine.paused && !paused) {
            agent.Stop();
            paused = true;
        } else if (!GameEngine.paused && paused) {
            agent.Resume();
            paused = false;
        }

        //if (stop || (state.Equals(State.ChasePlayer) && agent.remainingDistance < attackDistance + attackOffset)) {
        //    agent.Stop();
        //}
    }

    void OnTriggerEnter(Collider other) {
        switch (other.tag) {
            case "Barricade":
                if (isInside) return;
                barricade = other.GetComponent<Barricade>();
                if (barricade.transform != barricadeTarget) { // wrong target
                    agent.ResetPath(); // recompute path
                    return;
                }
                if (barricade.Destroyed) {
                    TransitionChasePlayer();
                } else {
                    TransitionAttackBarricade();
                }
                break;
            case "InsideTrigger":
                if (isInside) return;
                isInside = true;
                TransitionChasePlayer();
                break;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Barricade")) {
            if (isInside) return;
            else if (barricade == null) TransitionTargetBarricade();
            else if ((state.Equals(State.AttackBarricade) == !barricade.Destroyed)) return;
            else if ((state.Equals(State.ChasePlayer))) TransitionAttackBarricade();
            else if (barricade.Destroyed) TransitionGetInside();
        } else if (other.CompareTag("OutsideTrigger")) {
            if (barricade == null) return;
            if (state.Equals(State.GetInside) && !barricade.Destroyed) {
                TransitionAttackBarricade();
            }

        }
    }

    void OnTriggerExit(Collider other) {
        switch (other.tag) {
            case "Barricade":
                barricade = null;
                break;
        }
    }

    void TransitionChasePlayer() {
        stop = false;
        agent.Resume();
        state = State.ChasePlayer;
        agent.stoppingDistance = defaultStoppingDistance;
        target = playerTarget;
    }

    void TransitionTargetBarricade() {
        state = State.TargetBarricade;
        agent.stoppingDistance = defaultStoppingDistance;
        target = barricadeTarget;
    }

    void TransitionGetInside() {
        state = State.GetInside;
        agent.stoppingDistance = 0f;
        target = barricade.insideTrigger.transform;
    }

    void TransitionAttackBarricade() {
        state = State.AttackBarricade;
        agent.stoppingDistance = defaultStoppingDistance;
        target = barricadeTarget;
    }

    void TransitionAttackPlayer() {
        stop = true;
        agent.Stop();
        state = State.AttackPlayer;
        agent.stoppingDistance = defaultStoppingDistance;
        target = playerTarget;
    }

    void TargetBarricade() {
        target = barricadeTarget;
    }

    void AttackBarricade() {
        if (barricade == null) return;
        if (timeToAttack < Time.time) {
            Attack();
            StartCoroutine(DelayedBreak());
        }
    }

    void GetInside() {
    }

    void ChasePlayer() {
        if (isInside && agent.remainingDistance <= attackDistance + attackOffset) {
            TransitionAttackPlayer();
        }
    }

    void AttackPlayer() {
        if (agent.remainingDistance > attackDistance + attackOffset) {
            TransitionChasePlayer();
            return;
        }
        if (timeToAttack < Time.time) {
            Attack();
            StartCoroutine(DelayedAttackPlayer());
        }
        SmoothLookAtPlayer();
    }

    void Attack() {
        anim.SetTrigger("Attack");
        timeToAttack = Time.time + attackSpeed;
    }

    public override void OnDeath() {
        print(name + " OnDeath().");
        Destroy(this.gameObject);
        waveManager.ZombieDied();
    }

    void SmoothLookAtPlayer() {
        Vector3 pos = player.transform.position - agent.transform.position;
        Quaternion newRot = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 5f);
    }

    IEnumerator DelayedBreak() {
        yield return StartCoroutine(Wait(1.2f));
        if (barricade != null) barricade.Break();
    }

    IEnumerator DelayedAttackPlayer() {
        yield return StartCoroutine(Wait(1.2f));
        if (state.Equals(State.AttackPlayer)) player.Damage(attackPower);
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }

}
