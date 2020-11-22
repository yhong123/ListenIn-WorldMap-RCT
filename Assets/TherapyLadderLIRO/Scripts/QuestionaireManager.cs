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
    public GameObject[] questionnaireStructure;

    [SerializeField]
    [Tooltip("Assigned statically")]
    private static int INDEXMIDQUESTIONNAIRE = 5;

    [SerializeField]
    private GameObject ButtonsGO;

    [SerializeField]
    private GameObject InputTextGO;

    [SerializeField]
    private Text inputText;

    [SerializeField]
    private GameObject SliderGO;

    [SerializeField]
    private GameObject sliderObject;

    [SerializeField]
    private Button buttonNext;

    [SerializeField]
    private Button infoButton;


    [SerializeField]
    private GameObject[] faceList;

    private int currQuestionaireCount = 0;


    private bool endSaving = false;

    private List<string> responses = new List<string>();
    private string formatResponse = "q{0}:{1}";

    //Saving private response from texts
    private string currResponse = "";
    //Saving private slider input
    private int currSliderValue;


    void Start()
    {

        int currStep = TherapyLIROManager.Instance.GetUserProfile.m_userProfile.m_QuestionaireUserProfile.questionnairStage;
        if (currStep == 1)
            currQuestionaireCount = INDEXMIDQUESTIONNAIRE + 1;
        else
            currQuestionaireCount = 0;

        ActivateQuestion(currQuestionaireCount);

    }

    private void ActivateQuestion(int questionNumber)
    {
        foreach (var item in questionnaireStructure)
        {
            item.SetActive(false);
        }

        SliderGO.SetActive(false);
        InputTextGO.SetActive(false);

        buttonNext.interactable = false;
        infoButton.interactable = false;

        RectTransform currTransform = sliderObject.GetComponent<RectTransform>();
        Vector3 position = currTransform.localPosition;
        position.x = 0;
        sliderObject.transform.localPosition = position;

        GameObject currQuestion = questionnaireStructure[currQuestionaireCount];
        if (currQuestion.tag == "QSlider")
        {
            SliderGO.SetActive(true);
            buttonNext.interactable = false;
            infoButton.interactable = true;
        }
        else if (currQuestion.tag == "QInput")
        {
            InputTextGO.SetActive(true);
            buttonNext.interactable = true;
            infoButton.interactable = true;
        }
        else
        {
            buttonNext.interactable = true;
            infoButton.interactable = true;
        }

        currQuestion.SetActive(true);

    }

    private IEnumerator FinishAndSaveHalf()
    {
        yield return new WaitForEndOfFrame();
        SaveToFile();
        yield return StartCoroutine(TherapyLIROManager.Instance.SaveHalfQuestionnaire(true));
        //endButton.interactable = true;
    }

    private IEnumerator FinishAndSaveSecondHalf()
    {
        yield return new WaitForEndOfFrame();
        SaveToFile();
        yield return StartCoroutine(TherapyLIROManager.Instance.SaveSecondHalfQuestionaire(true));
        //endButton.interactable = true;
    }

    private void SaveToFile()
    {
        string directory = GlobalVars.GetPathToLIROOutput(NetworkManager.UserId);
        string filename = string.Format("Questionaire_{0}.txt", TherapyLIROManager.Instance.GetCurrentTherapyCycle());
        string fullPath = Path.Combine(directory, filename);
       
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

        //SEND TO SERVER
        WWWForm form = new WWWForm();
        form.AddField("id_user", NetworkManager.UserId);
        form.AddField("file_name", filename);
        form.AddField("file_size", Encoding.ASCII.GetBytes(sb.ToString()).Length);
        form.AddField("folder_name", GlobalVars.OutputFolderName);
        form.AddBinaryData("file_data", Encoding.ASCII.GetBytes(sb.ToString()), filename);
        NetworkManager.SendDataServer(form, NetworkUrl.ServerUrlUploadFile);

    }

    public void onEndQuestionaireClicked()
    {
        MadLevel.LoadLevelByName("MainHUB");
    }

    #region Button Events
    public void SetCurrentFace(int faceIdx)
    {
        buttonNext.interactable = true;

        currSliderValue = faceIdx + 1;
        GameObject currFace = faceList[faceIdx];

        RectTransform currTransform = sliderObject.GetComponent<RectTransform>();
        Vector3 position = currTransform.localPosition;
        position.x = currFace.GetComponent<RectTransform>().localPosition.x;
        sliderObject.transform.localPosition = position;

    }

    public void SaveAnswerAndContinue()
    {
        buttonNext.interactable = false;
        infoButton.interactable = false;

        GameObject currQuestion = questionnaireStructure[currQuestionaireCount];
        if (currQuestion.tag == "QSlider")
        {
            responses.Add(string.Format(formatResponse, currQuestionaireCount.ToString(), currSliderValue.ToString()));
        }
        else if (currQuestion.tag == "QInput")
        {
            currResponse = inputText.text;
            responses.Add(string.Format(formatResponse, currQuestionaireCount.ToString(), currResponse));
        }

        currQuestionaireCount++;

        ActivateQuestion(currQuestionaireCount);

    }
    #endregion

}
