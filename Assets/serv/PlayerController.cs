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
        /*float p1AxisArmX = Input.GetAxis("P1HorizontalAxis1");
        float p1AxisArmY = Input.GetAxis("P1VerticalAxis1");
        Debug.Log(p1AxisArmX+ " " + p1AxisArmY);
        float p1AxisLegX = Input.GetAxis("P1HorizontalAxis2");
        float p1AxisLegY = Input.GetAxis("P1VerticalAxis2");
        Debug.Log(p1AxisLegX + " " + p1AxisLegY);*/

        /*        
        float p1AxisArmX = Input.GetAxis("P1HorizontalAxis1");
        float p1AxisArmY = Input.GetAxis("P1VerticalAxis1");
        Debug.Log(p1AxisArmX+ " " + p1AxisArmY);
        float p1AxisLegX = Input.GetAxis("P1HorizontalAxis2");
        float p1AxisLegY = Input.GetAxis("P1VerticalAxis2");
        Debug.Log(p1AxisLegX + " " + p1AxisLegY);
        */


        //TODO check names according to players
        Vector2 auxVec = player1.GetAxis2D("Move Horizontal Left", "Move Vertical Left");
        if (!(auxVec.x == 0 && auxVec.y ==0)) {
            p1LegAngle = Vector2.SignedAngle(Vector2.right, auxVec) +90;
            //P1LegRight.localEulerAngles = Vector3.forward * legRightAngle;
        }

        auxVec = player1.GetAxis2D("Move Horizontal Right", "Move Vertical Right");
        if (!(auxVec.x == 0 && auxVec.y == 0)) {
            p1ArmAngle = Vector2.SignedAngle(Vector2.right, auxVec) + 90;
        }

        auxVec = player2.GetAxis2D("Move Horizontal Left", "Move Vertical Left");
        if (!(auxVec.x == 0 && auxVec.y == 0)) {
            p2LegAngle = Vector2.SignedAngle(Vector2.right, auxVec) +90;
        }

        auxVec = player2.GetAxis2D("Move Horizontal Right", "Move Vertical Right");
        if (!(auxVec.x == 0 && auxVec.y == 0)) {
            p2ArmAngle = Vector2.SignedAngle(Vector2.right, auxVec) + 90;
        }

        /* if (Input.GetKey(KeyCode.D)) {
             legRightAngle -= Time.deltaTime * legMovSpeed;
         } else if (Input.GetKey(KeyCode.A)) {
             legRightAngle += Time.deltaTime * legMovSpeed;
         }
         legRightAngle = Mathf.Clamp(legRightAngle, -90, 90);

         if (Input.GetKey(KeyCode.E)) {
             legLeftAngle -= Time.deltaTime * legMovSpeed;
         } else if (Input.GetKey(KeyCode.Q)) {
             legLeftAngle += Time.deltaTime * legMovSpeed;
         }
         legLeftAngle = Mathf.Clamp(legLeftAngle, -90, 90);*/

        if (Input.GetKeyDown(KeyCode.K)) {
           // updateWeapons(0.5f, 0.5f);
        }

        updateJointValues();
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
