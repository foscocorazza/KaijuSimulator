using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationGenerator : MonoBehaviour {

    public GameObject swordBase;
    public GameObject hammerBase;
    public GameObject whipBase;
    public GameObject whipCube;
    public GameObject rangeBase;

    public Rigidbody p1Hand;
    public Rigidbody p2Hand;

    [HideInInspector]
    public GameObject p1Weapon;
    [HideInInspector]
    public GameObject p2Weapon;

    private int probability;
    private List<float> generatedNum;
    private string abc = "qwertyuioplkjhgfdsazxcvbnm";

    void Awake() {
        //kirzde
        generatedNum = FeatureGenerator.GenerateNumbersFromString(getRandomString(6));
        probability = (int)FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 0.0f, 100.0f);
        mutateBody();
        mutateWeapons();
    }

    private string getRandomString(int length) {
        string result = "";
        for (int i = 0; i < length; i++) {
            result += abc[Random.Range(0, abc.Length)];
        }
        //Debug.Log(result);
        return result;
    }

    private void mutateBody() {


    }

    private void mutateWeapons() {
        //TODO based on something create weapon :v
        //Debug.Log(generatedNum[8] + " " + generatedNum[9]);

        //PLAYER 1
        float mass1 = 1;
        /*p1Weapon = CreateWhip((int)FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 3.0f, 8.0f), null,
            GameObject.Instantiate(whipBase, Vector3.up * 10, Quaternion.identity));
        mass1 = FeatureGenerator.remap(generatedNum[9], 0f, 1f, 0.01f, 0.15f);*/

        /*p1Weapon = CreateSword(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.5f, 1.5f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 15.0f, 50.0f));            
        mass1 = FeatureGenerator.remap(generatedNum[8]+generatedNum[9], 0f, 2f, 0.15f, 0.01f);*/

        /*p1Weapon = CreateHammer(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 4f, 10f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 4f, 15.0f));
        mass1 = FeatureGenerator.remap(generatedNum[8] + generatedNum[9], 0f, 2f, 0.15f, 0.01f);*/
        p1Weapon = CreateRangedWeapon(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.7f, 2f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 0.3f, 1.5f),
            Mathf.FloorToInt(FeatureGenerator.remap(generatedNum[7], 0.0f, 1.0f, 0f, 2.9f)));
        mass1 = FeatureGenerator.remap(generatedNum[8] + generatedNum[9], 0f, 2f, 0.15f, 0.01f);

        //Debug.Log("mass " + mass);
        GetComponent<PlayerController>().delayValues[1] = mass1;
        
        p1Weapon.transform.SetParent(transform);
        p1Weapon.transform.position = p1Hand.transform.position;
        p1Weapon.transform.localRotation = Quaternion.identity;
        p1Weapon.transform.GetChild(0).gameObject.AddComponent<CopyTransform>().targetGO = p1Hand.transform;

        //PLAYER 2
        float mass2;
        p2Weapon = CreateSword(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.5f, 1.5f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 15.0f, 50.0f));
        mass2 = FeatureGenerator.remap(generatedNum[8] + generatedNum[9], 0f, 2f, 0.15f, 0.01f);

        GetComponent<PlayerController>().delayValues[1] = mass2;
        
        p2Weapon.transform.SetParent(transform);
        p2Weapon.transform.position = p2Hand.transform.position;
        p2Weapon.transform.localRotation = Quaternion.identity;
        p2Weapon.transform.GetChild(0).gameObject.AddComponent<CopyTransform>().targetGO = p2Hand.transform;
    }

    public GameObject CreateWhip(int iterations, GameObject prevGO, GameObject rootGO) {
        if (iterations == 0) {
            return rootGO;
        }
        if (prevGO == null) {
            prevGO = rootGO.transform.GetChild(0).gameObject;
        }
        GameObject newObj = new GameObject();
        newObj = GameObject.Instantiate(whipCube, rootGO.transform);
        newObj.transform.localPosition = prevGO.transform.localPosition + new Vector3(prevGO.transform.localScale.x+0.1f, 0, 0);

        newObj.transform.localScale = newObj.transform.localScale +
            new Vector3(0f, Random.Range(0.1f, 0.4f), 0.0f);

        newObj.GetComponent<HingeJoint>().connectedBody = prevGO.GetComponent<Rigidbody>();
        prevGO = newObj;
        iterations--;

        return CreateWhip(iterations, prevGO, rootGO);

    }

    public GameObject CreateSword(float width, float height) { 
        //Debug.Log(width+" "+height);
        GameObject sB = GameObject.Instantiate(swordBase, Vector3.up * 10, Quaternion.identity);
        Transform sBase = sB.transform.GetChild(1);
        Transform blade = sBase.transform.GetChild(0);
        sBase.localScale = new Vector3(width, sBase.localScale.y, sBase.localScale.z);
        blade.localScale = new Vector3(blade.localScale.x, height, blade.localScale.z);
        blade.localPosition = new Vector3(0f, height/2 + 0.5f, 0f);
        return sB;
    }

    public GameObject CreateHammer(float width, float height) {
        GameObject sB = GameObject.Instantiate(hammerBase, Vector3.up *10, Quaternion.identity);
        Transform bottom = sB.transform.GetChild(0).GetChild(0);
        Transform top = bottom.transform.GetChild(0);

        bottom.localScale = new Vector3(bottom.localScale.x, height, bottom.localScale.z);
        bottom.localPosition = new Vector3(0, height / 2 + 0.5f, 0);

        top.localScale = new Vector3(width, height/15f, top.localScale.z);
        top.localPosition = new Vector3(0, height/30f + 0.5f, 0);
        return sB;
    }

    public GameObject CreateRangedWeapon(float width, float height, int bulletType) {
        GameObject sB = GameObject.Instantiate(rangeBase, Vector3.up * 10, Quaternion.identity);
        Transform mainBody = sB.transform.GetChild(0);
        mainBody.localScale = new Vector3(width, height, mainBody.localScale.z);
        mainBody.localPosition = new Vector3(0, height / 2 + 0.5f, 0);

        return sB;
    }
}
