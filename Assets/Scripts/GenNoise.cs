using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenNoise : MonoBehaviour {
    private Material mat;
    public Texture2D noiseTex;
    public int pixWidth;
    public int pixHeight;
    public float xOrg;
    public float yOrg;
    public float scale = 1.0F;

    private Color[] pix;

    private void Awake() {
        Renderer r = GetComponentInChildren<Renderer>();
        mat = r.material;
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
    }
    // Use this for initialization
    void Start () {
        noiseTex = new Texture2D(pixWidth, pixHeight);
	}
	
	// Update is called once per frame
	void Update () {
        CalcNoise();
        mat.SetTexture("_MainTex", noiseTex);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst) {
        Graphics.Blit(src, dst, mat);
    }

    void CalcNoise() {
        float y = 0.0F;
        while (y < noiseTex.height) {
            float x = 0.0F;
            while (x < noiseTex.width) {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
}
