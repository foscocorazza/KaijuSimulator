using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour {

    public BossJointController P1Arm;
    public BossJointController P1Leg;
    public Rigidbody P1Hand;
    public BossJointController P2Arm;
    public BossJointController P2Leg;
    public Rigidbody P2Hand;
    public bool isJumping;

    //P1 Leg Arm , P2 Leg Arm
    public float[] delayValues = new float []{0.1f, 0.1f, 0.1f, 0.1f };

    private float p1LegAngle;
    private float p1ArmAngle;
    private float p2LegAngle;
    private float p2ArmAngle;
    
    private GameObject p1AssignedWeapon;
    private GameObject p2AssignedWeapon;
    private Player player1;
    private Player player2;
    private Transform myHip;

    void Start () {
        p1LegAngle = 0;
        p1ArmAngle = 0;
        p2LegAngle = 0;
        p2ArmAngle = 0;
        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);
        isJumping = true;
        myHip = transform.GetChild(0).transform;
    }

	void Update () {
        Vector2 auxVec = validateAxisVec(player1.GetAxis2D("Move Horizontal Left", "Move Vertical Left"));
        p1LegAngle = validateAngle(Mathf.LerpAngle(p1LegAngle, Vector2.SignedAngle(Vector2.down, auxVec),delayValues[0]));

        auxVec = validateAxisVec(player1.GetAxis2D("Move Horizontal Right", "Move Vertical Right"));
        p1ArmAngle = validateAngle(Mathf.LerpAngle(p1ArmAngle, Vector2.SignedAngle(Vector2.right, auxVec), delayValues[1]));
        //p2LegAngle = validateAngle(Mathf.LerpAngle(p2LegAngle, Vector2.SignedAngle(Vector2.down, auxVec), delayValues[2]));

        auxVec = validateAxisVec(player2.GetAxis2D("Move Horizontal Left", "Move Vertical Left"));
        p2LegAngle = validateAngle(Mathf.LerpAngle(p2LegAngle, Vector2.SignedAngle(Vector2.down, auxVec), delayValues[2]));
        
        auxVec = validateAxisVec(player2.GetAxis2D("Move Horizontal Right", "Move Vertical Right"));
        p2ArmAngle = validateAngle(Mathf.LerpAngle(p2ArmAngle, Vector2.SignedAngle(Vector2.left, auxVec), delayValues[3]));

        if (myHip.localPosition.y > 0.1f) {
            isJumping = true;
        }

        updateJointValues();
    }

    private float validateAngle(float anAngle) {
        return anAngle >= 180 ? anAngle - 360 : anAngle <= -180 ? anAngle + 360 : anAngle;
    }

    private Vector2 validateAxisVec(Vector2 vec) {
        if (vec.x == 0 && vec.y == 0) {
            vec = Vector2.down;
        }
        return vec;
    }

    private void updateJointValues() {
        P1Arm.updateTarget(p1ArmAngle);
        P1Leg.updateTarget(p1LegAngle);
        P2Arm.updateTarget(p2ArmAngle);
        P2Leg.updateTarget(p2LegAngle);
    }

}
