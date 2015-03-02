using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

    // Navigation
    public Transform target;
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        agent.SetDestination(target.position);
    }

    void getInBuilding() {

    }

    void chasing() {

    }

    void attacking() {

    }
}
