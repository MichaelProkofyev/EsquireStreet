using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	public Text loadingText;
	public Image loadingImage;
	float fadeToWhiteSpeed = 3f;

	private bool fadingToClear = false;
	private bool fadingToWhite = false;


	public void FinishedLoading() {
		loadingText.gameObject.SetActive(false);
		fadingToClear = true;
		// StartFadeToWhite();
	}

	public void StartFadeToWhite() {
		fadingToWhite = true;
		loadingImage.color = new Color(1f, 1f, 1f, 0f);	
	}

	// Use this for initialization
	void Start () {

	}
	
	void Update () {
		if (fadingToClear) {
			loadingImage.color = Color.Lerp(loadingImage.color, Color.clear, Time.deltaTime/2.5f);
			if (loadingImage.color.a <= 0.01f) {
				fadingToClear = false;
				loadingImage.color = Color.clear;
			}
		}else if (fadingToWhite) {
			loadingImage.color = Color.Lerp(loadingImage.color, Color.white, Time.deltaTime * fadeToWhiteSpeed);
			if (0.99f <= loadingImage.color.a) {
				fadingToWhite = false;
				loadingImage.color = Color.white;
			}
		} 
	}
}
