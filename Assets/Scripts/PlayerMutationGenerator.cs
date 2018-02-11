using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutationGenerator : MonoBehaviour {

    public GameObject swordBase;
    public GameObject hammerBase;
    public GameObject whipBase;
    public GameObject whipCube;
    public GameObject rangeBase;
    public GameObject[] bullets;
    public float fireRate1 = 2.1f;
    public float fireRate2 = 1.3f;

    public Rigidbody p1HandHand;
    public Rigidbody p2HandHand;

    public Transform arm1;
    public Transform hand1;
    public Transform arm2;
    public Transform hand2;
    public Transform leg1;
    public Transform foot1;
    public Transform leg2;
    public Transform foot2;
    public Transform head1;
    public Transform head2;

    [HideInInspector]
    public GameObject p1Weapon;
    [HideInInspector]
    public GameObject p2Weapon;

    private GameObject myRangeW1;
    private GameObject myBullet1;
    private GameObject myRangeW2;
    private GameObject myBullet2;
    private int probability;
    private List<float> generatedNum;
    private string abc = "qwertyuioplkjhgfdsazxcvbnm";

    void Awake() {
        //kirzde
        string aux = PlayerPrefs.GetString("PlayerName");
        aux = aux == null || aux.Length < 5 ? getRandomString(6) : aux;
        generatedNum = FeatureGenerator.GenerateNumbersFromString(aux);
        probability = (int)FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 0.0f, 100.0f);
        mutateBody();
        mutateWeapons();
    }

    void Start() {
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
        float thickness = FeatureGenerator.remap(generatedNum[6], 0.0f, 1.0f, -0.1f, 0.7f);
        float mass = FeatureGenerator.remap(generatedNum[6], 0f, 1f, 0.15f, 0.01f);
        GetComponent<PlayerController>().delayValues[1] = mass;
        GetComponent<PlayerController>().delayValues[3] = mass;
        //ARMS
        arm1.localScale = new Vector3(arm1.localScale.x + thickness,
            arm1.localScale.y + FeatureGenerator.remap(generatedNum[5], 0.0f, 1.0f, -0.3f, 0.7f),
            arm1.localScale.z);
        arm1.GetComponent<HingeJoint>().anchor = Vector3.up;
        hand1.localScale = new Vector3(hand1.localScale.x + thickness,
            hand1.localScale.y,
            hand1.localScale.z);
        hand1.GetComponent<HingeJoint>().anchor = Vector3.up;

        arm2.localScale = new Vector3(arm2.localScale.x + thickness,
            arm2.localScale.y + FeatureGenerator.remap(generatedNum[4], 0.0f, 1.0f, -0.3f, 0.7f),
            arm2.localScale.z);
        arm2.GetComponent<HingeJoint>().anchor = Vector3.up;
        hand2.localScale = new Vector3(hand2.localScale.x + thickness,
            hand2.localScale.y,
            hand2.localScale.z);
        hand2.GetComponent<HingeJoint>().anchor = Vector3.up;

        //LEGS
        leg1.localScale = new Vector3(leg1.localScale.x + thickness,
            leg1.localScale.y + FeatureGenerator.remap(generatedNum[4], 0.0f, 1.0f, -0.3f, 0.7f),
            leg1.localScale.z);
        leg1.GetComponent<HingeJoint>().anchor = Vector3.up;
        foot1.localScale = new Vector3(foot1.localScale.x + thickness,
            foot1.localScale.y,
            foot1.localScale.z);
        foot1.GetComponent<HingeJoint>().anchor = Vector3.up;

        leg2.localScale = new Vector3(leg2.localScale.x + thickness,
            leg2.localScale.y + FeatureGenerator.remap(generatedNum[5], 0.0f, 1.0f, -0.3f, 0.7f),
            leg2.localScale.z);
        leg2.GetComponent<HingeJoint>().anchor = Vector3.up;
        foot2.localScale = new Vector3(foot2.localScale.x + thickness,
            foot2.localScale.y,
            foot2.localScale.z);
        foot2.GetComponent<HingeJoint>().anchor = Vector3.up;

        //HEADS
        head1.localScale = Vector3.one * FeatureGenerator.remap(generatedNum[4], 0.0f, 1.0f, 0.7f, 1.5f);
        head2.localScale = Vector3.one * FeatureGenerator.remap(generatedNum[5], 0.0f, 1.0f, 0.7f, 1.5f);
    }

    private void mutateWeapons() {
        //TODO based on something create weapon :v
        //Debug.Log(generatedNum[8] + " " + generatedNum[9]);

        //PLAYER 1
        int probWeapon1 = (int)FeatureGenerator.remap(generatedNum[1], 0.0f, 1.0f, 0.0f, 100f);
        //probWeapon1 = 80;
        if (probWeapon1 <= 25) {
            p1Weapon = CreateWhip((int)FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 4.0f, 10.0f), null,
            GameObject.Instantiate(whipBase, Vector3.up * 10, Quaternion.identity));
        } else if (probWeapon1 <= 50) {
            p1Weapon = CreateSword(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.5f, 1.5f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 15.0f, 50.0f));
        } else if (probWeapon1 <= 75) {
            p1Weapon = CreateHammer(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 4f, 10f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 4f, 15.0f));
        } else {
            p1Weapon = CreateRangedWeapon(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.7f, 2f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 0.3f, 1.5f));
            int auxBulletProb = (int)FeatureGenerator.remap(generatedNum[7], 0.0f, 1.0f, 0f, 100f);
            auxBulletProb = auxBulletProb <= 50 ? 0 : 1;
            myBullet1 = bullets[auxBulletProb];
            myRangeW1 = p1Weapon.transform.GetChild(0).gameObject;
            StartCoroutine(startFire1());
        }
        p1Weapon.transform.SetParent(transform);
        p1Weapon.transform.position = p1HandHand.transform.position;
        p1Weapon.transform.localRotation = Quaternion.identity;
        p1Weapon.transform.GetChild(0).gameObject.AddComponent<CopyTransform>().targetGO = p1HandHand.transform;

        //PLAYER 2
        int probWeapon2 = (int)FeatureGenerator.remap(generatedNum[2], 0.0f, 1.0f, 0.0f, 100f);
        //probWeapon2 = 80;
        if (probWeapon2 <= 25) {
            p2Weapon = CreateWhip((int)FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 4.0f, 10.0f), null,
            GameObject.Instantiate(whipBase, Vector3.up * 10, Quaternion.identity));
        } else if (probWeapon2 <=50) {
            p2Weapon = CreateSword(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0.5f, 1.5f),
                        FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 15.0f, 50.0f));
        } else if (probWeapon2 <=75) {
            p2Weapon = CreateHammer(FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 4f, 10f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 4f, 15.0f));
        } else {
            p2Weapon = CreateRangedWeapon(FeatureGenerator.remap(generatedNum[7], 0.0f, 1.0f, 0.7f, 2f),
            FeatureGenerator.remap(generatedNum[9], 0.0f, 1.0f, 0.3f, 1.5f));
            int auxBulletProb = (int)FeatureGenerator.remap(generatedNum[8], 0.0f, 1.0f, 0f, 100f);
            auxBulletProb = auxBulletProb <= 50 ? 0 : 1;
            myBullet2 = bullets[auxBulletProb];
            myRangeW2 = p2Weapon.transform.GetChild(0).gameObject;
            StartCoroutine(startFire2());
        }                
        p2Weapon.transform.SetParent(transform);
        p2Weapon.transform.position = p2HandHand.transform.position;
        p2Weapon.transform.localRotation = Quaternion.identity;
        p2Weapon.transform.GetChild(0).gameObject.AddComponent<CopyTransform>().targetGO = p2HandHand.transform;
        p2Weapon.transform.GetChild(0).gameObject.GetComponent<CopyTransform>().mirrorY = -1;
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

    public GameObject CreateRangedWeapon(float width, float height) {
        GameObject sB = GameObject.Instantiate(rangeBase, Vector3.up * 10, Quaternion.identity);
        Transform mainBody = sB.transform.GetChild(0);
        mainBody.localScale = new Vector3(width, height, mainBody.localScale.z);
        mainBody.localPosition = new Vector3(0, height / 2 + 0.5f, 0);
        return sB;
    }

    private IEnumerator startFire1() {
        GameObject aux;
        while (true) {
            yield return new WaitForSeconds(fireRate1);
            aux = GameObject.Instantiate(myBullet1, myRangeW1.transform.position, myRangeW1.transform.rotation);
        }
    }

    private IEnumerator startFire2() {
        GameObject aux;
        while (true) {
            yield return new WaitForSeconds(fireRate2);
            aux = GameObject.Instantiate(myBullet2, myRangeW2.transform.position, myRangeW2.transform.rotation);
        }
    }

}
