using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameControler : MonoBehaviour
{
    public static GameControler Instance;

    public static int Points { get; private set; }
    public static bool GameStarted { get; private set; }

    [SerializeField]
    private TextMeshProUGUI pointsText;

    private void Awake() {
        if (Instance == null){
            Instance = this;
        }
    }

    private void Start() {
        StartGame();
    }

    private void StartGame(){
        SetPoints(0);
        GameStarted = true;
    }

    public void AddPoints(int points){
        SetPoints(Points + points);
    }

    public void SetPoints(int points){
        Points = points;
        pointsText.text = Points.ToString();
    }
}
