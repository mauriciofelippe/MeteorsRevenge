using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private Transform player;
    private Camera cam;
    private Vector3 playerPosOnScreen = new Vector3();

    private Vector2 playerScreenMax = new Vector2();
    private const int WidthInTiles = 25;
    private const int HeighInTiles = 14;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        cam = GetComponent<Camera>();
        playerScreenMax = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    private void Update()
    {
        // playerScreenMax = new Vector2(Screen.width, Screen.height);
        playerPosOnScreen = cam.WorldToScreenPoint(player.position);
        // print(playerPosOnScreen);

        if (playerPosOnScreen.x < 0)
        {
            transform.position = new Vector3(transform.position.x - WidthInTiles, transform.position.y, transform.position.z);
        }
        else if(playerPosOnScreen.x > playerScreenMax.x)
        {
            transform.position = new Vector3(transform.position.x + WidthInTiles, transform.position.y,transform.position.z);
        }
        else if(playerPosOnScreen.y < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y-HeighInTiles,transform.position.z);
        }
        else if (playerPosOnScreen.y > playerScreenMax.y)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+HeighInTiles, transform.position.z);
        }
        
    }
}
