using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class Health : MonoBehaviour {

	public List<GameObject>	pHealthList;	//Player Health
	public List<GameObject>	eHealthList;	//Enemy Health

	public GameObject			healthOrb;		//current healt orb.

	public Text 				message;		//Status message.
	public int wrong = 1;

	void Start ()
	{
		message = GameObject.Find ("MessageText").GetComponent<Text>();
		message.text = "";

		//Get the health orbs
		for (int i = 1; i <= 3; i++) 
		{
			pHealthList.Add(GameObject.Find("pHP" + i));
			eHealthList.Add(GameObject.Find ("eHP" + i));
		}
	}

	public void DamagePlayer()
	{
		healthOrb = pHealthList[wrong - 1];


		message.text = "Ouch!";
		healthOrb.SetActive(false);

		if (wrong == 3)
			message.text = "Game Over";

		wrong++;
	}

	public void DamageEnemy()
	{
		healthOrb = eHealthList [0];
		eHealthList.Remove (healthOrb);

		message.text = "Correct!";
		Destroy (healthOrb);

		if (eHealthList.Count == 0) 
		{
			RunonEncounterController runoncontrol;
			runoncontrol = GetComponent<RunonEncounterController> ();
			runoncontrol.anim.SetTrigger ("PlayerWins");
			message.text = "Victory!";
		}
	}
}
