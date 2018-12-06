using UnityEngine;
﻿using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;
    public float levelStartDelay = 2f;

    private int level = 1;
    private Text levelText;
    private GameObject levelImage; // To hide the level while it's being built
    private BoardManager boardScript;
    private SpawnManager spawnScript;
    private GameObject bomb;

    Animator anim;

    void Awake() {
      //Check if instance already exists
          if (instance == null) {

              //if not, set instance to this
              instance = this;
              Debug.Log("Instantiating game manager");
          }
          //If instance already exists and it's not this:
          else if (instance != this)

              //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
              Destroy(gameObject);

          //Sets this to not be destroyed when reloading scene
          DontDestroyOnLoad(gameObject);

          //Get a component reference to the attached BoardManager script
          boardScript = GetComponent<BoardManager>();
          spawnScript = GetComponent<SpawnManager>();
          anim = GetComponent<Animator>();
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        initGame();
        level++;
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        Debug.Log("Enabling scene");
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        Debug.Log("Disabling scene");
    }

    void initGame() {
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Stage " + level;

        levelImage.SetActive(true);

        Invoke("hideLevelImage", levelStartDelay);
        Debug.Log("Setting up level " + level);
        boardScript.setupScene(level);
        spawnScript.setParams();
    }

    void hideLevelImage() {
        levelImage.SetActive(false);
    }

    public void triggerBomb() {
      GameObject bomb = GameObject.FindGameObjectWithTag("Bomb");
      bomb.AddComponent<SeekBehavior>();
      bomb.GetComponent<SeekBehavior>().target = GameObject.FindGameObjectWithTag("Player");
      bomb.GetComponent<SeekBehavior>().speed = 3.0f;
      bomb.GetComponent<SeekBehavior>().initialHeight = 25.0f;

      bomb.GetComponent<Collider>().gameObject.SetActive(false);
    }

    public void gameOver() {
        Debug.Log("Game over");
        anim.SetTrigger("GameOver");

        enabled = false;
    }

    void restartLevel() {
        Debug.Log("Restarting scene");
        SceneManager.LoadScene(0);
    }

    public GameObject getTarget() {
        return boardScript.getTarget();
    }

    public void checkWinCondition() {
      if (ScoreManager.score >= boardScript.getNumPickups()) {
          Debug.Log("You win - starting next level");
          Invoke("restartLevel", levelStartDelay);
      }
    }

    public int getLevel() {
        return level;
    }

    // Update is called once per frame
    void Update() {

    }
}
