using UnityEngine;
using System.Collections;

public class testdelete : MonoBehaviour {


    private UISprite sprite1;
	// Use this for initialization
	void Start () {
        sprite1 = gameObject.GetComponent<UISprite>();
        sprite1.GetComponent<Button>().onClick = ClickDown;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ClickDown(GameObject go)
    {
        sprite1.color = Color.red;
    }


}
