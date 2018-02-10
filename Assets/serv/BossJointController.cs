using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJointController : MonoBehaviour {

    private HingeJoint myHinge;
    private JointSpring mySpring;

    void Awake() {
        myHinge = GetComponent<HingeJoint>();
        mySpring = myHinge.spring;
    }

    public void updateTarget(float newAngle) {
        //Formatting the angle
        if (newAngle > 180) {
            newAngle -= 360;
        }
        newAngle = Mathf.Clamp(newAngle, myHinge.limits.min, myHinge.limits.max);
        mySpring.targetPosition = newAngle;
        myHinge.spring = mySpring;
    }
}
