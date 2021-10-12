using System;
using System.Collections;
using Game;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int score;
    public static int lives;
    public static int keys;
    public static bool hasTorch;
    public static bool hasSword;
    public static bool hasHammer;

    public ItemManager itemManager;

    public static GameManager gm;

    private void Awake()
    {
        if (gm != null && gm != this)
            Destroy(gameObject);
        gm = this;
        DontDestroyOnLoad(gameObject);
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
}
