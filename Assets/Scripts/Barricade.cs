using UnityEngine;
using System.Collections;

public class Barricade : MonoBehaviour {

    public Rigidbody[] planks;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;
    private int currentPlank = 0;
    public float repairDelay = 2f;
    private bool canRepair = true;

    void Start() {
        originalPositions = new Vector3[planks.Length];
        originalRotations = new Quaternion[planks.Length];
        for (int i = 0; i < planks.Length; i++) {
            planks[i].isKinematic = true;
            originalPositions[i] = planks[i].position;
            originalRotations[i] = planks[i].rotation;
            //print(string.Format("Plank {0}: pos={1}, rot={2}", i, originalPositions[i], originalRotations[i]));
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Repair();
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            Break();
        }
    }

    public void Break() {
        print("Barricade: Enter Break");
        if (Destroyed) { print("No Planks to break!"); return; }
        planks[currentPlank].isKinematic = false;
        planks[currentPlank].WakeUp();
        currentPlank++;
    }

    public void Repair() {
        if (!canRepair) return;
        print("Barricade: Enter Repair");
        int plankIndex = currentPlank - 1;
        if (plankIndex < 0 || plankIndex > planks.Length) { print("No Planks to repair!"); return; }
        Rigidbody plank = planks[plankIndex];
        plank.isKinematic = true;
        plank.Sleep();
        //print(plank.position + " -> original: " + originalPositions[plankIndex]);
        plank.position = originalPositions[plankIndex];
        plank.rotation = originalRotations[plankIndex];
        currentPlank = plankIndex;
        StartCoroutine(DelayRepair());
    }

    IEnumerator DelayRepair() {
        canRepair = false;
        yield return StartCoroutine(Wait(repairDelay));
        canRepair = true;
    }

    IEnumerator Wait(float duration) {
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
            yield return 0;
    }

    public bool NeedsRepair {
        get { return currentPlank > 0; }
    }

    public bool Destroyed {
        get { return currentPlank == planks.Length; }
    }

}
