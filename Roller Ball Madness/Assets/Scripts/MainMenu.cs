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

	public Material playerMaterial;

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

		Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels"); // Create an array of all sprites inside the Resores>Levels Folder
		foreach(Sprite thumbnail in thumbnails) //For every sprite in this "thumbnails" array...
		{
			GameObject container = Instantiate(levelButtonPrefab) as GameObject; // Create a new button
			container.GetComponent<Image>().sprite = thumbnail; // Change background image for the thumbnail
			container.transform.SetParent(levelButtonContainer.transform,false); //Set the prefabs parent to levelButtonContainer. // The "false" tells the prefab not to use its own position but instead use the parents position.
			LevelData level = new LevelData(thumbnail.name);
			container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = (level.BestTime != 0.0f) ? level.BestTime.ToString("f") : "";

			container.transform.GetChild(1).GetComponent<Image>().enabled = nextLevelLocked; // Have the LockedOverlay enabled/disabled depending on wether nextLevellocked is true or false.
			container.GetComponent<Button>().interactable = !nextLevelLocked; // Make the ButtonPanel interactable/not interactable depending on wether nextLevelLocked is true or false.

			if (level.BestTime == 0.0f) // if we have not completed this level
			{
				nextLevelLocked = true;
			}

			string sceneName = thumbnail.name;
			container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
		}

		int textureIndex = 0;
		Sprite[] textures = Resources.LoadAll<Sprite>("Player"); // Create an arroy of all textures inside the Resorces>Player folder.
		foreach(Sprite texture in textures)
		{
			GameObject container = Instantiate(shopButtonPrefab) as GameObject; // Create a new button
			container.GetComponent<Image>().sprite = texture; // Change background image for the thumbnail
			container.transform.SetParent(shopButtonContainer.transform, false); //Set the prefabs parent to shopButtonContainer. // The "false" tells the prefab not to use its own position but instead use the parents position.

			int index = textureIndex;
			container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));
			container.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = costs[index].ToString(); // Get the skin price text component and change it to the cost set in the costs[] array.
			if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
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

	private void ChangePlayerSkin(int index)
	{
		if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
		{
			float x = (index % 4) * .25f;
			float y = ((int)index / 4) * .25f;

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

			playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
			GameManager.Instance.currentSkinIndex = index;
			GameManager.Instance.Save();
		}
		else
		{
			//You do not have the skin doyou want to buy it?
			int cost = costs[index];

			if (GameManager.Instance.currency >= cost)
			{
				GameManager.Instance.currency -= cost;
				GameManager.Instance.skinAvailability += 1 << index;
				GameManager.Instance.Save();
				currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
				shopButtonContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
				ChangePlayerSkin(index);
			}
		}
	}
}
