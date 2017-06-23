using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SentenceList : MonoBehaviour 
{
	private string[] sentences = 
	{
		"\"One day,\" boasted Dylan, \"I will hang my portrait between Edison and Tesla on the Wall of Fame and Science.\"",
		"The tour guide lavishly waved his hand toward an exhibit. \"To your right, you will see a miniture recreation of the 1893 \'Chicago World Fair.\' This was the first fair to be lit entirely through the use of electricity.\" (\"War of the Currents\")", 
		"\"We have only begun to scratch the surface!\" Shrieked the mad scientist as he observed the dog\'s frantic, uncomfortable behavior to the nibbling and burrowing of the one hundred and twelve test fleas.",
		"According to Jacobson, \"One famous legend surrounding the eccentric Tesla was that he had an earthquake machine in his Manhattan laboratory that shook his building and nearly brought down the neighborhood during experiments\" (Jacobson)",
		"\"When distraught,\" began the mournful librarian, \"I quote poetry. The \'Charge of the Light Brigade\' would be quite fitting at this moment for we too are fighting a battle outnumbered. Let us unsheath our fear molded swords and charge undaunted into this dark and ominous pit.\"",
		"Staring into the rising flames, Silas declared to the motionless crowd: \"Mother always told me \'Silas, fire will destroy your life one day if you make it your best friend.\'\"",
		
		//Citations
		"Grieve, Grantland. \"Strangest Man in New York.\" The Salt Lake Herald 27 June 1897: 17. Print.",
		"\"Invention to Revolutionize Naval Warfare.\" The San Francisco Call 9 Nov. 1898: 8. Print.",
		"McClure, S. S. \"What Are Cathode Rays?\" The Omaha Daily Bee 15 Mar. 1896: 13. Print.",
		"McGovern, Chauncey Montgomery. \"Artificial Daylight At Last Made By Tesla.\" The San Francisco Call 16 Apr. 1899: 1899. Print.",
		"\"Tesla\'s Death Beam.\" Plattsburgh Daily Republican 14 July 1934: 1934. Print.",
		"\"Tesla\'s Latest Wonder.\" The San Francisco Call 13 Nov. 1898: 25. Print.",
		"\"Tesla\'s New Invention.\" The Times 15 Feb. 1901: 24. Print.",
		"\"Wireless Vision Seen By Tesla.\" The Nassau Post 15 Oct. 1915: n. pag. Print."
	};

	private string[] powText =
	{
		"(POW!)",
		"(KA-BLAM!)",
		"(ZA-POW!)",
		"(BLAM!)",
		"(WHACK!)",
		"(THWACK!)",
		"(TWAP!)",
		"(CRACK!)",
		"(SMACK!)",
		"(KA-POW!)"
	};
	public MiniGameTimer miniGameTimer;

	private string randomizedPow;
	private string randomizedPowColors;

	private int[] indexOfPunctuation;
	private bool canErase = false;
	public Text sentence;
	public int currentPosition = 0;
	public int minInsertions;
	public int maxInsertions;
	public List<int> insertionIndexes;
	public List<int> preexistingIndexes;
	public List<int> overlappingIndexes;
	public List<GameObject> titleList;
	public AudioSource[] sources;
	public List<AudioSource> soundEffects;
	public string randomizedSentence;

	private QuotronHealth quotronHealth;
	private PlayerHealth playerHealth;

	private Text announcement;

	public Image playerPortrait;

	public Sprite playerPortraitRight;
	public Sprite playerPortraitLeft;
	public Sprite playerPortraitAttack;
	public Sprite playerPortraitHurt;
	public Sprite playerPortraitDefeated;
	private Sprite playerPortraitDefault;

	public GameObject quotron;
	private Animator quotronAnimator;

	public Image quotronPortrait;
	
	public Sprite quotronPortraitHurt;
	public Sprite quotronPortraitAngry;
	public Sprite quotronPortraitHappy;

	private float delay = 0.2f;
	
	public int coins;
	public int stash;

	public GameObject canvas;

	public GameObject ko;
	public GameObject pow;
	public GameObject projectile;
	
	public int combo;
	public GameObject comboText;
	private string comboTextforObject;
	public int comboTimer;
	
	void Start()
	{
		//disable pause menu
		PauseMenu.Instance.CanOpen = false;
		
		
		quotronHealth = GameObject.Find("Quotron Health Container").GetComponent<QuotronHealth>();
		quotronAnimator = quotron.GetComponent<Animator>();
		playerHealth = GameObject.Find("Player Health Container").GetComponent<PlayerHealth>();
		announcement = GameObject.Find("Announcement").GetComponent<Text>();
		sources = GetComponents<AudioSource>();

		foreach (AudioSource source in sources)
		{
			soundEffects.Add (source);
		}

		currentPosition = 0;
		
		playerPortraitDefault = playerPortrait.sprite;
		
		GenerateSentence();
	}

	public void GenerateSentence()
	{
		coins = 0;
		miniGameTimer.time = 0.0f;
		//miniGameTimer.timerActive = true;
		//select random sentence
		int randomSentence = Random.Range (0, sentences.Length);
		int numberOfInsertions = Random.Range (minInsertions, maxInsertions);
		indexOfPunctuation = new int[numberOfInsertions];
		
		string incorrectSentence = sentences[randomSentence];
		randomizedSentence = incorrectSentence;

		ScanForPreexistingPunctuation();
		quotronHealth.ClearHealth();

		playerPortrait.sprite = playerPortraitDefault;
		quotronAnimator.Play ("Encounter-Quotron-Idle", 1, 0f);
		announcement.text = "";

		for(int i = 0; i < numberOfInsertions; i++)
		{
			int randomLocation = Random.Range (0, incorrectSentence.Length);
			int punctuation = Random.Range (0, 2);

			if(punctuation == 0)
			{
				//insert ' at location
				if(((randomLocation+1 <= incorrectSentence.Length) || (randomLocation-1 >= incorrectSentence[0])) && incorrectSentence[randomLocation].Equals("\'"))
				{
						numberOfInsertions += 1;
				}
				else 
				{
					incorrectSentence = incorrectSentence.Insert(randomLocation, "\'");
					indexOfPunctuation[i] = randomLocation;
				}

			}
			else
			{
				//insert " at location
				if(((randomLocation+1 <= incorrectSentence.Length) || (randomLocation-1 >= incorrectSentence[0])) && incorrectSentence[randomLocation].Equals("\""))
				{
					if(incorrectSentence[randomLocation].Equals("\""))
					{
					numberOfInsertions += 1;
					}
				}
				else
				{
					incorrectSentence = incorrectSentence.Insert(randomLocation, "\"");
					indexOfPunctuation[i] = randomLocation;
				}
			}

			quotronHealth.currentHealth += 1;
			quotronHealth.GenerateQuotronHealth();
			coins += 5;

			for(int i2 = 0; i2 < preexistingIndexes.Count; i2++)
				{
					if(indexOfPunctuation[i] <= preexistingIndexes[i2])
					{
//						Debug.Log("Existing index found at " + preexistingIndexes[i2]);
						preexistingIndexes[i2] = preexistingIndexes[i2] + 1;
						
					}
				}
			
		}
		sentence.text = incorrectSentence;
//		Debug.Log ("sentence start length " + sentence.text.Length.ToString());
		
		GenerateIndexes();
		//GenerateBadIndexes();

		playerHealth.GeneratePlayerHealth();

	}

//----------------------------------SCAN FOR PREEXISTING INDEXES----------------------------------

	void ScanForPreexistingPunctuation()
	{
		preexistingIndexes.Clear ();

		for(int i = 0; i < randomizedSentence.Length; i++)
		{
			if(randomizedSentence[i].ToString().Equals("\"") || randomizedSentence[i].ToString().Equals("\'"))
			{
				preexistingIndexes.Add(i);
			}
		}
//		Debug.Log ("Pre-existing punctuation found: " + preexistingIndexes.Count);
	}

//----------------------------------GENERATE INDEXES----------------------------------
	
	void GenerateIndexes()
	{
		insertionIndexes.Clear();
		
		for(int i = 0; i < sentence.text.Length; i++)
		{
			if (sentence.text[i].ToString().Equals ("\"") || sentence.text[i].ToString().Equals ("\'"))
				{
					insertionIndexes.Add(i);
				}
		}
	}
		
	void GenerateBadIndexes()
	{
		overlappingIndexes.Clear();

		for(int i = 0; i < preexistingIndexes.Count; i++)
		{
//			Debug.Log ("Pre-existing index: " + preexistingIndexes[i]);
			if(preexistingIndexes[i].Equals(insertionIndexes[i]) == true)
			{
				overlappingIndexes.Add (insertionIndexes[i]);
			}
		}
	}

//----------------------------------UPDATE----------------------------------
	
	void Update()
	{
		if(canErase)
		{
			if(Input.GetKeyDown("a"))
			{
				Retreat();
			}
			
			if(Input.GetKeyDown("d"))
			{
				Advance();
			}
			
			if(Input.GetKeyDown("f"))
			{
				Erase();
			}
		}
	}

//----------------------------------ADVANCE----------------------------------

	void Advance()
	{
		if(insertionIndexes.Count != 0)
		{
			RemoveStyling();
			currentPosition++;

			if(playerPortraitRight != null)
			{
				playerPortrait.sprite = playerPortraitRight;

				delay = 0.5f;

				StopCoroutine("DelayReset");
				StartCoroutine("DelayReset");
			}
		
			if(currentPosition < insertionIndexes.Count)
			{
				ApplyStyling();
			}
			else
			{
				currentPosition = 0;
				ApplyStyling();
			}
		}
	}

//----------------------------------RETREAT----------------------------------
	
	void Retreat()
	{
		if(insertionIndexes.Count != 0)
		{
			RemoveStyling ();
			currentPosition--;
		
			if(playerPortraitLeft != null)
			{
				playerPortrait.sprite = playerPortraitLeft;

				delay = 0.5f;
			
				StopCoroutine("DelayReset");
				StartCoroutine("DelayReset");
			}
		
			if(currentPosition < 0)
			{
				currentPosition = insertionIndexes.Count - 1;
				ApplyStyling();
			}

			else
			{
				ApplyStyling();
			}
		}	
	}

//----------------------------------ERASE----------------------------------

	void Erase()
	{
		
		if (preexistingIndexes.Contains(insertionIndexes[currentPosition]))
		{
//			Debug.Log ("Wrong! " + sentence.text[insertionIndexes[currentPosition]+14].ToString());

			if(playerHealth.currentHealth <= 1)
			{
				StopCoroutine("DelayReset");
				
				playerHealth.LoseHealth();
//				Debug.Log("You lose!");
				StartCoroutine("DelayBeforeLoad");

				miniGameTimer.timerActive = false;
				int random = Random.Range(5, 25);
	
				if(stash <= random || stash == 0)
				{
					stash = 0;
					announcement.text = "You lose!";
				}
				else
				{
					announcement.text = "You lose! -" + random + " coins.";
					stash -= random;
				}
				soundEffects[3].Stop ();
				soundEffects[3].Play ();

				coins = 0;

				playerPortrait.sprite = playerPortraitDefeated;
				canErase = false;
			}
		
			else 
			{
				playerHealth.LoseHealth();
				soundEffects[6].Stop ();
				soundEffects[6].Play ();
				combo = 0;
			
				playerPortrait.sprite = playerPortraitHurt;
			
				StopCoroutine("DelayReset");
				StartCoroutine("DelayReset");
			}
		}	

		else if(sentence.text[insertionIndexes[currentPosition]+14].ToString().Equals("\'") || sentence.text[insertionIndexes[currentPosition]+14].ToString().Equals("\""))
		{
			playerPortrait.sprite = playerPortraitAttack;

			for (int i2 = 0; i2 < preexistingIndexes.Count; i2++)
			{
				if(insertionIndexes[currentPosition] < preexistingIndexes[i2])
				{
					preexistingIndexes[i2] = preexistingIndexes[i2] - 1;
				}
			}

			RemoveStyling();
			sentence.text = sentence.text.Remove (insertionIndexes[currentPosition], 1);
			insertionIndexes.Remove(insertionIndexes[currentPosition]);

			soundEffects[0].pitch = Random.Range(0.7f, 1.3f);
			soundEffects[0].Stop ();
			soundEffects[0].Play();

			GameObject tempProjectile = Instantiate (projectile) as GameObject;
			tempProjectile.transform.SetParent (canvas.transform, false);

			titleList.Add(tempProjectile);
			
			StartCoroutine ("DestroyOldTitles");

			int randomPow = Random.Range (0, powText.Length);
			float randomFloat = Random.Range (-30.0f, +30.0f);
			float randomScale = Random.Range (0.3f, 0.7f);
			randomizedPow = powText[randomPow];
			
			string[] powColors =
			{
				"<color=red>" + randomizedPow + "</color>",
				"<color=yellow>" + randomizedPow + "</color>",
				"<color=orange>" + randomizedPow + "</color>"
			};
			
			int randomPowColor = Random.Range (0, powColors.Length);
			
			randomizedPowColors = powColors[randomPowColor];
			
			GameObject tempPow = Instantiate (pow) as GameObject;
			tempPow.transform.SetParent (canvas.transform, false);
			tempPow.GetComponent<Text>().text = randomizedPowColors;
			tempPow.transform.rotation = Quaternion.Euler(0,0, randomFloat);
			tempPow.transform.position = new Vector3(tempPow.transform.position.x + randomFloat, tempPow.transform.position.y + randomFloat, tempPow.transform.position.z + randomFloat);
			tempPow.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
			
			titleList.Add(tempPow);
			
			StartCoroutine ("DestroyOldTitles");

//----------------------------------COMBO COUNTER----------------------------------
		
			combo++;

			StopCoroutine("ComboTimer");
			StartCoroutine("ComboTimer");

			if (combo > 1)
			{
				int randCoins;
				randCoins = Random.Range (0, 2);
				comboTextforObject = combo + " Hit Combo!";
				if (randCoins > 0)
				{
					comboTextforObject = combo + " Hit Combo!\n<color=yellow>+" + randCoins + " Coins!</color>";
					coins += randCoins;
				}
				
				if (combo > 2)
				{
					randCoins = Random.Range (3, 7);
					comboTextforObject = combo + " Hit Combo!\n<color=yellow>+" + randCoins + " Coins!</color>\n<i><color=green>Health Gained!</color></i>";
					coins += randCoins;
					playerHealth.GainHealth();
					soundEffects[5].Play();
				}
				if (combo > 4)
				{
					randCoins = Random.Range (8, 21);
					comboTextforObject = combo + " Hit Combo!\n<color=yellow>+" + randCoins + " Coins!</color>\n<i><color=green>Health Gained!</color></i>";
					coins += randCoins;
					playerHealth.GainHealth();
					soundEffects[5].Play();
				}

				GameObject temp = Instantiate (comboText) as GameObject;
				temp.transform.SetParent (canvas.transform, false);
				temp.GetComponent<Text>().text = comboTextforObject;
				
				titleList.Add(temp);
		
				StartCoroutine ("DestroyOldTitles");
			}

			if(insertionIndexes.Count == 0 || preexistingIndexes.Count == insertionIndexes.Count)
			{
//				Debug.Log ("You win!");
				StartCoroutine("DelayBeforeLoad");
				
				//counter and flag for the gamecontroller for returning to the main scene
				GameController.Instance.encounterWon = true;
				GameController.Instance.quotronsDefeated += 1;
				
				soundEffects[4].Stop ();
				soundEffects[4].Play ();

				miniGameTimer.timerActive = false;

				if(miniGameTimer.totalSeconds <= 8 + quotronHealth.totalHealth)
				{
					coins += 3;
					
					if(miniGameTimer.totalSeconds <= 5 + quotronHealth.totalHealth)
					{
						coins += 9;
					
						if(miniGameTimer.totalSeconds <= 3 + quotronHealth.totalHealth)
						{
							coins += 18;
						}
					}
				}

				GameObject temp = Instantiate (ko) as GameObject;
				temp.transform.SetParent (canvas.transform, false);
				temp.GetComponent<Text>().text = "You win! +" + coins + " Coins!";
				titleList.Add(temp);

				StartCoroutine("DestroyOldTitles");
		
				stash += coins;

				canErase = false;
			}

			else 
			{

				if(currentPosition >= insertionIndexes.Count)
				{
					currentPosition = 0;
				}

				else 
				{
					for(int i = 0; i < insertionIndexes.Count; i++)
					{
//						Debug.Log ("Index value: " + insertionIndexes[i]);
					
						if(insertionIndexes[i] >= insertionIndexes[currentPosition])
						{
							insertionIndexes[i] = insertionIndexes[i] - 1;
						}
					}
				}

					ApplyStyling();
				
			}
					
			StopCoroutine("DelayReset");
			StartCoroutine("DelayReset");
			
			if(quotronHealth.currentHealth == 1)
			{
				quotronHealth.LoseHealth();
				quotronAnimator.Play ("Encounter-Quotron-Death", 1, 0f);
				quotronPortrait.sprite = quotronPortraitHappy;
			}

			else 
			{
				quotronHealth.LoseHealth();
				quotronAnimator.Play ("Encounter-Quotron-Hurt", 0, 0f);
			}
		}

		else
		{
//			Debug.Log ("Wrong! " + sentence.text[insertionIndexes[currentPosition]+14].ToString());

			playerHealth.LoseHealth();

			playerPortrait.sprite = playerPortraitAttack;

			StopCoroutine("DelayReset");
			StartCoroutine("DelayReset");
			
		}
			
	}

//----------------------------------COROUTINE DELAY SPRITE SWITCH----------------------------------

	IEnumerator DelayReset()
	{
		yield return new WaitForSeconds(delay);

		playerPortrait.sprite = playerPortraitDefault;
	}

//----------------------------------COROUTINE COMBO TIMER----------------------------------
	
	IEnumerator ComboTimer()
	{
		yield return new WaitForSeconds(comboTimer);
		
		combo = 0;
	}

//----------------------------------DELAY BEFORE DESTROYING TEMP TITLE----------------------------------
	
	IEnumerator DestroyOldTitles()
	{
		yield return new WaitForSeconds(2);
		
		GameObject.Destroy(titleList[0]);
		titleList.Remove(titleList[0]);
	}

//----------------------------------TOGGLE ERASE----------------------------------
	
	public void StartErase()
	{
		if(canErase == false)
		{
			canErase = true;
			currentPosition = 0;
			ApplyStyling();
		}
		else if(canErase == true)
		{
			canErase = false;
			RemoveStyling();
		}
	}

//----------------------------------APPLY STYLING----------------------------------

	private void ApplyStyling()
	{
		sentence.text = sentence.text.Insert (insertionIndexes[currentPosition], "<color=red>");
		sentence.text = sentence.text.Insert (insertionIndexes[currentPosition] + 11, "<b>");
		sentence.text = sentence.text.Insert (insertionIndexes[currentPosition] + 15, "</b>");
		sentence.text = sentence.text.Insert (insertionIndexes[currentPosition] + 19, "</color>");
	}
	
//----------------------------------REMOVE STYLING----------------------------------
	
	private void RemoveStyling()
	{
		
		if(sentence.text.Contains("<b>"))
		{
			sentence.text = sentence.text.Replace ("<b>", "");
		}
		
		if(sentence.text.Contains("</b>"))
		{
			sentence.text = sentence.text.Replace ("</b>", "");
		}
		
		if(sentence.text.Contains("<color=red>"))
		{
			sentence.text = sentence.text.Replace ("<color=red>", "");
		}
		
		if(sentence.text.Contains("</color>"))
		{
			sentence.text = sentence.text.Replace ("</color>", "");
		}
	}
	
//----------------------------------COROUTINE DELAY LEVEL LOAD----------------------------------

	private IEnumerator DelayBeforeLoad()
	{
		yield return new WaitForSeconds(1.5f);
		SaveAndLoadLevel.Instance.LoadLevel("MainFloor");
	}
}
