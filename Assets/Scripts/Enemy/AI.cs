using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

    // Self
    //private 

    // Navigation
    public Transform target;
    private NavMeshAgent agent;

    // AI
    private enum AIState { TargetBarricade, AttackBarricade, ChasePlayer, AttackPlayer }
    private AIState state;
    private Barricade barricade; // the barricade the AI will target/attack
    private bool isInside = false; // true if the AI has gotten passed the barricade already

    // Attack
    public float attackSpeed = 3f;
    private float timeToAttack = 0f;

    // Animation
    private Animator anim;


    void Start() {
        anim = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        state = AIState.TargetBarricade;
    }

    void Update() {
        agent.SetDestination(target.position);
        switch (state) {
            case AIState.TargetBarricade:
                TargetBarricade();
                break;

            case AIState.AttackBarricade:
                AttackBarricade();
                break;

            case AIState.ChasePlayer:
                ChasePlayer();
                break;

            case AIState.AttackPlayer:
                AttackPlayer();
                break;
        }

        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    void OnTriggerEnter(Collider other) {
        print("AI: Enter " + other.name);
        switch (other.tag) {
            case "Barricade":
                if (isInside) return;
                barricade = other.GetComponent<Barricade>();
                if (barricade.Destroyed) {
                    state = AIState.ChasePlayer;
                } else {
                    state = AIState.AttackBarricade;
                }
                break;
        }
    }

    void OnTriggerStay(Collider other) {
        if (other.CompareTag("Barricade")) {
            if (isInside) return;
            if (barricade == null || ((state.Equals(AIState.AttackBarricade) == !barricade.Destroyed))) return;
            if (barricade.Destroyed)
                state = AIState.ChasePlayer;
        }
    }

    void OnTriggerExit(Collider other) {
        print("AI: Exit " + other.name);
        switch (other.tag) {
            case "Barricade":
                barricade = null;
                isInside = true;
                break;
        }
    }

    void TargetBarricade() {
        //print("Target Barricade");
    }

    void AttackBarricade() {
        //print("Attack Barricade");
        if (timeToAttack < Time.time) {
            Attack();
            StartCoroutine(DelayedBreak());
        }
    }

    void ChasePlayer() {
        //print("Chase Player");
    }

    void AttackPlayer() {
        //print("Attack Player");
    }

    void Attack() {
        anim.SetTrigger("Attack");
        timeToAttack = Time.time + attackSpeed;
    }

    IEnumerator DelayedBreak() {
        yield return StartCoroutine(Wait(1.2f));
        barricade.Break();
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }

    //public AIState State {
    //    get { return state; }
    //}

}
