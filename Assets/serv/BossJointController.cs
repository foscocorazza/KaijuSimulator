using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJointController : MonoBehaviour {

    private HingeJoint myHinge;
    private JointSpring mySpring;
    void Awake() {
        myHinge = GetComponent<HingeJoint>();
    }

    public void updateTarget(float newAngle) {
        //Formatting the angle
        if (newAngle > 180) {
            newAngle -= 360;
        }
        mySpring = myHinge.spring;
        newAngle = Mathf.Clamp(newAngle, myHinge.limits.min, myHinge.limits.max);
        mySpring.targetPosition = newAngle;
        myHinge.spring = mySpring;
    }
}
