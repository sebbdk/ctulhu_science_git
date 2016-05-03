using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SimpleJSON;

public class DialogController : MonoBehaviour {

	public TextAsset JSONData;
	public string initialReponseID;

	public GameObject textPrompt;
	public GameObject choisePrefab;

	public GameObject backgroundImage;

	private JSONNode reponses;
	private JSONNode currentResponse;

	// Use this for initialization
	void Start () {
		LoadData ();
	}

	void LoadData() {
		reponses = JSON.Parse (JSONData.text);
		ShowReponse (initialReponseID);
	}

	public void ShowReponse(string id) {
		currentResponse = reponses [id];
		textPrompt.GetComponent<Text> ().text = currentResponse ["text"];

		//remove old choices
		GameObject[] choices = GameObject.FindGameObjectsWithTag ("Choice");
		for(var i = 0 ; i < choices.Length ; i ++) {
			Destroy(choices[i]);
		}

		//add new choices
		foreach (JSONNode reponse in currentResponse ["actions"] as JSONArray) {
			GameObject choice = Instantiate (choisePrefab);
			choice.transform.SetParent (transform, false);
			choice.GetComponent<Text>().text = " - " + reponse["text"];
			AddListener (reponse["goto"], choice.GetComponent<Button> ());
		}

		//render background
		if (currentResponse ["graphic"] != null) {
			string filePath = currentResponse ["graphic"];
			Debug.Log (filePath);
			Sprite sprite = Resources.Load (filePath, typeof(Sprite)) as Sprite;
			Debug.Log (sprite);
			backgroundImage.GetComponent<SpriteRenderer> ().sprite = sprite;
		}

	}

	void AddListener(string name, Button button) {
		button.onClick.AddListener(() => ShowReponse(name));
	}
}
