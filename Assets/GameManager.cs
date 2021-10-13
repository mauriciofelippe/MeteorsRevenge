using System;
using System.Collections;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            print("Score: "+_score);
        }
    }
    public static int Lives;
    public static int Keys;
    public static bool HasTorch;
    public static bool HasSword;
    public static bool HasHammer;

    public ItemManager itemManager;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    internal void ResetGame()
    {
        Score = 0;
        Lives = 3;
        Keys = 0;
        HasTorch = false;
        HasSword = false;
        HasHammer = false;

        SceneManager.LoadScene(0);
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    private IEnumerator Init()
    {
        itemManager = Instantiate(itemManager);
        yield return itemManager.isActiveAndEnabled;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }
}
