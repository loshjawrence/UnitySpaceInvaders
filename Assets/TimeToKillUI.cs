using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeToKillUI : MonoBehaviour {
    Global globalObj;
    public Text TTKtext;

    // Use this for initialization
    void Start() {
        GameObject g = GameObject.Find("GlobalObject");
        globalObj = g.GetComponent<Global>();
        TTKtext = gameObject.GetComponent<Text>();
    }



    // Update is called once per frame
    void Update() {
        int TTK = (int)globalObj.TTK;
        TTKtext.text = "TimeToKill: " + TTK.ToString();
    }

}
