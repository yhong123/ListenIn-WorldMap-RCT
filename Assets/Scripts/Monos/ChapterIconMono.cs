using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChapterIconMono : MonoBehaviour
{

    public event System.Action<int> OnPressedButton;

    [SerializeField]
    private Image m_Image;
    public Image Image { get { return m_Image; } }

	[SerializeField]
	public Sprite m_nextLvl_Image;
	//public Sprite NextLvl_Image { get { return m_nextLvl_Image; } }
	[SerializeField]
	public Sprite m_nextLvl_Image_small;

	[SerializeField]
	private Image m_Completed_Image;
	public Image Completed_Image { get { return m_Completed_Image; } }

	[SerializeField]
	private Image m_title_Image;
	public Image Title_Image { get { return m_title_Image; } }

    [SerializeField]
    private Text m_Title, m_Percentage;
    public Text Title { get { return m_Title; } }
    public Text PlayText { get { return m_Percentage; } }

	[SerializeField]
	private GameObject m_playButton;
	public GameObject PlayButton
	{
		get { return m_playButton; }
	}

    [SerializeField]
    private Image m_BronzeReward;
    public Image BronzeReward
    {
        get { return m_BronzeReward; }
    }

    [SerializeField]
    private Image m_SilverReward;
    public Image SilverReward
    {
        get { return m_SilverReward; }
    }

    [SerializeField]
    private Image m_GoldReward;
    public Image GoldReward
    {
        get { return m_GoldReward; }
    }

    [SerializeField]
	private Sprite m_playSprite; 
	public Sprite PlaySprite
	{
		get { return m_playSprite; }
	}

	[SerializeField]
	private Sprite m_playLockedSprite; 
	public Sprite PlayLockedSprite
	{
		get { return m_playLockedSprite; }
	}

	[SerializeField]
	private GameObject m_JigsawPieces_root;
	public GameObject JigsawPieceRoot
	{
		get { return m_JigsawPieces_root; }
	}

    [SerializeField]
    private GameObject[] m_JigsawPieces;
    public GameObject[] JigsawPieces
    {
        get { return m_JigsawPieces; }
    }

    public int ID;

    public void PressButton()
    {
        if (OnPressedButton != null){
            OnPressedButton(ID);
        }
    }
}
