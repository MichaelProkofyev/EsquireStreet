using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;


public class Social : MonoBehaviour {

	public void ShareScreenShot () {
		if (!FB.IsInitialized) {
			FB.Init(this.OnInitComplete);
		}else {
			if (!FB.IsLoggedIn) {
				FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends", "publish_actions" }, this.HandleResult);
			}else {
				StartCoroutine(ShareImageShot());
			}
		}
	}

	private void OnInitComplete()
        {
            // this.Status = "Success - Check log for details";
            // this.LastResponse = "Success Response: OnInitComplete Called\n";
            string logMessage = string.Format(
                "OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
                FB.IsLoggedIn,
                FB.IsInitialized);
            Debug.Log("FB " + logMessage);
			FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends", "publish_actions" }, this.HandleResult);
        }
	private void HandleResult(IResult result) {
            // Debug.Log("FB LOGIN RESULT " + result.RawResult);
			if (result.Cancelled) {
				Debug.Log("FB LOGIN FAILED");
			}else {
				Debug.Log("FB LOGIN SUCCESS");
				StartCoroutine(ShareImageShot());

			}
	}


	IEnumerator ShareImageShot() {
 
		yield return new WaitForEndOfFrame();

		var width = Screen.width;
		var height = Screen.height;
		var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		// Read screen contents into the texture
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();
		byte[] screenshot = tex.EncodeToPNG();

		var wwwForm = new WWWForm();
		wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");

		FB.API("me/photos", HttpMethod.POST, null, wwwForm);
 
     }

}
