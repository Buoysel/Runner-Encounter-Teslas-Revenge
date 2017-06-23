using UnityEngine;
using System.Collections;

public class AttackingWord : MonoBehaviour {

	private PlayerHealth playerHealth, enemyHealth;
	private SourcesMinigameController SMC;
	public int value = 0;
	public bool buttonLock = false;
	private bool atPos = false;
	public float buttonLockTime = 0.5f, SH = 0f;

	public float destinationY;

	private float speed = -500;

	void Start () {
		playerHealth = GameObject.Find("Player Health Container").GetComponent<PlayerHealth>();
		enemyHealth = GameObject.Find("Enemy Health Container").GetComponent<PlayerHealth>();
		SMC = GameObject.Find("Main Camera").GetComponent<SourcesMinigameController>();

		StartCoroutine ("startLock");
	}

	void FixedUpdate(){

		if (this.transform.position.y > destinationY && atPos == false) {
			SpawnFall (1);
		} else {
			SpawnFall (2);
			atPos = true;
		}
	}

	IEnumerator startLock(){
		yield return new WaitForSeconds (buttonLockTime);
		buttonLock = false;
	}

	//------------------------------------------SpawnFalling and related--------------------------//
	public void SpawnFall(int i){
		if (i == 1) {			
			this.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, speed);
		} else if (i == 2) {
			this.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, 0);
		}
	}
	
	public void setDesinationY(float y){
		destinationY = y;
	}

	public void externalSpawnFall(int s){
		atPos = false;
		destinationY = -1000;
		this.GetComponent<Rigidbody2D> ().velocity = new Vector2(0, s);
	}

	//------------------------------------------Press---------------------------------------------//
	public void onPress(){
		if(buttonLock == false){

			if (value == 1) {
				buttonLock = true;

				playerHealth.GainHealth();
				enemyHealth.LoseHealth();
				//Debug.Log ("player gained, enemy lost");

				if(playerHealth.currentHealth > 0){
					SMC.SpawnBookCaller();
				}
		
			} else if (value == -1) {
				buttonLock = true;

				playerHealth.LoseHealth();
				enemyHealth.GainHealth();

				SMC.ChangeSprite(2);

				//Debug.Log ("player lost, enemy gained");

				if(playerHealth.currentHealth > 0){
					SMC.SpawnBookCaller();
				}

			} else {
				Debug.Log ("Something in the AttackingWord Script broke");
			}
		}
	}

	public void setVal(int i){
		value = i;
	}
}
