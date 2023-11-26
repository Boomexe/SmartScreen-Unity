using System;
using System.Collections;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class UpdateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dateText;
    [SerializeField] private TextMeshProUGUI funFactText;

    int lastHour;


    private readonly string[] months = {"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"};

    void Start()
    {
        funFactText.text = "";
    }

    void Update()
    {
        UpdateLiveText();
        MyInput();

        RunEveryHour();
    }

    void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(GetFactRequest("https://uselessfacts.jsph.pl/api/v2/facts/random"));
        }
    }

    void UpdateLiveText()
    {
        int currentHour = DateTime.Now.TimeOfDay.Hours;

        if (currentHour > 12)
        {
            currentHour = currentHour % 12;
        }

        timeText.text = $"{(currentHour >= 10 ? currentHour : "0"+currentHour)}:{(DateTime.Now.TimeOfDay.Minutes >= 10 ? DateTime.Now.TimeOfDay.Minutes : "0"+DateTime.Now.TimeOfDay.Minutes)}:{(DateTime.Now.TimeOfDay.Seconds >= 10 ? DateTime.Now.TimeOfDay.Seconds : "0"+DateTime.Now.TimeOfDay.Seconds)}";
        dateText.text = $"{DateTime.Now.DayOfWeek}, {months[DateTime.Now.Month-1]} {DateTime.Now.Day}, {DateTime.Now.Year}";
    }

    void RunEveryHour()
    {
        if (DateTime.Now.Hour != lastHour)
        {
            StartCoroutine(GetFactRequest("https://uselessfacts.jsph.pl/api/v2/facts/random"));
        }

        lastHour = DateTime.Now.Hour;
    }

    IEnumerator GetFactRequest(String uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch(webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(string.Format("Something went wrong: {0}", webRequest.error));
                    break;
                
                case UnityWebRequest.Result.Success:
                    FunFact fact = JsonConvert.DeserializeObject<FunFact>(webRequest.downloadHandler.text);
                    funFactText.text = fact.text;
                    break;
            }
        }
    }

    public class FunFact
    {
        public string id { get; set; }
        public string text { get; set; }
        public string source { get; set; }
        public string source_url { get; set; }
        public string language { get; set; }
        public string permalink { get; set; }
    }
}
