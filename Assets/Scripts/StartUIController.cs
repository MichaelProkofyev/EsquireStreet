using UnityEngine;
using UnityEngine.UI;

public class StartUIController : MonoBehaviour {

	public UIController animationController;
	public Button[] diffButtons;
	public bool finishedLoading = false;
	int selectedIdx = 0;
	float horizontal;

	// Use this for initialization
	void Start () {
		// diffButtons[0].Select();
	}
	
	// Update is called once per frame
	void Update () {

		if (finishedLoading && Input.anyKeyDown) {
			animationController.StartAnimation();
		}

		//  if (Input.GetAxisRaw("Vertical") == -1 && selectedIdx != 0) {
		// 		diffButtons[--selectedIdx].Select();
		//  }else if (Input.GetAxisRaw("Vertical") == 1 && selectedIdx != 2) {
		// 	diffButtons[++selectedIdx].Select();
		//  }
	}
}
