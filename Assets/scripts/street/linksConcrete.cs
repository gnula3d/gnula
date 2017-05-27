using UnityEngine;
using System.Collections;

public class linksConcrete : MonoBehaviour {


	public void sponsor()
	{
		Application.ExternalEval ("window.open('http://gnula3d.esy.es/sponsor/','contenido')");
	}
}
