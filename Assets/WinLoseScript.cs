using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinLoseScript : MonoBehaviour {
    Global globalObj;
    public Text gameOverText;

    // Use this for initialization
    void Start() {
        GameObject g = GameObject.Find("GlobalObject");
        globalObj = g.GetComponent<Global>();
        gameOverText = gameObject.GetComponent<Text>();
        gameOverText.text = "";
    }

    // Update is called once per frame
    void Update() {
        if (globalObj.gameOver) {
            gameOverText.text = "GameOver: " + globalObj.score.ToString() + "\nPress Esc for Start Menu";
        }
    }


}
