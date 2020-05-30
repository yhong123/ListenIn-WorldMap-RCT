using UnityEngine;
using System.Collections;

public class SplashMono : MonoBehaviour
{
    [SerializeField] private GameObject m_SplashScreen;
//    m_NHSGroup,
//    m_SoftVGroup;
    public GameObject SplashScreen { get { return m_SplashScreen; } }
//    public GameObject NHSGroup { get { return m_NHSGroup; } }
//    public GameObject SoftVGroup { get { return m_SoftVGroup; } }

    [SerializeField]
	private GameObject m_MainMenu, m_Options, m_Doctor;//, m_Intro;
    public GameObject MainMenu { get { return m_MainMenu; } }
	//public GameObject Intro { get { return m_Intro; } }
    public GameObject OptionsScreen { get { return m_Options; } }
    public GameObject DoctorsScreen { get { return m_Doctor; } }
    
    public void StartGame()
    {
        StateSplash.Instance.StartGame();
    }
    public void GoToMainMenu()
    {
        StateSplash.Instance.GoToMainMenu();
    }
    public void GoToOptionMenu()
    {
        StateSplash.Instance.GoToOptionMenu();
    }
	public void GoToIntro()
	{
		StateSplash.Instance.GoToGameIntro();
	}
    public void GoToDoctor()
    {
        StateSplash.Instance.GoToDoctor();
    }
}
