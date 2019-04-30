using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Video ? - https://www.youtube.com/watch?v=3yesq9qmvUk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=14
// Video 3 - https://www.youtube.com/watch?v=UUbTAphpq40&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=18 -- Player repawn after falling off level.
// Video 4 - https://www.youtube.com/watch?v=aKfXXySFfYk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=29 -- Timer
// Video 5 - https://www.youtube.com/watch?v=KcKo8QHOjlk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=30 -- Time buffer/dely before game start
// Video 6 - https://www.youtube.com/watch?v=cQmv_zkEZHY&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=31 -- Level End Panel/ win panel stuff



public class LevelManager : MonoBehaviour
{
	private const float TIME_BEFORE_START = 4.0f;   // Any changes to this value also have to be changed in the PlayerController script.

	private static LevelManager instance;
	public static LevelManager Instance { get { return instance; } }

	public GameObject pauseMenu;
	public GameObject endMenu;
	public Transform respawnPoint;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI endTimerText; // The time text that is on the win/level end panel 
	public GameObject gameplayUICanvas;
	private GameObject player;
	private LevelData LevelData; // Get access to the LevelData class from the mainmenu script.
	//	Coin Count Stuff.
	public GameObject levelPrefab; //	The Level prefab, needed to to get the child.count of coins.
	private int initialCoinCount = 0;
	public TextMeshProUGUI coinsCollectedText;
	//	Coin Count Stuff End.
	// Flashing Time Stuff.
	public GameObject flashingGoldTime;
	public GameObject flashingSilverTime;
	public GameObject flashingBronzeTime;
	//	Count Down Stuff.
	public GameObject[] countDownArray;

	private float startTime;
	private float levelDuration; // how long we have been playing the level for.
	public float silverTime;
	public float goldTime;

	private void Awake()
	{	//	Count down stuff.
		countDownArray[0].SetActive(false);
		countDownArray[1].SetActive(false);
		countDownArray[2].SetActive(false);
		countDownArray[3].SetActive(false);
		//	Count down stuff end.
	}
	private void Start()
	{
		instance = this;
		pauseMenu.SetActive(false);
		endMenu.SetActive(false);
		//	Flashing Time texts.
		flashingGoldTime.SetActive(false);
		flashingSilverTime.SetActive(false);		//------- Disable these at runtime.
		flashingBronzeTime.SetActive(false);
		//	Flashing Time texts end.
		startTime = Time.time;
		player = GameObject.FindGameObjectWithTag("Player");
		player.transform.position = respawnPoint.position;
		// Coin Stuff.-----------------------------------------------------------------------------------------------------------------------------------------
		// Find out how mamny coins are in the level.
		//	Get the coins game object, will only work if the coins game object is the first cild of the level prefab.
		GameObject coinParent = levelPrefab.transform.GetChild(0).gameObject;
		//	Get the child count of the coins game object.
		initialCoinCount = coinParent.transform.childCount;
		// Coin Stuff End.--------------------------------------------------------------------------------------------------------------------------------------
		//	Count Down Stuff.
		StartCoroutine(CountDown());
		//	Count Down Stuff End.
	}

	private void Update()
	{
		// Kill the player if their position on the Y-axis is less than -10;
		if (player.transform.position.y < -10)
		{
			Death();
		}
		

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
	{	//	If the game hasnt started yet / The count down hasnt finished / The timer hasnt started
		//	Dont allow the player to pause by "Returning" out of this method
		if (Time.time - startTime < TIME_BEFORE_START)
		{
			return;
		}
		//	Else allow the player to use the pause menu.
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

		// Coin Stuff.-------------------------------------------------------------------------------------------------------------------------------------------------------------
		// Find out how mamny coins are in the level.
		//	Get the coins game object, will only work if the coins game object is the first cild of the level prefab.
		GameObject coinParent = levelPrefab.transform.GetChild(0).gameObject;
		//	Get the child count of the coins game object.
		int endCoinCount = coinParent.transform.childCount;
		//	Calculate how many coins we collected.
		//int coinsCollected = Mathf.Abs (initialCoinCount - endCoinCount);
		int coinsCollected =  (initialCoinCount - endCoinCount);
		coinsCollectedText.text = "Coins Collected : " + coinsCollected;
		// Coin Stuff End.-------------------------------------------------------------------------------------------------------------------------------------------------------------

		// Star Stuff.
		GameObject starOne = gameplayUICanvas.transform.GetChild(7).GetChild(3).GetChild(0).gameObject;
		GameObject starTwo = gameplayUICanvas.transform.GetChild(7).GetChild(3).GetChild(1).gameObject;
		GameObject starThree = gameplayUICanvas.transform.GetChild(7).GetChild(3).GetChild(2).gameObject;
		// Star Stuff End.

		//	If we complete the level within the Gold Time.
		if (levelDuration < goldTime)
		{
			GameManager.Instance.currency += 50;
			// Enable 3 Stars
			starOne.GetComponent<Image>().enabled = true;
			starTwo.GetComponent<Image>().enabled = true;
			starThree.GetComponent<Image>().enabled = true;
			// Enable Flashing Gold Time.
			flashingGoldTime.SetActive(true);
		}
		//	If we complete the level within the Silver Time.
		else if (levelDuration < silverTime)
		{
			GameManager.Instance.currency += 25;
			// Enable 2 stars
			starOne.GetComponent<Image>().enabled = true;
			starTwo.GetComponent<Image>().enabled = true;
			// Enable Flashing Silver Time.
			flashingSilverTime.SetActive(true);
		}
		//	If we complete the level within the Bronze Time.
		else
		{
			GameManager.Instance.currency += 10;
			// enable 1 star.
			starOne.GetComponent<Image>().enabled = true;
			// Enable Flashing Bronze Time.
			flashingBronzeTime.SetActive(true);
		}
		//	Save everything
		GameManager.Instance.Save();
		// TODO - Find out what this all means
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
		RestartLevel();
	}

	IEnumerator CountDown()
	{	//	Enable/Disable elemets from the Count down array.
		countDownArray[0].SetActive(true);
		yield return new WaitForSecondsRealtime(.95f);
		countDownArray[0].SetActive(false);
		countDownArray[1].SetActive(true);
		yield return new WaitForSecondsRealtime(.95f);
		countDownArray[1].SetActive(false);
		countDownArray[2].SetActive(true);
		yield return new WaitForSecondsRealtime(.95f);
		countDownArray[2].SetActive(false);
		countDownArray[3].SetActive(true);
		yield return new WaitForSecondsRealtime(.95f);
		countDownArray[3].SetActive(false);
	}
}
