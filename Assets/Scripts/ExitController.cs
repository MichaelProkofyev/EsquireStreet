using UnityEngine;

public class ExitController : MonoBehaviour {

	public Transform playerTransform;
	public GameObject sky;
	public GameController uiController;
	private int sequence = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.anyKeyDown) {
			switch (sequence)
			{
				case 0:
					if (Input.GetKeyDown(KeyCode.I)) {
						sequence++;
					}else {
						sequence = 0;
					}		
					break;
				case 1:
					if (Input.GetKeyDown(KeyCode.D)) {
						sequence++;
					}else {
						sequence = 0;
					}
					break;
				case 2:
					if (Input.GetKeyDown(KeyCode.D)) {
						sequence++;
					}else {
						sequence = 0;
					}
					break;
				case 3:
					if (Input.GetKeyDown(KeyCode.Q)) {
						sequence++;
					}else {
						sequence = 0;
					}
					break;		
				case 4:
					if (Input.GetKeyDown(KeyCode.D)) {
						playerTransform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_CanJump = true;
						playerTransform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 10f, playerTransform.position.z);
						sky.SetActive(false);
						// playerTransform.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true; 
					}
					sequence = 0;
					break;		
			}
			// Debug.Log(sequence);
		}
	}

	void OnTriggerEnter(Collider other) {
		other.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        uiController.StartFadeToWhite();
		uiController.ShowFinishPanel();
		
    }
}
