using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Video 1 - https://www.youtube.com/watch?v=mRC7sz-NAcE&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=2
// Video 2 - https://www.youtube.com/watch?v=a0fYMnurBUk&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=3
// Video 3 - https://www.youtube.com/watch?v=S1fRoRbNwSs&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=4
// Video 4 - https://www.youtube.com/watch?v=FqzZor_FRvo&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=5
// Video 5 - https://www.youtube.com/watch?v=A54aq0pniVo&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=6
// Video 6 - https://www.youtube.com/watch?v=cQHF4_YPvsM&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=7 
// Video 7 - https://www.youtube.com/watch?v=zXLkXMPc760&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=15 -- Buying skins for correct prices and Displaying correct Best times.
// Video 8 - https://www.youtube.com/watch?v=HvH5I4-BWxM&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=16 -- Locking/unloacking levels depending on completion.
// Video 9 - https://www.youtube.com/watch?v=LKTvt_SLN2s&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=27 -- 15 Mins: Make best time show in minutes and seconds rather than just seconds.
// Video 10 - https://www.youtube.com/watch?v=_zsS_TJOrUQ&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=28 -- Enabling stars depending on how fast we have completed the level


public class LevelData
{
	public LevelData(string levelName)
	{
		string data = PlayerPrefs.GetString(levelName);
		if (data == "")
		{
			return;
		}
		string[] allData = data.Split('&'); // Split the data at the & sign.
		BestTime = float.Parse(allData[0]);
		SilverTime = float.Parse(allData[1]);
		GoldTime = float.Parse(allData[2]);

	}

	public float BestTime { set; get; }
	public float SilverTime { set; get; }
	public float GoldTime { set; get; }
}

public class MainMenu : MonoBehaviour
{
	private const float CAMERA_TRANSITION_SPEED = 3f;

	public GameObject levelButtonPrefab;
	public GameObject levelButtonContainer;
	public GameObject shopButtonPrefab;
	public GameObject shopButtonContainer;
	public Text currencyText;

	// My Changes
	public Material[] playerMatArray;
	public GameObject playerPrefab;
	// My Changes End.

	private Transform cameraTransform;
	private Transform cameraDesiredLookAt;

	private bool nextLevelLocked = false;

	// 16 skins, 16 prices, change as needed.
	private int[] costs = { 0, 150, 150, 150, 300, 300, 300, 300, 500, 500, 500, 500, 1000, 1250, 1500, 2000 };

