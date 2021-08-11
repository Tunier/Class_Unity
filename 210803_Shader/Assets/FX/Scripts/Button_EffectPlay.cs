using UnityEngine;
using System.Collections;

public class Button_EffectPlay : MonoBehaviour {

	private string nowScene;


	void OnGUI () {
 		if(GUI.Button(new Rect(20,40,80,20), " Play")) {
			nowScene = Application.loadedLevelName;
			Application.LoadLevel(nowScene);
		}
	}
}