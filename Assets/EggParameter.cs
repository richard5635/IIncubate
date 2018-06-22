using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggParameter : MonoBehaviour
{
	[HideInInspector]public bool isNew = true;
    GameController gameController;
	EggMovement eggMovement;
	public int SoundParameter;
    public int KnockParameter;
    public int StareParameter;
    public int TotalParameter;

    public int hatchgoodlimit = 25;
    public int hatchbadlimit = 75;

    public int PhaseA = 75;
    public int PhaseB = 135;
    public int PhaseC = 180;
	public int PhaseD = 200;

	void Awake()
	{
		gameController = GameObject.Find("IIncubate").GetComponent<GameController>();
		eggMovement = GetComponent<EggMovement>();
	}

    public void Initialize()
    {
        SoundParameter = 0;
        KnockParameter = 0;
        StareParameter = 0;
		AddParameter(0,0,0);
		eggMovement.UpdateParameterText();
    }

    public void AddParameter(int sd, int kn, int st)
    {
        // int SoundParameterII;
        // int KnockParameterII;
        // int StareParameterII;

        SoundParameter += sd;
        KnockParameter += kn;
        StareParameter += st;

        // SoundParameterII = (SoundParameter > 25 ? SoundParameter - 25 : 0) + (SoundParameter > 75 ? (int)(SoundParameter * (-0.2f)) : 0);
        // KnockParameterII = (KnockParameter > 25 ? KnockParameter - 25 : 0) + (KnockParameter > 75 ? (int)(SoundParameter * (-0.2f)) : 0);
        // StareParameterII = (StareParameter > 25 ? StareParameter - 25 : 0) + (StareParameter > 75 ? (int)(SoundParameter * (-0.2f)) : 0);


        TotalParameter = SoundParameter + KnockParameter + StareParameter;

        CheckParameter();
    }

    void CheckParameter()
    {
        //Case 1 : Just show three different patterns
        if (TotalParameter > PhaseA && TotalParameter <= PhaseB)
        {
            //StartCoroutine("PatternA");
        }
        else if (TotalParameter > PhaseB && TotalParameter <= PhaseC)
        {
            //StartCoroutine("PatternB");
            //Pattern where it is overtreated
            //StartCorouting("PatternD");
			StartCoroutine(gameController.CountDown());
			isNew = false;
        }
        else if (TotalParameter <= PhaseA)
        {
            //StartCoroutine("StayQuiet");
        }
        else if (TotalParameter > PhaseC)
        {
            //StartCoroutine("ReadytoHatch");
        }

        //Case 2 : Create different reactions based on which parameter is higher. For now just do Case 1.
    }

    public void DetermineHatch()
    {
        if (TotalParameter >= 135)
        {
            //Successful
        }
        else
        {
            //Fail
        }
    }

    public void ResetParameter()
    {
        SoundParameter = 0;
        KnockParameter = 0;
        StareParameter = 0;
        TotalParameter = 0;
        CheckParameter();
    }
}
