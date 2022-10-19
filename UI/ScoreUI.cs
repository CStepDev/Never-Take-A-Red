using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour 
{


    public Text scoreText;

    int playerScore; 

    PlayerCharacterManager playerManagerReference;



	//When the game starts the playerScore will be zero
	void Start () 
	{
        // I made the getting and storing of the PlayerCharacterReference a little more safe in a bug hunting
        // effort, the other more condensed way works but this feels more debuggable
        GameObject playerManagerObjectRef = GameObject.FindGameObjectWithTag("PlayerManager");
        playerManagerReference = playerManagerObjectRef.GetComponent<PlayerCharacterManager>();

        scoreText = gameObject.GetComponent<Text>();

    }


	void Update () 
	{
        playerScore = playerManagerReference.GetScore();

        scoreText.text = playerScore.ToString();
	}
}
