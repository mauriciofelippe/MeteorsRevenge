using UnityEngine;

public class FireChainWall : MonoBehaviour
{

    public GameObject fireWall;
    public int delayOff = 2;
    public int delayOn = 2;
    private float _counter;
    private bool _show = true;
    
    private void Update()
    {
        _counter += Time.deltaTime;

        if (!(_counter >  (!_show ? delayOff : delayOn))) return;
        
        _show = !_show;
        _counter = 0;
        fireWall.SetActive(_show);
    }
    
    
}
