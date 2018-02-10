using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public BossJointController P1ArmRight;
    public BossJointController P1LegRight;
    public Rigidbody P1HandRight;
    public BossJointController P2ArmLeft;
    public BossJointController P2LegLeft;
    public Rigidbody P2HandRight;
    public float legMovSpeed = 360;
    public GameObject[] P1Weapons;
    public GameObject[] P2Weapons;

    private float legRightAngle;
    private float legLeftAngle;
    private int p1WeaponsN;
    private int p2WeaponsN;
    private GameObject p1AssignedWeapon;
    private GameObject p2AssignedWeapon;

    void Start () {
        legRightAngle = 0;
        legLeftAngle = 0;
        p1WeaponsN = P1Weapons.Length;
        p2WeaponsN= P2Weapons.Length;
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

        if (Input.GetKey(KeyCode.D)) {
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
        legLeftAngle = Mathf.Clamp(legLeftAngle, -90, 90);

        updateJointValues();
    }

    private void updateJointValues() {
        P1LegRight.updateTarget(legRightAngle);
        P2LegLeft.updateTarget(legLeftAngle);
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
        p1AssignedWeapon.GetComponent<HingeJoint>().connectedBody = P1HandRight;
        p2AssignedWeapon.GetComponent<HingeJoint>().connectedBody = P2HandRight;
    }

}
