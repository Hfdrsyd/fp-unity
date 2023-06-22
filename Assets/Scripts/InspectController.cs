using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InspectController : MonoBehaviour
{
    [SerializeField] GameObject objectNameBG;
    [SerializeField] Text objectNameUI;

    [SerializeField] float onScreenTimer;
    [SerializeField] Text extraInfoUI;
    [SerializeField] GameObject extraInfoBG;
    [HideInInspector] bool startTimer;
    float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        objectNameBG.SetActive(false);
        extraInfoBG.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startTimer)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                timer = 0;
                ClearAdditionalInfo();
                startTimer = false;
            }
        }
    }

    void ShowName(string objectName)
    {
        objectNameBG.SetActive(true);
        objectNameUI.text = objectName;
    }

    void HideName()
    {
        objectNameBG.SetActive(false);
        objectNameUI.text = "";
    }

    void ShowAdditionalInfo(string newInfo)
    {
        timer = onScreenTimer;
        startTimer = true;
        extraInfoBG.SetActive(true);
        extraInfoUI.text = newInfo;
    }

    void ClearAdditionalInfo()
    {
        extraInfoBG.SetActive(false);
        extraInfoUI.text = "";
    }
}
