using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum QuestionType { InputText, Slider};

public class QuestionStructure
{
    public QuestionType qt;
    public string text;
    public Text uiText;
}

public class QuestionaireManager : MonoBehaviour
{

    [SerializeField]
    private List<QuestionStructure> listQS = new List<QuestionStructure>();

    [SerializeField]
    private GameObject inputTextGO;

    [SerializeField]
    private GameObject sliderGO;

    [SerializeField]
    private GameObject thankyouGO;

    [SerializeField]
    private Text displayTextQuestion;

    [SerializeField]
    private Text displayTextSlider;

    [SerializeField]
    private InputField inputText;

    [SerializeField]
    private Slider slider;

    private int questionsTotal;
    private int questionsCounter = 0;

    private List<string> responses = new List<string>();
    private string formatResponse = "q{0}:{1}";
    private string currResponse = "";

    void Start()
    {
        inputTextGO.SetActive(false);
        sliderGO.SetActive(false);
        listQS.Add(new QuestionStructure { qt = QuestionType.InputText, text = "Please describe your speech comprehension skills below", uiText = displayTextQuestion});
        listQS.Add(new QuestionStructure { qt = QuestionType.Slider   , text = "Please rate your speech comprehension skills below"    , uiText = displayTextSlider });
        questionsTotal = listQS.Count;
        PrepareNextQuestion();
    }

    public void PrepareNextQuestion()
    {
        if (questionsCounter == questionsTotal)
        {
            //End of questions.
            Debug.Log("Reached end of the questionaire");
            SaveToFile();
            return;
        }

        QuestionStructure currQuest = listQS[questionsCounter];

        currQuest.uiText.text = currQuest.text;
        currResponse = "";

        switch (currQuest.qt)
        {
            case QuestionType.InputText:
                inputTextGO.SetActive(true);
                break;
            case QuestionType.Slider:
                sliderGO.SetActive(true);
                break;
            default:
                break;
        }

    }

    private void ClearCurrentQuestion()
    {
        slider.value = 5;
        inputText.text = "";
        
    }

    private void SaveResponse()
    {
        responses.Add(string.Format(formatResponse,questionsCounter.ToString(),currResponse));
        questionsCounter++;
    }

    private void SaveToFile()
    {
        string directory = GlobalVars.GetPathToLIROOutput(NetworkManager.UserId);
        string filename = string.Format("Questionaire_{0}.txt", TherapyLIROManager.Instance.GetCurrentTherapyCycle());
        string fullPath = Path.Combine(directory, filename);
        try
        {
            using (StreamWriter sw = System.IO.File.CreateText(fullPath))
            {
                foreach (string line in responses)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public void OnNextClickedSlider()
    {
        currResponse = slider.value.ToString();
        SaveResponse();
        PrepareNextQuestion();
    }

    public void OnNextClickedQuestionaire()
    {
        currResponse = inputText.text;
        SaveResponse();
        PrepareNextQuestion();
    }

}
