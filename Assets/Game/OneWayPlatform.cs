using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{

    private PlatformEffector2D _platformEffector2D;

    private void Start()
    {
        _platformEffector2D = GetComponent<PlatformEffector2D>();
    }

    public void TurnPlatformOff()
    {
        _platformEffector2D.rotationalOffset = 180;
        Invoke(nameof(TurnPlatformOn),0.5f);
    }
    
    private void TurnPlatformOn()
    {
        _platformEffector2D.rotationalOffset = 0;
    }
}
