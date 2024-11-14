using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandLight : MonoBehaviour
{
    private Light playerLight;
    // Start is called before the first frame update
    private void Start()
    {
        playerLight = GetComponent<Light>();   
    }

    public void lightExpansion() {
        playerLight.range = 30;
    }
}
