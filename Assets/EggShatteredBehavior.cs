using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggShatteredBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(Hatch());
	}
	IEnumerator Hatch()
	{
		float duration = 0.5f;
		float elapsedTime = 0.0f;
		while(elapsedTime < duration)
		{
			for(int i = 0; i < transform.childCount; i++)
			{
				transform.GetChild(i).GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 20, 0));
				transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(30.0f,transform.position,4.0f, 2.0f);
				elapsedTime += Time.deltaTime;
			}
		}
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
		yield return null;
	}
}
