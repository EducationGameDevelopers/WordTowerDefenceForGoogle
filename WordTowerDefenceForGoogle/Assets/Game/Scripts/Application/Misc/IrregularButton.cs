using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IrregularButton : MonoBehaviour {

    private Image image;
    // Use this for initialization
    private void Awake()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.1f;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
