using UnityEngine;
using System.Collections;

public class IK : MonoBehaviour {

    private Animator anim;
    public static Gun gun;
    public static float ikLeftHandWeight;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex) {
        if (gun == null) return;
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.IK_LEFT_HAND.position);
    }

}
