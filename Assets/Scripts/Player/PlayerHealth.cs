using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    // Start is called before the first frame update
    public GameObject cam;

    public override void Death()
    {
        cam.transform.parent = null;
        Manager.SharedInstance.GameOver();
        base.Death();
    }
}
