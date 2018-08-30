using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using MadLevelManager;

public class SART_Test : MonoBehaviour {

	public GameObject door;
	public GameObject go;
	public GameObject noGo;
	public Text counter;
    public CSV_Maker csvMaker;


    public float waitTime;

	private BlockGenerator blockGenerator;

	private string blockType = "Test";
	private bool allTrialsDone;
	private int currentBlock;
	private int currentTrial;
	private bool showingTrial = false;
	private Coroutine lastRoutine;
	private float lastButtonPressTime = 0;

	//For writing CSV
	private float testStartTime;
	private float presentationTime;
	private int blockToWrite;
	private int trialNumberToWrite;
	private float firstHitTime;
	private float firstHitReactionTime;
	private bool trialValueToWrite;


	void Awake(){
		blockGenerator = GameObject.FindObjectOfType<BlockGenerator> ();
		csvMaker = GameObject.FindObjectOfType<CSV_Maker> ();

		currentBlock = 0;
		currentTrial = 0;
	}

	void Start(){

		Reset ();
		lastRoutine = StartCoroutine(StartGame());
	}

	void Update(){

		if(showingTrial == true){

			if(HitButton.buttonPressed == true && (Time.time - lastButtonPressTime > 0.3f)){ //When the player presses the button, we record the reaction time and write to CSV

				lastButtonPressTime = Time.time;

				firstHitTime = (Time.time - testStartTime) - presentationTime;

				firstHitReactionTime = firstHitTime + presentationTime;

				csvMaker.Write (blockType, blockToWrite, trialNumberToWrite, trialValueToWrite, presentationTime * 1000, firstHitReactionTime * 1000, 0, true);

				HitButton.buttonPressed = false;
				StopCoroutine (lastRoutine);
				StartCoroutine (WaitForNextStimulus ());

				if(allTrialsDone){ //If this was the last trial, exit test, output CSV, and return to Main Menu
					MainMenuManager.testCompleted = true;
					csvMaker.CreateCSVFile (csvMaker.path, csvMaker.dataCollected);
					SceneManager.LoadScene ("MainHUB");
				}

			} else if (HitButton.buttonPressed == true && (Time.time - lastButtonPressTime < 0.3f)){ //Check for second press within same trial. Deletes last row of CSV and rewrites it with both the first and second reaction time.
				csvMaker.dataCollected.RemoveAt(csvMaker.dataCollected.Count - 1);

				float secondHitTime = (Time.time - testStartTime) - presentationTime;
				float secondHitReactionTime = secondHitTime + presentationTime;

				csvMaker.Write (blockType, blockToWrite, trialNumberToWrite, trialValueToWrite, presentationTime * 1000, firstHitReactionTime * 1000, secondHitReactionTime * 1000, true);

				HitButton.buttonPressed = false;
			}
		}
	}

	IEnumerator StartGame(){ //Shows timer and then starts test
		for (int i = 3; i > 0; i--){
			counter.text = i.ToString ();
			yield return new WaitForSeconds (1);
		}

		counter.gameObject.SetActive (false);
		testStartTime = Time.time;

		StartCoroutine (WaitForNextStimulus ());
	}

	IEnumerator ShowTrial(bool trial){//Shows trial based on next item of block. If the player doesn't answer then write a CSV line with 0 as reaction time.

		trialValueToWrite = trial;

		presentationTime = Time.time - testStartTime;

		showingTrial = true;

		if(trial == true){
			ShowGo ();
		} else if (trial == false){
			ShowNoGo ();
		}
			
		yield return new WaitForSeconds (waitTime);

		csvMaker.Write (blockType, blockToWrite, trialNumberToWrite, trialValueToWrite, presentationTime * 1000, 0, 0, false);

		showingTrial = false;

		if(allTrialsDone){
			MainMenuManager.testCompleted = true;
			csvMaker.CreateCSVFile (csvMaker.path, csvMaker.dataCollected);
			MadLevel.LoadLevelByName ("MainHUB");
		} else {
			
			StartCoroutine (WaitForNextStimulus ());
		}
	}

	IEnumerator WaitForNextStimulus(){//Closes door and waits. After waiting shows next trial.
		
		Reset ();
		yield return new WaitForSeconds (waitTime);
		lastRoutine = StartCoroutine (ShowTrial (SelectTrial ()));
	}

	bool SelectTrial(){//Selects the next trial based on what was determiend by the blockgenerator.
		List<bool> block = blockGenerator.allBlocks [currentBlock];
		bool trial = block [currentTrial];

		blockToWrite = currentBlock;
		trialNumberToWrite = currentTrial;

		currentTrial++;

		if(currentTrial == block.Count && currentBlock == blockGenerator.allBlocks.Length - 1){
			allTrialsDone = true;
		}

		if(currentTrial == block.Count){
			currentTrial = 0;
			currentBlock++;
		}
	
		return trial;
			
	}

	//These functions just choose which character to show or the door.
	void ShowGo(){
		door.SetActive (false);
		noGo.SetActive (false);
		go.SetActive (true);
	}

	void ShowNoGo(){
		door.SetActive (false);
		go.SetActive (false);
		noGo.SetActive (true);
	}

	void Reset(){
		go.SetActive (false);
		noGo.SetActive (false);
		door.SetActive (true);
	}
}
