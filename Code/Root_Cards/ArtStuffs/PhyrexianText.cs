using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhyrexianText : MonoBehaviour
{
    public static char[] Codes = new char[]{'q','w','e','r','t','y','u','o','p','a','s','d','f','g','h','j','k','z','x','c','v','b','n','m',
                                        ',','Q','W','E','R','T','Y','U','O','P','A','S','D','F','G','H','L','K','Z','X','C','V','B','N','M'};
    // Start is called before the first frame update
    public int count,min,max;
    public float changetime;
    float time;
    string target;
    void Start()
    {
        GetComponent<TextMeshPro>().text = GenerateStrings(count,min,max);
        GetComponent<Renderer>().sortingLayerName = "MostFront";
        target = GenerateStrings(count,min,max);
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        GetComponent<TextMeshPro>().text = LarpTexts(count,GetComponent<TextMeshPro>().text,target,time/changetime);
        if(time >= changetime){
            time = 0;
            target = GenerateStrings(count,min,max);
        }
    }

    public static string LarpTexts(int count, string start, string end, float part){
        count += 1;
        int target = UnityEngine.Mathf.RoundToInt(start.Length * part);
        var newString = start.ToCharArray();
        for(int i = 0; i < count; i++){
            for(int j = 0; j < start.Length/count; j++){
                if((i*count)+j < target){
                    newString[(j*count)+count - 2 -i] = end[(j*count)+count - 2 -i];
                }else{
                }
            }
        }
        return newString.ArrayToString();
    }

    public static string GenerateStrings(int count, int minLangth, int maxLangth){
        var strings = new List<string>();
        for(int i = 0; i<count; i++){
            int len = Random.Range(minLangth,maxLangth+1);
            string text = "|";
            for(int j = 0; j < len; j++){
                text += Codes[Random.Range(0,Codes.Length)];
            }
            text += ".";
            for(int k = len; k<maxLangth;k++) text += " ";
            strings.Add(text);
        }
        string ret = "";
        for(int i = 0; i<maxLangth+2; i++){
            for(int j = 0; j< strings.Count; j++){
                ret += strings[j][i];
            }
            ret += System.Convert.ToChar(10);
        }
        return ret;
    }
}
