using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class JigsawAnimator : MonoBehaviour {

	public GameObject animatorMoves;
	private float singleMovementSpeed;
	private float singleMovementIncrease;

	public float speed;
	public float speedIncrease;

	public bool animate = false;

	[SerializeField]
	public List<Vector3> positions;

	void Awake()
	{

	}

	public void StartAnimation()
	{
		animate = true;
	}

	// Use this for initialization
	void Start () {

		singleMovementSpeed = speed;
		singleMovementIncrease = speedIncrease;
		List<RectTransform> transforms = new List<RectTransform>();
		positions = new List<Vector3>();
		List<Component> components = new List<Component>(animatorMoves.GetComponentsInChildren(typeof(RectTransform)));
		transforms = components.ConvertAll(c => (RectTransform)c);
		transforms.Remove(animatorMoves.GetComponent<RectTransform>());
		
		positions = transforms.Select(obj => obj.localPosition).ToList();
		positions.Add(gameObject.GetComponent<RectTransform>().localPosition);

	}

	IEnumerator Animate(int startIdx, int endIdx){

		float elapsedTime = 0.0f;
		Vector3 startpos = positions[startIdx];
		Vector3 finalpos = positions[endIdx];

		while(elapsedTime < 1.0f + singleMovementSpeed)
		{
			transform.localPosition = Vector3.Slerp(startpos, finalpos, elapsedTime);
			elapsedTime += singleMovementSpeed;
			yield return null;
		}

		if(startIdx == 0)
		{
			startIdx = endIdx;
			endIdx = positions.Count - 1;
			singleMovementSpeed += singleMovementIncrease;
			StartCoroutine(Animate(startIdx,endIdx));
		}

	}

	// Update is called once per frame
	void Update () {
		if(animate)
		{
			animate = false;
			StartCoroutine(Animate(0,Random.Range(1,positions.Count-2)));
			//StartCoroutine(Animate(0,positions.Count-1));
		}
	}
}
