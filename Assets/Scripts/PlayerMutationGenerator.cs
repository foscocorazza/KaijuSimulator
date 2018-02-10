using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationGenerator : MonoBehaviour {

    public GameObject whipBase, whipSomething, swordBase;
    public Rigidbody p1Hand;

    public GameObject p1Weapon;

    private int probability;
    List<float> generatedNumbers;

    void Awake() {
        generatedNumbers = FeatureGenerator.GenerateNumbersFromString("rugard");
        probability = (int)FeatureGenerator.remap(generatedNumbers[9], 0.0f, 1.0f, 0.0f, 100.0f);
        mutateBody();
        //mutateWeapons();
    }

    private void mutateBody() {


    }

    private void mutateWeapons() {
        //TODO based on something create weapon :v

        //p1Weapon = CreateWhip((int)FeatureGenerator.remap(generatedNumbers[9], 0.0f, 1.0f, 5.0f, 14.0f), 0, null, null);
        p1Weapon = CreateSword(0.1f, FeatureGenerator.remap(generatedNumbers[9], 0.0f, 1.0f, 1.0f, 10.0f), 1.0f);
        //p1Weapon = CreateStick(0.1f, 1.5f, 1.0f);

        p1Weapon.transform.SetParent(transform);
        p1Weapon.transform.position = p1Hand.transform.position;
        p1Weapon.transform.localRotation = p1Hand.transform.localRotation;

        p1Weapon.transform.GetChild(0).GetComponent<SpringJoint>().connectedBody = p1Hand;
        //p1Hand.connectedBody = p1Weapon.transform.GetChild(0).GetComponent<Rigidbody>();
    }

    public GameObject CreateWhip(int iterations, float offset, GameObject prevGO, GameObject rootGO) {
        if (iterations == 0) {
            return rootGO;
        }
        GameObject newObj = new GameObject();
        if (Random.Range(0, 100) < probability)
            newObj = GameObject.Instantiate(whipBase, this.gameObject.transform);
        else
            newObj = GameObject.Instantiate(whipSomething, this.gameObject.transform);
        newObj.transform.position += new Vector3(offset, 0, 0);

        if (prevGO != null) {
            if (newObj.transform.localScale.x > 0.3f) newObj.transform.localScale = newObj.transform.localScale + new Vector3(Random.Range(0.0f, 0.2f), 0.0f, 0.0f);
            newObj.GetComponent<HingeJoint>().connectedBody = prevGO.GetComponent<Rigidbody>();
        } else {
            rootGO = newObj;
        }
        prevGO = newObj;
        iterations--;

        return CreateWhip(iterations--, offset + newObj.GetComponent<Collider>().bounds.size.x, prevGO, rootGO);

    }

    public GameObject CreateSword(float width, float height, float mass) {
        GameObject sB = GameObject.Instantiate(swordBase, Vector3.up * 10, Quaternion.identity);
        Transform blade = sB.transform.GetChild(2);
        blade.localScale = new Vector3(width, height, 0.05f);
        blade.localPosition += new Vector3(0, blade.gameObject.GetComponent<Collider>().bounds.size.y / 2 - 0.5f, 0);
        //sB.transform.GetChild(0).GetComponent<Rigidbody>().mass = mass;
        return sB;
    }

    public GameObject CreateStick(float width, float height, float mass) {
        GameObject sB = GameObject.Instantiate(swordBase, Vector3.up *10, Quaternion.identity);
        Transform top = sB.transform.GetChild(1);
        Transform bottom = sB.transform.GetChild(2);
        top.localScale = new Vector3(width, height, 0.05f);
        top.localPosition += new Vector3(0, top.gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);

        bottom.localScale = new Vector3(width, height, 0.05f);
        bottom.localPosition -= new Vector3(0, bottom.gameObject.GetComponent<Collider>().bounds.size.y / 2, 0);
        sB.GetComponent<Rigidbody>().mass = mass;
        return sB;
    }
}
