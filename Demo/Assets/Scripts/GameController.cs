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
	public GameObject ball;

	[Header("Buttons")]
	public Button btnPlay;

	[Header("Text")]
	public Text txtScore;
	public Text txtHighScore;
	public Text txtGameOver;


	static GameController _instance;

	public static GameController instance {
		get {
			if (_instance == null) {
				_instance = FindObjectOfType<GameController> ();
			}
			return _instance;
		}				
	}


	public void LoadUserPrefs() {

		savedHighScore = PlayerPrefs.GetInt ("highscore", 0);
		currentHighScore = savedHighScore;
		SaveUserPrefs();
	}


	public void SaveUserPrefs() {
		PlayerPrefs.SetInt ("highscore", currentHighScore);
		PlayerPrefs.Save ();
	}



	void Start() {
		LoadUserPrefs ();
		btnPlay.gameObject.SetActive (true);
		ball.gameObject.SetActive (false);
		txtGameOver.gameObject.SetActive (false);
		txtHighScore.text = currentHighScore.ToString ("n0");
	}



	public void UpdateScore () {
		score++;
		txtScore.text = score.ToString ("n0");
	}



	void ResetHighScore(int hiScr) {
		currentHighScore = hiScr;
		//		if (currentHighScore > savedHighScore) {
		txtHighScore.text = currentHighScore.ToString ("n0");
		//		}
	}



	void UpdateHighScores() {
		if ((score > currentHighScore) && (score > savedHighScore)) {
			currentHighScore = savedHighScore = score;
			SaveUserPrefs ();
		}
		ResetHighScore (currentHighScore);
	}


	void PositionBall() {
		Vector3 ballPosition = new Vector3(Random.Range(-9.0f, 9.0f), 5.0f, -0.1f);
		//	Vector3 centeredBallPosition = new Vector3(0.45f, 5.0f, -0.1f);
		ball.transform.position = ballPosition;		
	}

	public void StartLevel() {
		gameOver = false;
		UpdateHighScores ();
		score = 0;
		txtScore.text = score.ToString ("n0");
		btnPlay.gameObject.SetActive (false);
		txtGameOver.gameObject.SetActive (false);
		PositionBall ();
		ball.gameObject.SetActive (true);
	}


	public void EndLevel() {
		UpdateHighScores ();
		btnPlay.gameObject.SetActive (true);
		ball.gameObject.SetActive (false);
		txtGameOver.gameObject.SetActive (true);

	}


	void Update() {
		if (gameOver) {
			EndLevel ();
		}
	}


	void OnDestroy() {
		SaveUserPrefs ();
	}

}