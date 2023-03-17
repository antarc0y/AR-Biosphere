using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
	public GameObject pop_up_panel;
	public Animator dialogUI;

	public void open_popup(){
		pop_up_panel.SetActive(true);
		dialogUI.SetTrigger("pop");
	}

	public void close_popup(){
		pop_up_panel.SetActive(false);
		dialogUI.SetTrigger("close");
	}
   // Start is called before the first frame update

}
