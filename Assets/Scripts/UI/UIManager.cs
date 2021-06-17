using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Tagline;
    // Start is called before the first frame update
    void Awake()
    {
        Tagline.GetComponent<CanvasRenderer>().SetAlpha(0);
        StartCoroutine(AlphaAnim());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartLevel(GameManager.Instance.currentLevelIndex);
        }
    }

    public IEnumerator AlphaAnim()
    {
        while (true)
        {
            Tagline.CrossFadeAlpha(1f, 1f, false);
            yield return new WaitForSeconds(1.01f);
            Tagline.CrossFadeAlpha(0, 1f, false);
            yield return new WaitForSeconds(1.01f);
        }
    }
}
