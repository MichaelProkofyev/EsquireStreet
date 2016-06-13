using UnityEngine;
using System.Collections;

public class ExitController : MonoBehaviour {

	public UIController uiController;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
        uiController.StartFadeToWhite();
    }
}