	private void Start()
	{

		ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
		currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
		cameraTransform = Camera.main.transform;
		
		Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels"); // Create an array of all the sprites that are inside the Resores>Levels Folder
		foreach(Sprite thumbnail in thumbnails) //For every sprite in this "thumbnails" array...
		{
			GameObject container = Instantiate(levelButtonPrefab) as GameObject; // Create a new button
			container.GetComponent<Image>().sprite = thumbnail; // Change background image for the thumbnail // Uncomment if this dosnt work
			container.transform.SetParent(levelButtonContainer.transform,false); //Set the prefabs parent to levelButtonContainer. // The "false" tells the prefab not to use its own position but instead use the parents position.
			
			LevelData level = new LevelData(thumbnail.name);

			string minutes = ((int)level.BestTime / 60).ToString("00"); // Used to have the best time sow in seconds and minutes rather that just seconds.
			string seconds = (level.BestTime % 60).ToString("00.00"); // Used to have the best time sow in seconds and minutes rather that just seconds. -------Best time nolonger saving

			container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (level.BestTime != 0.0f) ? minutes + ":" + seconds : "Not Completed";

			container.transform.GetChild(2).GetComponent<Image>().enabled = nextLevelLocked; // Have the LockedOverlays image componant enabled/disabled depending on wether nextLevellocked is true or false.
			container.transform.GetChild(2).GetChild(0).GetComponent<Text>().enabled = nextLevelLocked; // Have the LockedOverlays childs text componant enabled/disabled depending on wether nextLevellocked is true or false.
			container.GetComponent<Button>().interactable = !nextLevelLocked; // Make the ButtonPanel interactable/not interactable depending on wether nextLevelLocked is true or false.

			
			GameObject starOne = container.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
			GameObject starTwo = container.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
			GameObject starThree = container.transform.GetChild(1).GetChild(0).GetChild(2).gameObject;
			

			if (level.BestTime == 0.0f) // if we have not completed this level
			{
				nextLevelLocked = true;
			}
			else if (level.BestTime < level.GoldTime) // if the best time is less than the gold time (If we have completed the level withing the gold time)
			{
				//Enable 3 stars
				starOne.GetComponent<Image>().enabled = true;
				starTwo.GetComponent<Image>().enabled = true;
				starThree.GetComponent<Image>().enabled = true;
			}
			else if (level.BestTime < level.SilverTime) // if the best time is less than the silver time (If we have completed the level only within the silver time)
			{
				// Enable 2 stars
				starOne.GetComponent<Image>().enabled = true;
				starTwo.GetComponent<Image>().enabled = true;
			}
			else // if we have completed the level in anyother amout of time
			{
				// enable 1 star.
				starOne.GetComponent<Image>().enabled = true;
			}

			string sceneName = thumbnail.name;
			container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
		}

		// ---- Shop Stuff
		int textureIndex = 0;
		Sprite[] textures = Resources.LoadAll<Sprite>("Player"); // Create an arroy of all textures inside the Resorces>Player folder.
		foreach(Sprite texture in textures)
		{
			GameObject container = Instantiate(shopButtonPrefab) as GameObject; // Create a new button
			container.GetComponent<Image>().sprite = texture; // Change background image for the thumbnail
			container.transform.SetParent(shopButtonContainer.transform, false); //Set the prefabs parent to shopButtonContainer. // The "false" tells the prefab not to use its own position but instead use the parents position.

			int texIndex = textureIndex;
			container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(texIndex));
			container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costs[texIndex].ToString(); // Get the skin price text component and change it to the cost set in the costs[] array.
			if ((GameManager.Instance.skinAvailability & 1 << texIndex) == 1 << texIndex)
			{
				container.transform.GetChild(0).gameObject.SetActive(false);
			}
			textureIndex++;
		}
	}

	private void Update()
	{
		if(cameraDesiredLookAt != null)
		{
			cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, CAMERA_TRANSITION_SPEED * Time.deltaTime);
		}
	}

	private void LoadLevel(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public void LookAtMenu(Transform menuTransform)
	{
		cameraDesiredLookAt = menuTransform;
	}

	// i changed to puiblic
	public void ChangePlayerSkin(int skinIndex)
	{
		if ((GameManager.Instance.skinAvailability & 1 << skinIndex) == 1 << skinIndex)	// If the skin is available to us.
		{
			float y = ((int)skinIndex / 4) * .25f;

			if (y == .0f)
			{
				y = .75f;
			}
			else if (y == .25f)
			{
				y = .5f;
			}                                   //https://www.youtube.com/watch?v=A54aq0pniVo&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=6 11 mins in.
			else if (y == .50f)
			{
				y = .25f;
			}
			else if (y == .75f)
			{
				y = 0f;
			}
			
			// Change the players skin to the selected material. --------- Try moving all this to the GameManager script.
			Renderer playerRenderer = playerPrefab.GetComponent<Renderer>();	// Get access to the Renderer Component on the player.
			playerRenderer.sharedMaterial = playerMatArray[skinIndex];		// Set the players skin to the material in the material array at the GameManagers currentSkinIndex number.

			GameManager.Instance.currentSkinIndex = skinIndex;
			GameManager.Instance.Save();
		}
		else   // If we do not own the skin attempt to buy it.
		{
			int cost = costs[skinIndex]; // The "cost" is equal to the cost set in the costs array at the top of this script.

			if (GameManager.Instance.currency >= cost)	// If our current currency is equal to or more than the cost...
			{
				GameManager.Instance.currency -= cost; // Subtract the cost from the players currency...
				GameManager.Instance.skinAvailability += 1 << skinIndex;	// Make the skin available...
				GameManager.Instance.Save();	// Save everything to the PlayerPrefs.
				currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();	// Update the currency displayed ingame to our current currency.
				shopButtonContainer.transform.GetChild(skinIndex).GetChild(0).gameObject.SetActive(false);	// Disable the price tag graphic.
				ChangePlayerSkin(skinIndex);	// Return to the begining of this method;
			}
		}
	}
}
