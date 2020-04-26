using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MadLevelManager;
using System.Text;

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

    [SerializeField]
    private Button endButton;

    private bool endSaving = false;

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
        if (questionsCounter >= questionsTotal && !endSaving)
        {
            //End of questions.
            Debug.Log("Reached end of the questionaire");
            endSaving = true;
            thankyouGO.SetActive(true);
            StartCoroutine(FinishAndSave());
            return;
        }
        else
        {
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
    }

    private IEnumerator FinishAndSave()
    {
        yield return new WaitForEndOfFrame();
        SaveToFile();
        yield return StartCoroutine(TherapyLIROManager.Instance.SaveCurrentQuestionaire(true));
        endButton.interactable = true;
    }

    private void ClearCurrentQuestion()
    {
        slider.value = 4;
        inputText.text = "";
        inputTextGO.SetActive(false);
        sliderGO.SetActive(true);

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
            StringBuilder sb = new StringBuilder();
            using (StreamWriter sw = System.IO.File.CreateText(fullPath))
            {
                foreach (string line in responses)
                {
                    sw.WriteLine(line);
                    sb.AppendLine(line);
                }
                sw.Close();
            }

            WWWForm form = new WWWForm();
            form.AddField("id_user", NetworkManager.UserId);
            form.AddField("file_name", filename);
            form.AddField("content", sb.ToString());

            NetworkManager.SendDataServer(form, NetworkUrl.ServerUrlDataInput, sb.ToString(), filename);

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
        ClearCurrentQuestion();
        PrepareNextQuestion();
    }

    public void OnNextClickedQuestionaire()
    {
        currResponse = inputText.text;
        SaveResponse();
        ClearCurrentQuestion();
        PrepareNextQuestion();
    }

    public void onEndQuestionaireClicked()
    {
        MadLevel.LoadLevelByName("MainHUB");
    }

}
