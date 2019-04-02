using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void ScanOnCollision()
    {
        AstarPath.active.Scan();
    }
}
