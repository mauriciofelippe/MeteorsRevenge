using System;
using Game;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public PlayerController player;
    private Camera cam;
    private Vector3 playerPosOnScreen = new Vector3();

    private Vector2 playerScreenMax = new Vector2();
    private const int WidthInTiles = 25;
    private const int HeighInTiles = 14;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        Application.targetFrameRate = 60;
        
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif
        Screen.SetResolution(1600,896,FullScreenMode.ExclusiveFullScreen);
        playerScreenMax = new Vector2(Screen.width, Screen.height);        
    }

    
    // Update is called once per frame
    private void Update()
    {
        // playerScreenMax = new Vector2(Screen.width, Screen.height);
        playerPosOnScreen = cam.WorldToScreenPoint(player.transform.position);
        // print(playerPosOnScreen);

        if (playerPosOnScreen.x < 0)
        {
            transform.position = new Vector3(transform.position.x - WidthInTiles, transform.position.y, transform.position.z);
            player.lastPortal = player.transform.position + Vector3.left;
        }
        else if(playerPosOnScreen.x > playerScreenMax.x)
        {
            player.lastPortal = player.transform.position + Vector3.right;
            transform.position = new Vector3(transform.position.x + WidthInTiles, transform.position.y,transform.position.z);
        }
        else if(playerPosOnScreen.y < 0)
        {
            player.lastPortal = player.transform.position + Vector3.down;
            transform.position = new Vector3(transform.position.x, transform.position.y-HeighInTiles,transform.position.z);
        }
        else if (playerPosOnScreen.y > playerScreenMax.y)
        {
            player.lastPortal = player.transform.position + Vector3.up;
            transform.position = new Vector3(transform.position.x, transform.position.y+HeighInTiles, transform.position.z);
        }
        
    }
}
