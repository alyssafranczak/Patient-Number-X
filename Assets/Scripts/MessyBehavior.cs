using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        //this.transform.localScale = new Vector3(.5f, .5f, .5f);
        Invoke("ChangeTag", 1);
    }

    private void ChangeTag()
    {
        this.tag = "MessySheets";
    }
}
