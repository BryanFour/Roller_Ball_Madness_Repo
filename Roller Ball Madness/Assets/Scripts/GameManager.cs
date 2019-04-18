using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=cQHF4_YPvsM&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=7 

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get { return instance; } }
	public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

	public int currentSkinIndex = 0;
	public int currency = 0;
	public int skinAvailability = 1;

    void Awake()
    {

		//Check if instance already exists
		if (instance == null)
		{
			//if not, set instance to this
			instance = this;
		}
		//If instance already exists and it's not this:
		else if (instance != this)
		{ 

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);

		}

		DontDestroyOnLoad(gameObject);

		if (PlayerPrefs.HasKey("CurrentSkin")) // If we have played before
		{
			currentSkinIndex = PlayerPrefs.GetInt("CurrentSkin");
			currency = PlayerPrefs.GetInt("Currency");
			skinAvailability = PlayerPrefs.GetInt("SkinAvailability");
		}
		else // If we have never played before
		{
			Save();
		}
    }

	public void Save()
	{
		PlayerPrefs.SetInt("CurrentSkin", currentSkinIndex);
		PlayerPrefs.SetInt("Currency", currency);
		PlayerPrefs.SetInt("SkinAvailability", skinAvailability);
	}
}
