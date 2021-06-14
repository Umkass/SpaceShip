using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusText : MonoBehaviour
{
    Text statusText;

    private void Awake()
    {
        statusText = GetComponent<Text>();
        GetComponent<CanvasRenderer>().SetAlpha(0);
    }
  
    public IEnumerator ChangeStatus(string displayText)
    {
        statusText.text = displayText;
        statusText.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(1.01f);
        statusText.CrossFadeAlpha(0, 1f, false);
        yield return new WaitForSeconds(1.01f);
        gameObject.SetActive(false);
    }
}
