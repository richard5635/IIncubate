using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class expressionHandler : MonoBehaviour {
    public GameObject sweat;
    public GameObject notice;
    public GameObject question;
    private GameObject setExpression;
    private IEnumerator animate;

    //Call by using expressionHandler.Expression(expressionHandler.blush);

    //I can also set each animation to stop by including script right inside the animator.
    IEnumerator Animate(GameObject obj)
    {
        Debug.Log("Start Expression.");
        obj.GetComponent<Animator>().SetBool("Start",true);
        yield return new WaitForSeconds(2) ;
        Debug.Log("Stop Expression.");
        obj.GetComponent<Animator>().SetBool("Start", false);
        yield return null;
    }

    public void Expression(GameObject obj)
    {
        setExpression = obj;
        obj.SetActive(true);
        StartCoroutine("Animate", obj);
    }
}
