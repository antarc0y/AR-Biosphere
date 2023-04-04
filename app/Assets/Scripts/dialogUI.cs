//using Codice.Client.Commands;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
	public GameObject pop_up_panel;
	public Animator dialogUI;
	//status = true, when popup is open, else false 
	public bool status;

	public void open_popup(){
		pop_up_panel.SetActive(true);
		status = true;
		dialogUI.SetTrigger("pop");
	}

	public void close_popup(){
		pop_up_panel.SetActive(false);
		status = false;
		dialogUI.SetTrigger("close");
	}
}
