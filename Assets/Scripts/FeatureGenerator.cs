using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatureGenerator : MonoBehaviour {

    //public List<float> features;
    //public string[] playerNames;

    public static string[] MixNames(string name1, string name2)
    {
        string[] res = new string[2];
        int name1Size = name1.Length - 1;
        int name2Size = name2.Length - 1;

        res[0] = name1.Substring(0, name1Size/2+ 1) + name2.Substring(name2Size / 2 + 1, name2Size - name2Size / 2);
        res[1] = name2.Substring(0, name2Size / 2 + 1) +  name1.Substring(name1Size/2 +1, name1Size- name1Size / 2) ;

        GenerateNumbersFromString("rugard");
        return res;
    }

    public static List<float> GenerateNumbersFromString(string input)
    {
        List<float> res = new List<float>();
        float seed = 0;

        if(input.Length<10)
        {
            input += input.Substring(0, 10 - input.Length);
        }

        foreach (char c in input)
        {
            seed += (float)c;
        }
        print(seed);

        seed = remap(seed, 291.0f, 1220.0f, 0.0f, 1.0f);
        for (int i = 0; i<input.Length; i++)
        {
            float remappedInputChar = remap((float)input[i], 97.0f, 123.0f, 0.0f, 1.0f);

            res.Add((seed + remappedInputChar)>1.0f? seed - remappedInputChar: seed + remappedInputChar);
        }
        

        return res;
    }
    static float remap(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
}
