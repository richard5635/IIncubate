using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPhysicalAI : MonoBehaviour
{
    EggMovement eggMovement;
    EggParameter eggParameter;
    int parSum;
    Vector3 pos;
    Vector3 initPos;
    Vector3 initRot;
    Rigidbody rg;
    Transform tf;
    public PID PIDx;
    public PID PIDy;
    public PID PIDz;
    private float minAngle = 2.0f;
    bool isBalancing = true;
	[HideInInspector]public bool isBusy = false;

    IEnumerator StandUp()
    {
        yield return null;
    }

    // Use this for initialization
    void Awake()
    {
        eggMovement = GetComponent<EggMovement>();
        eggParameter = GetComponent<EggParameter>();

        initPos = transform.localPosition;
        initRot = transform.eulerAngles;
        pos = transform.position;
        rg = transform.parent.gameObject.GetComponent<Rigidbody>();
        tf = transform.parent;

        //Standard PID
        PIDx = new PID(0.4f, 0, 2f);
        PIDy = new PID(0.4f, 0, 2f);
        PIDz = new PID(0.4f, 0, 2f);
    }

    void Start()
    {
        DecideBehavior();
    }
    // Update is called once per frame
    void Update()
    {
        if (isBalancing) SelfBalancing();
    }

    void SelfBalancing()
    {
        //Debug.Log(tf.eulerAngles);

        Vector3 eggEuler = new Vector3(
            mapto180(tf.eulerAngles.x),
            mapto180(tf.eulerAngles.y),
            mapto180(tf.eulerAngles.z)
        );

        rg.AddTorque(new Vector3(
            PIDx.Update(initRot.x, eggEuler.x, 1.0f),
            0,
            PIDz.Update(initRot.z, eggEuler.z, 1.0f)
        ));
        //Debug.Log("Torque Added at : " + PIDx.Update(0, eggEuler.x, 1.0f) + ", " + 0 + ", " + PIDz.Update(0, eggEuler.z, 1.0f));
    }

    float mapto180(float s)
    {
        // if(s >= 0.0f && s <= 180.0f)return s;
        // else if( s > 180.0f && s < 360.0f) return s-360;
        // else return s;
        if (s > 180) return s - 360;
        else return s;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        // if (s > a2) return b2;
        // else if (s < a1) return b1;
        return b1 + (s - a1) / (a2 - a1) * (b2 - b1);
    }


    void DecideBehavior()
    {
		if(isBusy) return;
        switch (Random.Range(0, 5))
        {
            case 0:
                StartCoroutine(Shake());
                break;
            case 1:
                if (eggParameter.TotalParameter >= eggParameter.PhaseA)
                {
                    StartCoroutine(Roll());
                }
                else
                {
                    DecideBehavior();
                }
                break;
            case 2:
                if (eggParameter.TotalParameter >= eggParameter.PhaseB)
                {
                    StartCoroutine(Jump());
                }
                else
                {
                    DecideBehavior();
                }
                break;
            default:
                StartCoroutine(Idle());
                break;
        }
        if (eggParameter.TotalParameter >= eggParameter.PhaseA && eggParameter.TotalParameter < eggParameter.PhaseB)
        {

        }
        if (eggParameter.TotalParameter >= eggParameter.PhaseB && eggParameter.TotalParameter < eggParameter.PhaseC)
        {

        }
        if (eggParameter.TotalParameter >= eggParameter.PhaseC)
        {

        }
        // int decider = Random.Range(0,5);
        // switch (0)
        // {
        // 	case 0:
        // 		StartCoroutine(Shake());
        // 		break; 
        // 	case 1:
        // 		StartCoroutine(Wiggle());
        // 		break;
        // 	case 2:
        // 		StartCoroutine(Roll());
        // 		break;
        // 	case 3:
        // 		StartCoroutine(Jump());
        // 		break;
        // }
    }

    IEnumerator Idle()
    {
        yield return new WaitForSeconds(1.0f);
        DecideBehavior();
        yield return null;
    }

    IEnumerator Shake()
    {
        float torque;
        Vector3 randTorque;
        float randDist = Random.Range(1.0f, 2.0f);
        if (eggParameter.TotalParameter >= eggParameter.PhaseA && eggParameter.TotalParameter < eggParameter.PhaseB)
        {
            torque = 4;
        }
        else if (eggParameter.TotalParameter >= eggParameter.PhaseB && eggParameter.TotalParameter < eggParameter.PhaseC)
        {
            torque = 6;
        }
        else if (eggParameter.TotalParameter >= eggParameter.PhaseC)
        {
            torque = 8;
        }
        else
        {
            torque = 0;
        }
        Debug.Log("Shake Start");
        //Distribute Torque
        randTorque = new Vector3(
            torque / randDist,
            0.2f * (torque / randDist),
            (torque / randDist)
        );

        yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));
        rg.AddTorque(new Vector3(randTorque.x, randTorque.y, randTorque.z));
        yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));
        rg.AddTorque(new Vector3(-randTorque.x, -randTorque.y, -randTorque.z));
        yield return new WaitForSeconds(Random.Range(1.5f, 3.0f));
        Debug.Log("Shake Finishes.");
        //StopCoroutine(Shake());
        DecideBehavior();
        yield return null;
    }

    IEnumerator Wiggle()
    {
        Debug.Log("Wiggle Start");
        DecideBehavior();
        yield return null;
    }

    IEnumerator Roll()
    {
        float torque = 4f;
        isBalancing = false;
        Debug.Log("Roll Start");
        rg.AddTorque(torque, 0, 0);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < 3; i++)
        {
            rg.AddTorque(0, torque, 0);
            yield return new WaitForSeconds(3.0f);
        }
        isBalancing = true;
        DecideBehavior();
        yield return null;
    }

    IEnumerator Jump()
    {
        float forceJ = 4f;
        Debug.Log("Jump Start");
        // if (Standing())
        // {
        //     rg.AddForce(new Vector3(0, 20.0f, 0));
        // }
		float elapsedTime = 0;
		float time = 0.5f;
		while(elapsedTime < time)
		{
			rg.AddRelativeForce(new Vector3(0, forceJ, 0));
			elapsedTime += Time.deltaTime;
		}
        DecideBehavior();
        yield return null;
    }
    public void RotateTowards(Vector3 target)
    {
        StartCoroutine(LookTowards(target, 0.5f));
    }
    IEnumerator LookTowards(Vector3 target)
    {
        Debug.Log("The Egg notices you!");
        isBalancing = false;
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, target, Time.deltaTime * 5);
        yield return new WaitForSeconds(3);
        isBalancing = true;
        yield return null;
    }

	void nullifyForce()
	{
		rg.velocity = Vector3.zero;
    	rg.angularVelocity = Vector3.zero;
	}

	//Input Delay

	IEnumerator Delay(float time)
	{
		isBusy = true;
		yield return new WaitForSeconds(time);
		isBusy = false;
		DecideBehavior();
	}
    IEnumerator LookTowards(Vector3 target, float time)
    {
		//if(isDelayTouch)StopCoroutine(LookTowards());
		//Fix
		isBusy = true;
		nullifyForce();
		Vector3 newDirection = new Vector3(
			target.x - tf.position.x,
			0,
			target.z - tf.position.z
		);
        float elapsedTime = 0;
        Debug.Log("The Egg notices you!");
		rg.useGravity = false;
        isBalancing = false;
		Vector3 stPos = tf.position;
		Vector3 stRot = tf.eulerAngles;


		//Stand Up
		//Land Position
		float stHeight = 0.446f;
		while(elapsedTime < 0.2f)
		{
			tf.position = Vector3.Lerp(stPos, new Vector3(stPos.x, stPos.y+0.1f, stPos.z), (elapsedTime/1) * 10);
			//tf.eulerAngles = Vector3.Lerp(stRot, new Vector3(0, 0, 0), elapsedTime * 3);
			tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.LookRotation(Vector3.zero), elapsedTime * 10);
			elapsedTime += Time.deltaTime;
			//Debug.Log("Elapsed Time: " + elapsedTime);
			yield return new WaitForEndOfFrame();
		}
		rg.useGravity = true;
		nullifyForce();
		elapsedTime = 0;
		yield return new WaitForSeconds(0.2f);
        while(elapsedTime < time)
        {
        	tf.rotation = Quaternion.Lerp(tf.rotation, Quaternion.LookRotation(newDirection), elapsedTime * 4);
        	elapsedTime +=Time.deltaTime;
        	yield return new WaitForEndOfFrame();
        }
		Debug.Log("The Egg stops looking at you.");
		yield return null;

		//Rotate
		// while(elapsedTime < time)
        // {
        // 	tf.eulerAngles = Vector3.Lerp(tf.eulerAngles, new Vector3
		// 	(
		// 		tf.eulerAngles.x,
		// 		(target.y)
		// 		tf.eulerAngles.z
		// 	), 
		// 	Time.deltaTime);
        // 	elapsedTime +=Time.deltaTime;
        // 	yield return null;
        // }
        

        // while (elapsedTime < time)
        // {
        //     Vector3 eggEuler = new Vector3(
        //     mapto180(tf.eulerAngles.x),
        //     mapto180(tf.eulerAngles.y),
        //     mapto180(tf.eulerAngles.z)
        //     );

        //     rg.AddTorque(new Vector3(
        //     PIDx.Update(target.x, eggEuler.x, 1.0f),
        //     PIDx.Update(target.y, eggEuler.x, 1.0f),
        //     PIDz.Update(target.z, eggEuler.z, 1.0f)
        // 	));

		// 	elapsedTime += Time.deltaTime;
        //     yield return null;
        // }
		isBalancing = true;
		StartCoroutine(Delay(1));
    }

    bool Standing()
    {
        if (rg.angularVelocity == Vector3.zero &&
            tf.eulerAngles.x < minAngle && tf.eulerAngles.x > -minAngle &&
            tf.eulerAngles.y < minAngle && tf.eulerAngles.y > -minAngle &&
            tf.eulerAngles.z < minAngle && tf.eulerAngles.z > -minAngle) return true;
        return false;
    }
}
