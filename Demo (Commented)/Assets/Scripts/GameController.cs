using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {


	[Header("Misc Variables")]
	public int score;					// Holds current score
	public int savedHighScore;			// Holds current saved high score (from prefs)
	public int currentHighScore;		// Holds current high score (live in game)
	public bool gameOver = false;		// Is the game over?

	[Header("PreFabs")]
	public GameObject ball;				// Holds the ball prefab

	[Header("Buttons")]
	public Button btnPlay;				// Holes the Start Game button

	[Header("Text")]
	public Text txtScore;				// Score label (value part only)
	public Text txtHighScore;			// High Score label (value part only)
	public Text txtGameOver;			// Game over label


	// The following sets up a Singleton of the GameCOntroller class.  Creates an instance of the class so public intities can be called from other scripts.  
	static GameController _instance;

	public static GameController instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<GameController> ();
			}
			return _instance;
		}				
	}

	// Load saved player preferences.  In this case, it only holds the high score
	public void LoadUserPrefs() {

		savedHighScore = PlayerPrefs.GetInt ("highscore", 0);	// Read the saved high score.  0 is the default (second parameter)
		currentHighScore = savedHighScore;						// Set the current high score to the read saved high score
		SaveUserPrefs();										// Save the player prefs in case this is the first read, so 0 will be written
	}

	// Save current values to player preferences.  In this case, only high score
	public void SaveUserPrefs() {
		PlayerPrefs.SetInt ("highscore", currentHighScore);		// Copy current high score to the "highscore" pref variable
		PlayerPrefs.Save ();									// Save the player prefs
	}



	// Run when game first started
	void Start() {
		LoadUserPrefs ();										// Load the player preferences
		btnPlay.gameObject.SetActive (true);					// Show the "Start Game" button
		ball.gameObject.SetActive (false);						// Hide the ball
		txtGameOver.gameObject.SetActive (false);				// Hide the "Game Over" text
		txtHighScore.text = currentHighScore.ToString ("n0");	// Set the high score value text to the current high score read from player prefs
	}


	// Update the current running score
	public void UpdateScore () {
		score++;								// Add one to the score
		txtScore.text = score.ToString ("n0");	// Update the current running score value text
	}


	// Update the high score value text to current high score.  Could actually move this code to UpdateHighScores() 
	void ResetHighScore(int hiScr) {
		currentHighScore = hiScr;								// Set current high score to passed value
		//		if (currentHighScore > savedHighScore) {		// Not needed, was testing something
		txtHighScore.text = currentHighScore.ToString ("n0");	// Set high score value text to current high score
		//		}												// Not needed, was testing something
	}


	// Update the high score value
	void UpdateHighScores() {
		if ((score > currentHighScore) && (score > savedHighScore)) {	// If the current running score is grater than the current high score and the saved high score (double check)
			currentHighScore = savedHighScore = score;					// Set current and save high scores to current running score
			SaveUserPrefs ();											// Write it to the player preferences
		}
		ResetHighScore (currentHighScore);								// Change the displayed high score
	}


	// Set ball starting position, random on the horizontal from -9 to 9
	void PositionBall() {
		Vector3 ballPosition = new Vector3(Random.Range(-9.0f, 9.0f), 5.0f, -0.1f);		// Set position, random -9 to 9, centers, just a bit in front of the Z
		//	Vector3 centeredBallPosition = new Vector3(0.45f, 5.0f, -0.1f);				// Comment out above and uncomment this to always start ball in the middle of the screen
		ball.transform.position = ballPosition;											// Set ball position
	}


	// Start the level.  Called when "Start Game" button is clicked.
	public void StartLevel() {
		gameOver = false;							// Flip off gameOver boolean
		UpdateHighScores ();						// Update the high score, just in case
		score = 0;									// Set current running score to 0
		txtScore.text = score.ToString ("n0");		// Update score value text
		btnPlay.gameObject.SetActive (false);		// Hide the button
		txtGameOver.gameObject.SetActive (false);	// Hide Game Over text
		PositionBall ();							// Position teh ball
		ball.gameObject.SetActive (true);			// Show the ball
	}

	// End of level.
	public void EndLevel() {
		UpdateHighScores ();						// Update high scores
		btnPlay.gameObject.SetActive (true);		// Activate Start Game button
		ball.gameObject.SetActive (false);			// Hide ball
		txtGameOver.gameObject.SetActive (true);	// Show Game Over text

	}

	// Runs every frame
	void Update() {
		if (gameOver) {			// is gameOver boolean is set
			EndLevel ();		// Run EndLevel()
		}
	}

	// Save preferences when game exits, just in case
	void OnDestroy() {
		SaveUserPrefs ();
	}

}