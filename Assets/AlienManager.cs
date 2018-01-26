using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienManager : MonoBehaviour
{

    private Global global;
    private GameObject[] aliens;
    private int totalAliens;
    private float fireTimer;
    private float fireTimerLimit;
    private float moveTimer;
    private float moveTimerLimit;
    public Vector3 originInScreenCoords;
    public GameObject objToSpawn;
    private float moveHoriz;
    private float moveVert;
    private float moveHorizCounter;
    private float moveHorizCounterLimit;
    private float moveVertCounter;
    private float moveVertCounterLimit;
    private float width;
    private float height;
    private float speed;

    private int[] colIndices;
    private int cols;
    private int colRemaining;

    // Use this for initialization
    void Start()
    {
        GameObject globalObj = GameObject.Find("GlobalObject");
        global = globalObj.GetComponent<Global>();
        totalAliens = global.totalAliens;
        aliens = new GameObject[totalAliens];
        fireTimer = 0;
        moveTimer = 0;
        fireTimerLimit = 2.0f;
        moveTimerLimit = fireTimerLimit;
        speed = 1.0f;
        originInScreenCoords = Camera.main.WorldToScreenPoint(new Vector3(0, 0, 0));

        cols = 10;
        colRemaining = cols;
        colIndices = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        width = Screen.width;
        height = Screen.height;
        moveHorizCounterLimit = 10;
        moveHorizCounter = moveHorizCounterLimit/2;
        moveVertCounterLimit = 4;
        moveVertCounter = 0;

        //moveHoriz = width * ((2.0f / 5.0f) / (float)moveHorizCounterLimit);
        //moveVert = (-1.0f / 6.0f) * height;
        moveHoriz = 0.7f;
        moveVert = -0.8f;

        SpawnAndPlaceAliens();
    }

    void SpawnAndPlaceAliens() {
        int alienWithPowerUp = (int)Random.Range(0.0f, (float)colRemaining);
        float marginX = 1.0f / 5.0f;
        float startX = marginX;
        float startY = 1.0f / 2.0f;
        float aliensPerRow = 10.0f;
        float spacingX = (1.0f - 2.0f * marginX) / (aliensPerRow - 1.0f);
        float aliensPerCol = 3.0f;
        float spacingY = ((1.0f / 2.0f) - (1.0f / 5.0f)) / (aliensPerCol - 1.0f);
        //Debug.Log("totalAliens: " + totalAliens);
        for (int i = 0; i < totalAliens; ++i) {
            float horizontalPos = startX + ((i % (int)aliensPerRow) * spacingX);
            float verticalPos = startY + ((i / (int)aliensPerRow) * spacingY);
            horizontalPos *= width;
            verticalPos *= height;
            //Debug.Log("alien" + i + ": x=" + horizontalPos + " y=" + verticalPos);
            aliens[i] = Instantiate(objToSpawn,
            Camera.main.ScreenToWorldPoint(
                new Vector3(horizontalPos, verticalPos, originInScreenCoords.z)),
            Quaternion.identity);
            aliens[i].GetComponent<Alien>().hasPowerUp = 
                i == alienWithPowerUp ? true : false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (global.gameOver) { return; }
        if (moveVertCounter >= moveVertCounterLimit)
        {
            global.gameOver = true; return;
        }

        moveTimer += Time.deltaTime;
        fireTimer += Time.deltaTime;
        if (moveTimer >= moveTimerLimit*speed)
        {
            Move();
        }

        if (fireTimer >= fireTimerLimit*speed)
        {
            Fire();
        }
    }

    private void Fire() {

        //physics.ignorecollision isnt working in bullet so 
        //do some hacky thing to fire only from lowest alien in col
        for (int i = 0; i < cols; ++i) {
            for (int j = 0; j < 3; ++j) {
                if(colIndices[i] == -1) { break; }
                if(aliens[colIndices[i]] != null) {
                    break;
                } else {
                    colIndices[i] += cols;
                    if(colIndices[i] >= totalAliens) {
                        colIndices[i] = -1;
                        --colRemaining;
                        break;
                    }
                }
            }
        }
        
        fireTimer = 0.0f;
        int alienToFire = (int)Random.Range(0.0f, (float)colRemaining);
        int c = -1;
        for (int i = 0; i < cols; ++i) {
            if(colIndices[i] == -1) { continue; }
            if (aliens[colIndices[i]] != null && ++c == alienToFire)
            {
                aliens[colIndices[i]].GetComponent<Alien>().Fire();
                break;
            }
        }
    }
    private void Move()
    {
        moveTimer = 0.0f;
        bool moveDown = false;

        if (++moveHorizCounter >= moveHorizCounterLimit)
        {
            moveHorizCounter = 0;
            moveHoriz *= -1.0f;
            ++moveVertCounter;
            moveDown = true;
            speed -= 0.1f;
        }

        for (int i = 0; i < totalAliens; ++i)
        {
            if (aliens[i] != null)
            {
                if (moveDown)
                {
                    //Debug.Log("Moving alien" + i + " down Z by " + moveVert);
                    //Debug.Log("Before alien" + i + " pos: " + aliens[i].transform.position);
                    aliens[i].transform.Translate(0, 0, moveVert);
                    //Debug.Log("After alien" + i + " pos: " + aliens[i].transform.position);
                }
                else
                {
                    //Debug.Log("Moving alien" + i + " across X by " + moveHoriz);
                    //Debug.Log("Before alien" + i + " pos: " + aliens[i].transform.position);
                    aliens[i].transform.Translate(moveHoriz, 0, 0);
                    //Debug.Log("alien" + i + " pos: " + aliens[i].transform.position);
                }

            }
        }
    }
}