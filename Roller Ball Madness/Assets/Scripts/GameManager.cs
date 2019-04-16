using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Video 1 - https://www.youtube.com/watch?v=cQHF4_YPvsM&list=PLLH3mUGkfFCWCsGUfwLMnDWdkpQuqW3xa&index=7 

public class GameManager : MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance { get { return instance; } }

	public int currentSkinIndex = 0;
	public int currency = 0;
	public int skinAvailability = 1;

    void Awake()
    {
		instance = this;
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
