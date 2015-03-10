using UnityEngine;
using System.Collections;

public class IK : MonoBehaviour {

    private Animator anim;
    private Gun gun;
    public static float ikLeftHandWeight;

    void Start() {
        anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex) {
        //float weight = Mathf.Lerp(anim.GetIKPositionWeight(AvatarIKGoal.LeftHand), ikLeftHandWeight, Time.deltaTime * 5f);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikLeftHandWeight);
        anim.SetIKPosition(AvatarIKGoal.LeftHand, gun.IK_LEFT_HAND.position);
    }

    public Gun Gun {
        set { gun = value; }
    }
}
