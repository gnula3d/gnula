using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void sponsor()
	{
		Application.ExternalEval ("window.open('sponsor.html','contenido')");
	}

}
