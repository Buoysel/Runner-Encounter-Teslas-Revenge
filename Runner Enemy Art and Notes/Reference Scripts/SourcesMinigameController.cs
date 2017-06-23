using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SourcesMinigameController : MonoBehaviour {

	public Image playerPortrait;
	public Sprite playerPortraitHurt;
	public Sprite playerPortraitDefeated;
	private Sprite playerPortraitDefault;

	private AttackingWord AW;

	private bool started = false, ready = true, gameEnded = false;

	private int cycle = 0;
	public int turns = 8;
	public float roundTime = 20f, currentTime;

	private Vector2 cartPosition = new Vector2 (400f, 400f);

	public GameObject bookPrefab;
	private GameObject book, book1, book2, book3;
	private AttackingWord aw1, aw2, aw3;
	public Canvas canvas;
	public Camera MainCamera;

	public GameObject BSP;
	public GameObject point1, point2, point3;

	private Text text;
	public Text announcement;

	private PlayerHealth playerHealth, enemyHealth;

	List<List<List<string>>> UpperList = new List<List<List<string>>>();

	List<List<string>> 	MidwestWeathers = new List<List<string>>(), 
						EagleHabits = new List<List<string>>(),
						RussianLit = new List<List<string>>(), 
						OregonTrail = new List<List<string>>();

	List<string> 		MidwestWeathersC = new List<string>(),
						MidwestWeathersW = new List<string>(),
						MidwestWeathersCUsed = new List<string>(),
						MidwestWeathersWUsed = new List<string>(),

						EagleHabitsC = new List<string>(),
						EagleHabitsW = new List<string>(),
						EagleHabitsCUsed = new List<string>(),
						EagleHabitsWUsed = new List<string>(),

						RussianLitC = new List<string>(),
						RussianLitW = new List<string>(),
						RussianLitCUsed = new List<string>(),
						RussianLitWUsed = new List<string>(),

						OregonTrailC = new List<string>(),
						OregonTrailW = new List<string>(),
						OregonTrailCUsed = new List<string>(),
						OregonTrailWUsed = new List<string>(),

						AngelHelp = new List<string>();

	public Text RockText, AngelText;

	public int startingWait = 1;

	void Start () {
		MidwestWeathersC.InsertRange (MidwestWeathers.Count, new string[]{			
			"The Midwest and Weather", "Tornado Season in the Midwest", 
			"Twister - Chasing Storms in the Midwest", "Beautiful Weather in the Midwest",
			"Examining the Midwest's Storms"
		});
		MidwestWeathersW.InsertRange (MidwestWeathers.Count, new string[]{
			"Unicorn Zebra - The Modern Rainbow", "Androids Suffer from Inclement Weather",
			"Reigning - Waves of Conspiracy", "The Perfect Western Day", 
			"Controlling the Weather with Electricity", "Composition of the Midwest",
			"A Day in the Midwest", "Weather Safety for Dummies"
		});
		
		EagleHabitsC.InsertRange (EagleHabits.Count, new string[]{
			"Bald Eagle Habitats", "The Home of the Eagle",	"Eagle Nesting Habits",
			"Habitats for Eagles - The Perfect Home", "Where to Find Eagle Nests"
		});
		EagleHabitsW.InsertRange (EagleHabits.Count, new string[]{
			"Eagles vs. Cowboys: Journey of Victory and Defeat", "Eagle Feeding",
			"Eagle Defense Tactics", "Soaring Above Searching for Prey with Eagles",
			"Eagle Calls and Sounds", "Taming Eagles", "Modeling Eagles",
			"Eagle Anatomy"
		});
		
		RussianLitC.InsertRange (RussianLit.Count, new string[]{
			"Russian Literature and Recurring Themes", "Crime and Punishment",
			"War and Peace", "Anna Karenina", "Leo Tolstoy"
		});
		RussianLitW.InsertRange (RussianLit.Count, new string[]{
			"Happiness and Joy", "Chaos and Bliss", "Les Miserables",
			"Four Clever Brothers", "Pride and Prejudice", "Frankenstein",
			"The Time Machine", "Vanity Fair"
		});

		OregonTrailC.InsertRange (OregonTrail.Count, new string[]{
			"Traveling During the 1800s on The Oregon Trail", 
			"Travels Across the Plains: The Oregon Trail Journal of Elizabeth J. Goltra",
			"Across the Wide and Lonesome Prairie: The Oregon Trail Diary of Hattie Campbell, 1847",
			"Our Wagon Train Through Danger - Overland Ship to Oregon", "Living on the Oregon Trail"
		});
		OregonTrailW.InsertRange (OregonTrail.Count, new string[]{
			"Adventures in Alaska",	"Traveling to Space",
			"Retracing the Oregon Trail in 2015 - Journey of Our Ancestors",
			"Journey to the Center of the Earth", "History of Midwestern Mining",
			"Talking to a Salesman in 1888", "Maps of Oregon from the 1800s", 
			"Finding Gracie: Adventures in Oregon" 
		});

		AngelHelp.InsertRange (AngelHelp.Count, new string[]{
			"Hello", "I'm here to help", "You can't win", "Go for the eyes!", "Here's a hint! (I'm out of ideas)", 
			"Defeat the monster before he escapes!", "It's been an honor standing here"
		});

		MidwestWeathers.InsertRange (MidwestWeathers.Count, new List<string>[] {
			MidwestWeathersC, MidwestWeathersW, MidwestWeathersCUsed, MidwestWeathersWUsed
		});
		EagleHabits.InsertRange (EagleHabits.Count, new List<string>[] {
			EagleHabitsC, EagleHabitsW, EagleHabitsCUsed, EagleHabitsWUsed
		});
		RussianLit.InsertRange (RussianLit.Count, new List<string>[] {
			RussianLitC, RussianLitW, RussianLitCUsed, RussianLitWUsed
		});
		OregonTrail.InsertRange (OregonTrail.Count, new List<string>[] {
			OregonTrailC, OregonTrailW, OregonTrailCUsed, OregonTrailWUsed
		});

		UpperList.InsertRange (UpperList.Count, new List<List<string>>[] {
			MidwestWeathers, EagleHabits, RussianLit, OregonTrail,
		});

		playerHealth = GameObject.Find("Player Health Container").GetComponent<PlayerHealth>();
		playerHealth.GeneratePlayerHealth();
		playerPortraitDefault = playerPortrait.sprite;

		enemyHealth = GameObject.Find("Enemy Health Container").GetComponent<PlayerHealth>();
		enemyHealth.GeneratePlayerHealth();

		RockText.text = "I'm your pet rock";
		AngelText.text = (AngelHelp [Random.Range (0, AngelHelp.Count)]);
		announcement.text = "Game started";
	}
	
	//-----------------------------------Update Section-----------------------------------//
	void FixedUpdate () { 
		if (ready == true) {
			if (started == false) {
				started = true;
				StartCoroutine (SpawnBook ());
				announcement.text = "";
				currentTime = roundTime;
			}

			currentTime -=Time.deltaTime;

			if(gameEnded == false){

				if(playerHealth.currentHealth <= 0 || turns <= 0){
					gameEnder(1);

				}else if(enemyHealth.currentHealth <= 0){
					gameEnder (2);
				}

				if(currentTime <= 0 && playerHealth.currentHealth > 0){
					if (turns > 0) {
						delBooks ();
						currentTime = roundTime;
						playerHealth.LoseHealth ();

						if (playerHealth.currentHealth > 0) {
							SpawnBookCaller ();
						} else {
							gameEnder (1);
						}
					}
				}
			}
		}
	}
	//-----------------------------------Game Ender-------------------------------------//

	public void gameEnder(int a){
		gameEnded = true;

		if(a == 1){
			//Player loses
			announcement.text = "Game Over";

			AngelText.text = "Our hero has been defeated!  All is lost!";
			RockText.text = "We shall become dust!";

			delBooks ();
			ChangeSprite(3);
		}else if(a == 2){
			//Player wins
			announcement.text = "You win!";

			AngelText.text = "Huzzah, the Bookcart has been defeated!";
			RockText.text = "Rock buddy takes credit for the win.";

			delBooks ();
			ChangeSprite(1);
		}
	}

	//-----------------------------------Gen Sections-----------------------------------//
	public void SpawnBookCaller(){
		if (playerHealth.currentHealth <= 0 || turns - 1 <= 0) {
			gameEnder (1);
		} else if (enemyHealth.currentHealth <= 0) {
			gameEnder (2);
		}else{
			delBooks ();
			StartCoroutine (SpawnBook());
			cycle ++;
			currentTime = roundTime;
			turns --;

			AngelText.text = (AngelHelp [Random.Range (0, AngelHelp.Count)]);
			//Debug.Log (turns +" turns left");
		}
	}

	IEnumerator SpawnBook(){
		CheckList ();

		yield return new WaitForSeconds (0.2f); 

		int A = Random.Range (1, 92);
		Vector2 position = new Vector2 (0, 0);
				
		int randomList = Random.Range (0, UpperList.Count);
		int randomCorrect = Random.Range (1, UpperList [randomList][0].Count);

		int count = 0;
		float SH = Screen.height;

		//123 (CWW)
		if (A <= 92 && A > 78) {
			popBook (position, point1.transform.position.y, randomList, 0, randomCorrect, count);

			count++;

			int randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point2.transform.position.y, randomList, 1, randomWrong, count);

			count++;

			randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point3.transform.position.y, randomList, 1, randomWrong, count);
				
			Debug.Log("CWW 1");

			//213 (WCW)
		} else if (A <= 78 && A > 62) {
			int randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point1.transform.position.y, randomList, 1, randomWrong, count);

			count++;
			
			popBook (position, point2.transform.position.y, randomList, 0, randomCorrect, count);

			count++;
			
			randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point3.transform.position.y, randomList, 1, randomWrong, count);

			Debug.Log("WCW 1");

			//231 (WWC)
		} else if (A <= 62 && A > 48) {
			int randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point1.transform.position.y, randomList, 1, randomWrong, count);

			count++;
			
			randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point2.transform.position.y, randomList, 1, randomWrong, count);

			count++;

			popBook (position, point3.transform.position.y, randomList, 0, randomCorrect, count);

			Debug.Log("WWC 1");

			//132 (CWW)
		} else if (A <= 48 && A > 32) {			
			popBook (position, point1.transform.position.y, randomList, 0, randomCorrect, count);

			count++;

			int randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point2.transform.position.y, randomList, 1, randomWrong, count);

			count++;
		
			randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point3.transform.position.y, randomList, 1, randomWrong, count);

			Debug.Log("CWW 2");

			//312 (WCW)
		} else if (A <= 32 && A > 16) {
			int randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point1.transform.position.y, randomList, 1, randomWrong, count);

			count++;

			popBook (position, point2.transform.position.y, randomList, 0, randomCorrect, count );

			count++;
			
			randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point3.transform.position.y, randomList, 1, randomWrong, count);

			Debug.Log("WCW 2");

			//321 (WWC)
		} else if (A <= 16 && A > 0) {
			int randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point1.transform.position.y, randomList, 1, randomWrong, count);

			count++;

			randomWrong = Random.Range (0, UpperList [randomList][1].Count);
			popBook (position, point2.transform.position.y, randomList, 1, randomWrong, count);
		
			count++;

			popBook (position, point3.transform.position.y, randomList, 0, randomCorrect, count);

			Debug.Log("WWC 2");

		} else {
			Debug.Log ("Something in IEnum Spawnbook broke");
		}

		RockText.text = (UpperList[randomList][0][0]);

	//-----------------------------------List Lister--------------------------------------//
		/*
		Debug.Log (MidwestWeathersC.Count + "/" + MidwestWeathersW.Count +" , "+ EagleHabitsC.Count + "/" + EagleHabitsW.Count
			+" , "+RussianLitC.Count + "/" + RussianLitW.Count+" , "+ OregonTrailC.Count + "/" + OregonTrailW.Count);
		*/
	}

	//-----------------------------------Sprite Section-----------------------------------//
	IEnumerator ChangeSpriteWait(){
		yield return new WaitForSeconds (0.5f);		
		
		playerPortrait.sprite = playerPortraitDefault;
	}
	
	public void ChangeSprite(int i){
		if (i == 1) {
			playerPortrait.sprite = playerPortraitDefault;
			
		} else if (i == 2) {
			playerPortrait.sprite = playerPortraitHurt;
			
			StopCoroutine("ChangeSpriteWait");
			StartCoroutine("ChangeSpriteWait");
			
		} else if (i == 3) {

			StopCoroutine("ChangeSpriteWait");
			playerPortrait.sprite = playerPortraitDefeated;
			
		} else {
			Debug.Log ("ChangeSprite was sent an invalid value (1-3 required) " + i + " was sent");
		}			
	}
	//-----------------------------------CheckList Section------------------------------//
	public void CheckList(){
		if (MidwestWeathersC.Count == 0 || MidwestWeathersW.Count == 0) {
			UpperList.Remove(MidwestWeathers);
		}

		if (EagleHabits.Count == 0 || EagleHabitsW.Count == 0) {
			UpperList.Remove(EagleHabits);
		}

		if (RussianLitC.Count == 0 || RussianLitW.Count == 0) {
			UpperList.Remove(RussianLit);
		}

		if (OregonTrailC.Count == 0 || OregonTrailW.Count == 0) {
			UpperList.Remove(OregonTrail);
		}
	}
	/*DEV Checklist -- DEFUNCT
	public void CheckList(){
		//list 0
		if (MidwestWeathersC.Count <= 2 || MidwestWeathersW.Count <= 2) {
			listRenewer (0);
		}
		//list 1
		if (EagleHabits.Count <= 2 || EagleHabitsW.Count <= 2) {
			listRenewer (1);
		}
		//list 2
		if (RussianLitC.Count <= 2 || RussianLitW.Count <= 2) {
			listRenewer (2);
		}
		//list 3
		if (OregonTrailC.Count <= 2 || OregonTrailW.Count <= 2) {
			listRenewer (3);
		}
	}

	public void listRenewer(int i){
		for(int a = 0; a <= UpperList[0][2].Count; a++){
			UpperList [i] [2] [0].Insert (a, UpperList [0] [0][a]);
			UpperList [i] [2].Clear ();
			Debug.Log ("Rep " + a);
		}

		for (int a = 0; a <= UpperList [0] [3].Count; a++) {
			UpperList [i] [3] [0].Insert (a, UpperList [0] [1] [a]);
			UpperList [i] [3].Clear ();
			Debug.Log ("Rep " + a);
		}
	}*/

	//-----------------------------------Book Section-----------------------------------//
	public void popBook(Vector2 startingP, float SH, int RL, int CWdeterminer, int randomVal, int count){

		GameObject spawnedWord = Instantiate (bookPrefab, startingP, Quaternion.identity) as GameObject;				
		spawnedWord.transform.SetParent(BSP.transform, false);

		text = GameObject.Find("Active Text").GetComponent<Text>();
		AW = GameObject.Find("Book(Clone)").GetComponent<AttackingWord>();
		book = GameObject.Find ("Book(Clone)");

		if (CWdeterminer == 0) {
			//Debug.Log(UpperList [RL] [0] [randomVal]);
			text.text = (UpperList [RL] [0] [randomVal]);

			UpperList [RL] [2].Add (UpperList [RL] [0][randomVal]);
			UpperList [RL] [0].RemoveAt (randomVal);

			AW.setVal(1);
			AW.setDesinationY(SH);

			text.transform.name = "InactiveText";
			book.transform.name = count + "R" + cycle;

		} else if (CWdeterminer == 1) {
			//Debug.Log(UpperList [RL] [1] [randomVal]);
			text.text = (UpperList [RL] [1] [randomVal]);

			UpperList [RL] [3].Add (UpperList [RL] [1][randomVal]);
			UpperList [RL] [1].RemoveAt (randomVal);

			AW.setVal(-1);
			AW.setDesinationY(SH);

			text.transform.name = "InactiveText";
			book.transform.name = count + "R" + cycle;

		} else {
			Debug.Log("Something in void popBook broke");
		}
	}



	public void delBooks(){
		string bName1 = 0 + "R" + (cycle), bName2 = 1 + "R" + (cycle), bName3 = 2 + "R" + (cycle);

		book1 = GameObject.Find (bName1);
		book2 = GameObject.Find (bName2);
		book3 = GameObject.Find (bName3);
		//Debug.Log (book1 + "" + bName1);

		book1.GetComponent<AttackingWord> ().externalSpawnFall (-750);
		book2.GetComponent<AttackingWord> ().externalSpawnFall (-750);
		book3.GetComponent<AttackingWord> ().externalSpawnFall (-750);
	
		StopCoroutine ("delAfterWait");
		StartCoroutine ("delAfterWait");
	}

	IEnumerator delAfterWait(){
		yield return new WaitForSeconds (1f);

		Destroy(book1);
		Destroy(book2);
		Destroy(book3);

		//Debug.Log ("deleted");

	}
}