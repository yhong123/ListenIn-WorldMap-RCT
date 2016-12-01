using UnityEngine;
using System.Collections;
using MadLevelManager;

public class QuitApplication : MonoBehaviour {

	public void Quit()
	{
        //If we are running in a standalone build of the game
        ListenIn.Logger.Instance.Log("Starting quit routine", ListenIn.LoggerMessageType.Info);
		GameStateSaver.Instance.Save();
		Application.Quit();

		//If we are running in the editor
	#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
	#endif
	}

	public void ResetSavedGameState()
	{
		GameStateSaver.Instance.Reset();
        MadLevelProfile.Reset();
        Application.Quit();
		#if UNITY_EDITOR
		//Stop playing the scene
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}
}
