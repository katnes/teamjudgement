﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public TextMesh text;
	public GameObject deathCrate;
	//public GameObject bloodTrigger;
	public float gravitySpeed;
	public GameObject bloodSplat;
	public GameObject deathScream1;
	public GameObject deathScream2;
	public GameObject deathScream3;
	public GameObject nextPris;
	public GameObject squish; 
	public GameObject crowdCheer;
	public GameObject crowdBoo; 
	public RatingSlider ratingSlider;

	//setting up prisoner sounds
	 public AudioSource[] pSounds;
	 public AudioSource intro;
	 public AudioSource plea;
	 public AudioSource crowd01;
	 public AudioSource crowd02;
	 public AudioSource kill;
	 public AudioSource spare;

	public enum GameState {INIT, CINEMATIC1, ACTION, CINEMATIC2, FINAL_PAUSE};
	public GameState gameState = GameState.INIT;

	// Set up Characters
	public GameObject Cobbler;
	public GameObject Smith;
	public GameObject Noble;
	public GameObject Soldier;
	public GameObject Wanderer;
	public GameObject peasant;
	int prisonerNum = 1;
	const int maxPrisonerNum = 5;

	int rand;
	bool screaming = false;
	bool wasKilled = true;

	public float countdownTime = 60.0f;
	public float timePassedInGameState = 0.0f;

	private Animator anim;
	private Animator crowdanim;

	// Use this for initialization
	void Start () {
		squish.SetActive (false);
		deathCrate.SetActive (false);
		crowdCheer.SetActive (false);
		Physics.gravity = new Vector3 (0, gravitySpeed * -1, 0);
		prisonerNum = 1;
		crowdanim = GameObject.Find ("Crowd2").GetComponent<Animator>();
	}

	void changeGameState(GameState newState) {
		gameState = newState;
		timePassedInGameState = 0.0f;
	}

	//coroutine Test	
	IEnumerator TestCoroutine()
	{
		yield return new WaitUntil (() => !intro.isPlaying);
		plea.Play();
	}

	// Update is called once per frame
	void Update () {

		timePassedInGameState += Time.deltaTime;

		if (gameState == GameState.INIT) {
			// Initialize the loop
			// Play "next prisoner" sound
			nextPris.SetActive (true);
			// Activate next prisoner prefab
			if (prisonerNum == 1) {
				Cobbler.SetActive(true);
				peasant = GameObject.Find ("Cobbler");
				anim = peasant.GetComponent<Animator>();
				//set sounds
				pSounds = peasant.GetComponents<AudioSource>();
				intro = pSounds[5];
 				plea = pSounds[4];
				crowd01 = pSounds[2];
				crowd02 = pSounds[3];
				kill = pSounds[1];
				spare = pSounds[0];
				//set popularity
				ratingSlider.prisoner.background = -5;
				ratingSlider.prisoner.action = -5;
				ratingSlider.prisoner.renown = -5;
			} else if (prisonerNum == 2) {
				Cobbler.SetActive(false);
				Soldier.SetActive(true);
				peasant = GameObject.Find ("Soldier");
				anim = peasant.GetComponent<Animator>();
				//set sounds
				pSounds = peasant.GetComponents<AudioSource>();
				intro = pSounds[5];
 				plea = pSounds[4];
				crowd01 = pSounds[2];
				crowd02 = pSounds[3];
				kill = pSounds[1];
				spare = pSounds[0];
				//set popularity
				ratingSlider.prisoner.background = 10;
				ratingSlider.prisoner.action = -5;
				ratingSlider.prisoner.renown = 10;
			} else if (prisonerNum == 3) {
				Soldier.SetActive(false);
				Wanderer.SetActive(true);
				peasant = GameObject.Find ("Wanderer");
				anim = peasant.GetComponent<Animator>();
				//set sounds
				pSounds = peasant.GetComponents<AudioSource>();
				intro = pSounds[5];
 				plea = pSounds[4];
				crowd01 = pSounds[2];
				crowd02 = pSounds[3];
				kill = pSounds[1];
				spare = pSounds[0];
				//set popularity
				ratingSlider.prisoner.background = -10;
				ratingSlider.prisoner.action = 30;
				ratingSlider.prisoner.renown = 10;
			} else if (prisonerNum == 4) {
				Smith.SetActive(true);
				peasant = GameObject.Find ("Smith");
				anim = peasant.GetComponent<Animator>();
				//set sounds
				pSounds = peasant.GetComponents<AudioSource>();
				intro = pSounds[5];
 				plea = pSounds[4];
				crowd01 = pSounds[2];
				crowd02 = pSounds[3];
				kill = pSounds[1];
				spare = pSounds[0];
				//set popularity
				ratingSlider.prisoner.background = -10;
				ratingSlider.prisoner.action = 0;
				ratingSlider.prisoner.renown = 20;
			} else if (prisonerNum == 5) {
				Noble.SetActive(true);
				peasant = GameObject.Find ("Noble");
				anim = peasant.GetComponent<Animator>();
				//set sounds
				pSounds = peasant.GetComponents<AudioSource>();
				intro = pSounds[5];
 				plea = pSounds[4];
				crowd01 = pSounds[2];
				crowd02 = pSounds[3];
				kill = pSounds[1];
				spare = pSounds[0];
				//set popularity
				ratingSlider.prisoner.background = -10;
				ratingSlider.prisoner.action = -20;
				ratingSlider.prisoner.renown = -10;
			}

			// Loop back around prisoners
			++prisonerNum;
			if (prisonerNum > maxPrisonerNum)
				prisonerNum = 1;
			
			peasant.SetActive (true);

			//After walk animation, change state
			anim.Play("Walk");
			changeGameState (GameState.CINEMATIC1);
			
		
		}
		// TODO: When CINEMATIC1 is over, go to ACTION - think this is OK?
		else if (gameState == GameState.CINEMATIC1 && timePassedInGameState > 2.0f) {
			intro.Play();
			changeGameState (GameState.ACTION);
		} else if (gameState == GameState.ACTION) {

			countdownTime -= Time.deltaTime;
			int remainingSeconds = Mathf.CeilToInt (countdownTime);
			if (remainingSeconds < 0)
				remainingSeconds = 0;
			ratingSlider.countdownText.text = "" + remainingSeconds;
			
			//playing audio clips
			if (intro.isPlaying)
				{
				plea.PlayDelayed(.5f);
				//prisoner idle animation
				anim.Play("Kneel");
				//else StartCoroutine(TestCoroutine());
				crowd01.PlayDelayed(.5f + plea.clip.length);
				crowd02.PlayDelayed(.5f + plea.clip.length + crowd01.clip.length);
				}
							
			//TODO: Add code to listen for touch of Death game object
			if (Input.GetKeyDown (KeyCode.D)) {
				//run kill code
				changeGameState (GameState.CINEMATIC2);
				kill.Play();
				anim.Play("Kill");
				wasKilled = true;
				ratingSlider.shiftPopularity (wasKilled);
				text.text = "Killed";

				rand = Random.Range (0, 3);
				if (screaming == false && !kill.isPlaying) {
					switch (rand) {
					case 0:
						deathScream1.SetActive (true);
						break;
					case 1:
						deathScream2.SetActive (true);
						break;
					case 2:
						deathScream3.SetActive (true);
						break;
					default:
						Debug.Log ("something is wrong");
						break;
					}
				}
			}

			//TODO: Add code to listen for touch of Mercy gameobject
			if (Input.GetKeyDown (KeyCode.A) || countdownTime < 0.0f) {
				//run let live code
				changeGameState (GameState.FINAL_PAUSE);
				spare.Play();
				wasKilled = false;
				ratingSlider.shiftPopularity (wasKilled);
				text.text = "Let live";
				anim.Play("Live");

				//TODO: Figure out if crowd should be happy or sad based on popularity score and choice
				crowdBoo.SetActive (true);
				crowdanim.Play("Mad");
			}
		} else if (gameState == GameState.CINEMATIC2) {
			
			if (timePassedInGameState > 2.0f) {

				if (wasKilled) {
					deathCrate.SetActive (true);
					//Physics.gravity = new Vector3 (0, gravitySpeed * -1, 0);
				} else {
					Debug.Log ("Inside CINEMATIC2 when spared!");
					// What do we do if you spare them?  For now, should never be here
				}
				changeGameState(GameState.FINAL_PAUSE);
			}
		} else if (gameState == GameState.FINAL_PAUSE) {
			if (timePassedInGameState > 5.0f) {
				reset();
			}
		}

		// Check final game loss conditions
		if (ratingSlider.kingdom.popularity <= 0) {
			// TODO: lose
		}
	}

	void reset() {

		squish.SetActive (false);
		deathCrate.transform.position = new Vector3 (deathCrate.transform.position.x, 10.0f, deathCrate.transform.position.z);
		deathCrate.SetActive (false);

		crowdCheer.SetActive (false);
		nextPris.SetActive (false);
		bloodSplat.SetActive (false);
		text.text = "";
		ratingSlider.ratingText.text = "";

		screaming = false;
		wasKilled = true;
		countdownTime = 35.0f;

		changeGameState(GameState.INIT);
	}
}
