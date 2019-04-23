using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Video ? - https://www.youtube.com/watch?v=3yesq9qmvUk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=14
// Video 3 - https://www.youtube.com/watch?v=UUbTAphpq40&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=18 -- Player repawn after falling off level.
// Video 4 - https://www.youtube.com/watch?v=aKfXXySFfYk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=29 -- Timer
// Video 5 - https://www.youtube.com/watch?v=KcKo8QHOjlk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=30 -- Time buffer/dely before game start
// Video 6 - https://www.youtube.com/watch?v=cQmv_zkEZHY&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=31 -- Level End Panel/ win panel stuff



public class LevelManager : MonoBehaviour
{
	private const float TIME_BEFORE_START = 3.0f;

	private static LevelManager instance;
	public static LevelManager Instance { get { return instance; } }

	public GameObject pauseMenu;
	public GameObject endMenu;
	public Transform respawnPoint;
	public Text timerText;
	public Text endTimerText; // The time text that is on the win/level end panel 
	public GameObject gameplayUICanvas;
	private GameObject player;

	private float startTime;
	private float levelDuration; // how long we have been playing the level for.
	public float silverTime;
	public float goldTime;

	private void Start()
	{
		instance = this;
		pauseMenu.SetActive(false);
		endMenu.SetActive(false);
		startTime = Time.time;
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = respawnPoint.position;
	}

	private void Update()
	{
		/*
		// Kill the player if their position on the Y-axis is less than -10;
		if (player.transform.position.y < -10)
		{
			Death();
		}
		*/

		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}

		levelDuration = Time.time - (startTime + TIME_BEFORE_START);
		string minutes = ((int)levelDuration / 60).ToString("00"); // Used to have the timer show in seconds and minutes rather that just seconds.
		string seconds = (levelDuration % 60).ToString("00.00"); // Used to have the timer show in seconds and minutes rather that just seconds.

		timerText.text = minutes + ":" + seconds;
	}

	public void TogglePauseMenu()
	{
		pauseMenu.SetActive(!pauseMenu.activeSelf);
		Time.timeScale = (pauseMenu.activeSelf) ? 0 : 1;
	}

	public void RestartLevel()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ToMenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}

	public void Victory()
	{
		foreach(Transform t in endMenu.transform.parent) // for everything in the endMenu's parent(the GamePlayUI Canvas)... 
		{
			t.gameObject.SetActive(false); // turn off all the game objects.
		}
		endMenu.SetActive(true); // turn on the victory panel/Level end panel

		Rigidbody rigid = player.GetComponent<Rigidbody>();     ///	
		rigid.constraints = RigidbodyConstraints.FreezeAll;     /// Stop the ball from moving after the level has completed
		
		// End time text stuff
		levelDuration = Time.time - (startTime + TIME_BEFORE_START);
		string minutes = ((int)levelDuration / 60).ToString("00"); // Used to have the timer show in seconds and minutes rather that just seconds.
		string seconds = (levelDuration % 60).ToString("00.00"); // Used to have the timer show in seconds and minutes rather that just seconds.
		endTimerText.text = minutes + ":" + seconds;
		// End time text stuff. end

		// Star Stuff.
		GameObject starOne = gameplayUICanvas.transform.GetChild(6).GetChild(3).GetChild(0).gameObject;
		GameObject starTwo = gameplayUICanvas.transform.GetChild(6).GetChild(3).GetChild(1).gameObject;
		GameObject starThree = gameplayUICanvas.transform.GetChild(6).GetChild(3).GetChild(2).gameObject;
		// Star Stuff End.

		if (levelDuration < goldTime)
		{
			GameManager.Instance.currency += 50;
			// Enable 3 Stars
			starOne.GetComponent<Image>().enabled = true;
			starTwo.GetComponent<Image>().enabled = true;
			starThree.GetComponent<Image>().enabled = true;
		}
		else if (levelDuration < silverTime)
		{
			GameManager.Instance.currency += 25;
			// Enable 2 stars
			starOne.GetComponent<Image>().enabled = true;
			starTwo.GetComponent<Image>().enabled = true;
		}
		else
		{
			GameManager.Instance.currency += 10;
			// enable 1 star.
			starOne.GetComponent<Image>().enabled = true;
		}
		GameManager.Instance.Save();

		string saveString = "";
		LevelData level = new LevelData(SceneManager.GetActiveScene().name);
		saveString += (level.BestTime > levelDuration || level.BestTime == 0.0f) ? levelDuration.ToString() : level.BestTime.ToString();
		saveString += '&';
		saveString += silverTime.ToString();
		saveString += '&';
		saveString += goldTime.ToString();
		PlayerPrefs.SetString(SceneManager.GetActiveScene().name, saveString);
		
	}

	public void Death()
	{
		//player.transform.position = respawnPoint.position;
		//Rigidbody rigid = player.GetComponent<Rigidbody>(); //// These are commented out becouse N3K commented them out intead of deleting them.
		//rigid.velocity = Vector3.zero;
		//rigid.angularVelocity = Vector3.zero;
		RestartLevel();
	}
}
