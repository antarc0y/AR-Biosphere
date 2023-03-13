using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dialogUI : MonoBehaviour
{
	public GameObject pop_up_panel;
	public Animator animator;

	public void open_popup(){
		pop_up_panel.SetActive(true);
		animator.SetTrigger("pop");
	}

	public void close_popup(){
		pop_up_panel.SetActive(false);
	}
   // Start is called before the first frame update

}
