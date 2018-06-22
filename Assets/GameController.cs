using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    // Use this for initialization
    public float count = 0;
    bool cStarted = false;
    public Text TextCountdown;
	public GameObject Lid;
    EggParameter eggParameter;
    EggMovement eggMovement;
	EggPhysicalAI eggPhysicalAI;
	GameObject Egg;
	Rigidbody rg;
    void Awake()
    {
        eggParameter = GameObject.Find("Egg").transform.Find("clean").gameObject.GetComponent<EggParameter>();
        eggMovement = GameObject.Find("Egg").transform.Find("clean").gameObject.GetComponent<EggMovement>();
		eggPhysicalAI = GameObject.Find("Egg").transform.Find("clean").gameObject.GetComponent<EggPhysicalAI>();
		Egg = GameObject.Find("Egg");
		rg = Egg.GetComponent<Rigidbody>();
		//StartCoroutine(ReproduceEgg());
    }
    public IEnumerator CountDown()
    {
		if(eggParameter.isNew)cStarted = false;
        if (!cStarted)
        {
			cStarted = true;
            count = 60;
            while (count > 0)
            {
                Debug.Log("Counting.. " + count);
                TextCountdown.text = Mathf.Floor(count).ToString();
                yield return new WaitForSeconds(1);
                count -= 1;
            }
            if (count == 0)
            {
                DecideEnd();
            }
        }
		else{
			Debug.Log("Countdown has already started!");
		}
        yield return null;
    }
    void DecideEnd()
    {
		eggPhysicalAI.isBusy = true;
        if (eggParameter.TotalParameter >= eggParameter.PhaseB && eggParameter.TotalParameter < eggParameter.PhaseC)
        {
            eggMovement.GoodEnding();
        }
        else
        {
            eggMovement.BadEnding();
        }
    }

	public void ReproduceEgg()
	{
		StartCoroutine(IReproduceEgg());
	}

	IEnumerator IReproduceEgg()
	{
		//Cause of Error? Not yet known, but when called outside it doesnt work, so do smth with processing inside.
		Debug.Log("Reproducing Egg");
		float duration = 2.0f;
		float elapsedTime = 0;
		while(elapsedTime < duration)
		{
			Debug.Log(elapsedTime);
			Lid.transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, -1), elapsedTime/duration);
			elapsedTime += Time.deltaTime;
			yield return null;
			
		}
		yield return new WaitForSeconds(1);
		Debug.Log("Dropping New Egg!");
		Egg.transform.position = new Vector3(0, 3, 0);
		Egg.transform.Find("clean").GetComponent<MeshRenderer>().enabled = true;
		eggParameter.Initialize();
		rg.isKinematic = false;
		yield return null;
	}
}
