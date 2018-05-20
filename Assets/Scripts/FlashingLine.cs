using System.Collections;
using UnityEngine;

public class FlashingLine : MonoBehaviour {

	private Animator animator;
	int startHash = Animator.StringToHash("startFlash");

	void Start () {
		animator = GetComponent<Animator>();
	}
    
    public void Flash () {
		animator.SetTrigger(startHash);
	}
}
