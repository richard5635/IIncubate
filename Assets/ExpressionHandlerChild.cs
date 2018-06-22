using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionHandlerChild : MonoBehaviour {

	private void OnEnable()
    {
        StartCoroutine("Animate");
    }

    IEnumerable Animate()
    {
		Debug.Log("Animate enabled.");
        yield return new WaitForSeconds(2) ;
        gameObject.SetActive(false);
        yield return null;
    }
}
