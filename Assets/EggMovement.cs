using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

public class EggMovement : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private float x;
    private float y;
    Rigidbody rg;
    Transform tf;
    public GameObject EggShattered;
    public Text TextStarePar;
    public Text TextSoundPar;
    public Text TextKnockPar;
    GameController gameController;
    expressionHandler exHandler;
    FaceExpressionHandler faceHandler;
    [HideInInspector] public Vector3 touchPosition;
    private Transform initialTransform;
    [HideInInspector] public int touchCount = 0;

    EggParameter eggParameter;
    EggPhysicalAI eggPhysicalAI;

    float rotX;
    float rotY;
    public float RotationGain = 5;


    private string[] keywords = { "よしよし", "もしもし", "卵を探せ", "どこ" };

    private KeywordRecognizer m_Recognizer;

    // Use this for initialization
    public void ProcessInput()
    {
        //call a variable based on string name?
        if (touchCount == 1) exHandler.Expression(exHandler.notice);
        if (eggParameter.TotalParameter >= eggParameter.PhaseA)
        {
            if (!eggPhysicalAI.isBusy) eggPhysicalAI.RotateTowards(touchPosition);
        }
    }

    void Awake()
    {
        exHandler = GameObject.Find("IIncubate").transform.Find("EggExpression").gameObject.GetComponent<expressionHandler>();
        faceHandler = GetComponent<FaceExpressionHandler>();
        gameController = transform.parent.parent.gameObject.GetComponent<GameController>();

        tf = transform.parent;
        rg = transform.parent.gameObject.GetComponent<Rigidbody>();

        initialPosition = tf.position;
        initialRotation = tf.eulerAngles;

        eggParameter = GetComponent<EggParameter>();
        eggPhysicalAI = GetComponent<EggPhysicalAI>();
    }
    void Start()
    {
        initialPosition = transform.localPosition;

        m_Recognizer = new KeywordRecognizer(keywords);
        m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;
        m_Recognizer.Start();
    }

    void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds {1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        switch (args.text)
        {
            case "よしよし":
                Debug.Log(args.text);
                eggParameter.AddParameter(10, 0, 0);
                break;
            case "もしもし":
                Debug.Log(args.text);
                eggParameter.AddParameter(10, 0, 0);
                break;
            case "卵を探せ":
                Debug.Log(args.text);
                eggParameter.AddParameter(10, 0, 0);
                break;
            case "どこ":
                Debug.Log(args.text);
                eggParameter.AddParameter(10, 0, 0);
                break;
            default:
                break;
        }
        UpdateParameterText();

    }

    void RestartEgg()
    {
        transform.position = initialPosition;
        transform.eulerAngles = initialRotation;
        //transform.parent.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.parent.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void UpdateParameterText()
    {
        TextSoundPar.text = eggParameter.SoundParameter.ToString();
        TextStarePar.text = eggParameter.StareParameter.ToString();
        TextKnockPar.text = eggParameter.KnockParameter.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //x += Input.GetAxis("Horizontal");
        //y += Input.GetAxis("Vertical");
        //transform.localPosition = new Vector3(x, y, initialPosition.z);
        rotX = Input.GetAxis("Vertical") * RotationGain;
        rotY = Input.GetAxis("Horizontal") * RotationGain;

        transform.parent.gameObject.GetComponent<Rigidbody>().AddTorque(new Vector3(
                rotY,
                rotX,
                0));

        #region Testing Expressions
        //Testing Expressions
        if (Input.GetKeyDown(KeyCode.A))
        {
            exHandler.Expression(exHandler.question);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            exHandler.Expression(exHandler.sweat);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            exHandler.Expression(exHandler.notice);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            exHandler.Expression(exHandler.notice);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            faceHandler.Expression(faceHandler.blush);
        }

        //----end of Testing Expressions----
        #endregion

        if (Input.GetKeyDown(KeyCode.Q))
        {
            eggParameter.AddParameter(10, 0, 0);
            UpdateParameterText();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            eggParameter.AddParameter(0, 10, 0);
            UpdateParameterText();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            eggParameter.AddParameter(0, 0, 10);
            UpdateParameterText();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            eggParameter.ResetParameter();
            UpdateParameterText();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            RestartEgg();
        }
    }

    public void GoodEnding()
    {
        Debug.Log("Good Ending.");
        StartCoroutine(Hatch(true));
    }

    IEnumerator Hatch(bool good)
    {
        Vector3 initPos = tf.position;
        rg.isKinematic = true;
        float duration = 2.0f;
        float elapsedTime = 0;
        while(elapsedTime < duration)
        {
            rg.position = Vector3.Lerp(initPos, new Vector3(0, 1.0f, 0), elapsedTime/duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(2);
        Instantiate(EggShattered, transform.position, transform.rotation, transform.parent);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        if(good)
        {

        }
        else
        {

        }
        yield return new WaitForSeconds(2);
        gameController.ReproduceEgg();
        yield return null;
    }


    public void BadEnding()
    {
        Debug.Log("Bad Ending");
        StartCoroutine(Hatch(false));
    }

}
