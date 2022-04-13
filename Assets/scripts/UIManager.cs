using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]
    TextMeshProUGUI deathCounter_TMP;
    [HideInInspector]
    public int deathCounter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void updateDeathCounterUI()
    {
        deathCounter_TMP.text = deathCounter.ToString();
    }
}
