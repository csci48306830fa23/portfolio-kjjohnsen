using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Skeeball : MonoBehaviour
{
    [SerializeField]
    TMP_Text infoText;
    [SerializeField]
    GameObject ballPrefab;
    [SerializeField]
    Transform ballSpawnLocation;

    [SerializeField]
    HoleTrigger trigger10;
	[SerializeField]
	HoleTrigger trigger20;
	[SerializeField]
	HoleTrigger trigger30;
	[SerializeField]
	HoleTrigger trigger40;
	[SerializeField]
	HoleTrigger trigger50;

    int points;
    int ballsThrown;
    int maxThrows = 9;
    bool gameStarted = false;
	// Start is called before the first frame update
	void Start()
    {
        trigger10.pointValue = 10;
        trigger20.pointValue = 20;
        trigger30.pointValue = 30;
        trigger40.pointValue = 40;
        trigger50.pointValue = 50;
        spawnBall();
        points = 0;
        ballsThrown = 0;
        updateScore();
        gameStarted = true;
    }

    void updateScore()
    {
		infoText.text = "Score: " + points+"\nBalls Left: " + (maxThrows-ballsThrown);
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnBall()
    {
        if (!gameStarted)
        {
            return;
        }
        GameObject.Instantiate<GameObject>(ballPrefab, 
            ballSpawnLocation.position, Quaternion.identity);
    }

    public void sensorDetected(HoleTrigger trigger)
    {
        if (!gameStarted)
        {
            return;
        }
        ballsThrown++;
        points += trigger.pointValue;
        updateScore();

        if (++ballsThrown < maxThrows)
        {
            spawnBall();
        }
        else
        {
            StartCoroutine(finishGame());
        }
    
    }

    IEnumerator finishGame()
    {
        gameStarted = false;
        infoText.text = "Game Over\nScore: " + points;
        yield return new WaitForSeconds(3.0f);
        points = 0;
        ballsThrown = 0;
        updateScore();
        gameStarted = true;
        spawnBall();

    }


}
