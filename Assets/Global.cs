using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour {
    public GameObject playerObjToSpawn;
    public float timer;
    public float spawnPeriod;
    public int numberSpawnedEachPeriod;
    public Vector3 originInScreenCoords;

    public GameObject playerObj;
    public int lives;
    private bool playerRespawn;
    private float playerRespawnTimer;

    public int score;
    public int totalAliens;
    public int totalKilledAliens;
    private int totalKilledAliens_big;
    private int totalKilledAliens_med;
    private int totalKilledAliens_small;

    public bool gameOver;

    private Vector3 camOrigPos;
    // Use this for initialization
    void Start()
    {
        Physics.gravity = new Vector3(0.0f, 0.0f, -9.8f);
        totalKilledAliens = 0;
        totalKilledAliens_big = 0;
        totalKilledAliens_med = 0;
        totalKilledAliens_small = 0;

        camOrigPos = Camera.main.transform.position;

        score = 0;
        timer = 0;
        spawnPeriod = 5.0f;
        numberSpawnedEachPeriod = 3;
        gameOver = false;

        originInScreenCoords =
         Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));

        //GameObject playerObj = GameObject.Find("Cannon") as GameObject;
        lives = 3;
        playerRespawn = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            Application.LoadLevel("TitleScene");
        }

        //check for win/lose condition
        if (totalKilledAliens >= totalAliens || lives <= 0) { gameOver = true; }

        if (gameOver) {
            EndGame();
        } else {
            PlayGame();
        }
    }

    void EndGame() {
        if (playerObj != null) {
            playerObj.GetComponent<Cannon>().DieGameOver();
        }
    }

    void PlayGame() {

        if (playerRespawn) {
            CameraShake();
            playerRespawnTimer -= Time.deltaTime;
            if (playerRespawnTimer <= 0.0f) {
                Instantiate(playerObjToSpawn, new Vector3(0, 0, -6.5f), Quaternion.identity);
                playerRespawn = false;
                Debug.Log("Reset cam to: " + camOrigPos.x + ", " + camOrigPos.y + ", " +camOrigPos.z);
                Camera.main.transform.position = camOrigPos;
            }
        }
        //timer += Time.deltaTime;
        //if (timer > spawnPeriod) {
        //    timer = 0;
        //    float width = Screen.width;
        //    float height = Screen.height;
        //    for (int i = 0; i < numberSpawnedEachPeriod; i++) {
        //        float horizontalPos = Random.Range(0.0f, width);
        //        float verticalPos = Random.Range(height / 5.0f, height);
        //        Instantiate(objToSpawn,
        //        Camera.main.ScreenToWorldPoint(
        //        new Vector3(horizontalPos,
        //        verticalPos, originInScreenCoords.z)),
        //         Quaternion.identity);
        //    }
        //}
    }

    public void UpdateTotalKilledAliens(int alienType) {
        ++totalKilledAliens;
        if(0 == alienType) {        //big
            ++totalKilledAliens_big;
        } else if (1 == alienType) {//med
            ++totalKilledAliens_med;
        } else {                    //small
            ++totalKilledAliens_small;
        }
    }

    public void UpdateLives()
    {
        --lives;
        playerRespawn = true;
        playerRespawnTimer = 2;
    }

    private void CameraShake()
    {
        Vector3 camPos = Camera.main.transform.position;
        float range = 0.2f;
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        Camera.main.transform.Translate(new Vector3(x, y, 0));//THIS IS LOCAL
    }
}
