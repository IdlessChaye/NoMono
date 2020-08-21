using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPerspectiveCam : MonoBehaviour {
    public GameObject characterPerspectiveCam;
	void Start() {
        characterPerspectiveCam.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.LeftShift))
            characterPerspectiveCam.SetActive(true);
        else
            characterPerspectiveCam.SetActive(false);

    }
}
