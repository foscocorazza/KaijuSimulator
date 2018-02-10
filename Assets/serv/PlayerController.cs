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

    public float delayVal = 0.01f;
    public float legMovSpeed = 360;
    public GameObject[] P1Weapons;
    public GameObject[] P2Weapons;

    private float p1LegAngle;
    private float p1ArmAngle;
    private float p2LegAngle;
    private float p2ArmAngle;

    private int p1WeaponsN;
    private int p2WeaponsN;
    private GameObject p1AssignedWeapon;
    private GameObject p2AssignedWeapon;
    private Player player1;
    private Player player2;

    void Start () {
        p1LegAngle = 0;
        p1ArmAngle = 0;
        p2LegAngle = 0;
        p2ArmAngle = 0;
        p1WeaponsN = P1Weapons.Length;
        p2WeaponsN= P2Weapons.Length;
        player1 = ReInput.players.GetPlayer(0);
        player2 = ReInput.players.GetPlayer(1);
    }

	void Update () {
        Vector2 auxVec = validateAxisVec(player1.GetAxis2D("Move Horizontal Left", "Move Vertical Left"));
        p1LegAngle = validateAngle(Mathf.LerpAngle(p1LegAngle, Vector2.SignedAngle(Vector2.right, auxVec)+90,delayVal));

        auxVec = validateAxisVec(player1.GetAxis2D("Move Horizontal Right", "Move Vertical Right"));
        //p1ArmAngle = validateAngle(Mathf.LerpAngle(p1ArmAngle, Vector2.SignedAngle(Vector2.right, auxVec) + 90, delayVal));
        p2LegAngle = validateAngle(Mathf.LerpAngle(p2LegAngle, Vector2.SignedAngle(Vector2.right, auxVec)+90, delayVal));

        /*auxVec = validateAxisVec(player2.GetAxis2D("Move Horizontal Left", "Move Vertical Left"));
        p2LegAngle = validateAngle(Mathf.LerpAngle(p2LegAngle, Vector2.SignedAngle(Vector2.right, auxVec)+90, delayVal));


        auxVec = validateAxisVec(player2.GetAxis2D("Move Horizontal Right", "Move Vertical Right"));
        p2ArmAngle = validateAngle(Mathf.LerpAngle(p2ArmAngle, Vector2.SignedAngle(Vector2.right, auxVec)+90, delayVal));
        */

        if (Input.GetKeyDown(KeyCode.K)) {
           // updateWeapons(0.5f, 0.5f);
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

    public void updateWeapons(float p1Weapon, float p2Weapon) {
        for (int i = 1; i <= p1WeaponsN; i++) {
            if (p1WeaponsN <= i/(float)p1WeaponsN) {
                p1AssignedWeapon = Instantiate(P1Weapons[i - 1], this.transform);
                break;
            }
        }
        for (int i = 1; i <= p2WeaponsN; i++) {
            if (p2WeaponsN <= i / (float)p2WeaponsN) {
                p2AssignedWeapon = Instantiate(P2Weapons[i - 1], this.transform);
                break;
            }
        }
        p1AssignedWeapon.GetComponent<HingeJoint>().connectedBody = P1Hand;
        p2AssignedWeapon.GetComponent<HingeJoint>().connectedBody = P2Hand;
    }

}
