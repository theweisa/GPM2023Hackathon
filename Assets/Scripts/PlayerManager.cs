using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : UnitySingleton<PlayerManager>
{
    public PlayerController controller;
    // Start is called before the first frame update
    override public void Awake()
    {
        base.Awake();
        controller = controller ? controller : Global.FindComponent<PlayerController>(gameObject);
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
