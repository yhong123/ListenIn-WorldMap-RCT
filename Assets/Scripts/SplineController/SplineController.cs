using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eOrientationMode { NODE = 0, TANGENT }

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
public class SplineController : MonoBehaviour
{
	public GameObject SplineRoot;
	public float Duration = 10;
	public eOrientationMode OrientationMode = eOrientationMode.NODE;
	public eWrapMode WrapMode = eWrapMode.ONCE;
	public bool AutoStart = false;
	public bool AutoClose = false;
	public bool HideOnExecute = false;
	public bool UseRigidBody = false;
	public bool UseScaling = false;


	SplineInterpolator mSplineInterp;
	Transform[] mTransforms;

	void OnDrawGizmos()
	{
		Transform[] trans = GetTransforms();
		if (trans.Length < 2)
			return;

		SplineInterpolator interp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
		SetupSplineInterpolator(interp, trans);
		interp.StartInterpolation(null, true, WrapMode, false, false);


		Vector3 prevPos = trans[0].position;
		for (int c = 1; c <= 200; c++)
		{
			float currTime = c * Duration / 100;
			Vector3 currPos = interp.GetHermiteAtTime(currTime);
			float mag = (currPos-prevPos).magnitude * 2;
			Gizmos.color = new Color(0, mag, 0, 1);
			Gizmos.DrawLine(prevPos, currPos);
			prevPos = currPos;
		}
	}

	public void ExecuteMotion()
	{
		Start();
	}

	void Start()
	{
		mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

		mTransforms = GetTransforms();

		if (HideOnExecute)
			DisableTransforms();

		if (AutoStart)
			FollowSpline();
	}

	void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
	{
		interp.Reset();

		float step = (AutoClose) ? Duration / trans.Length :
			Duration / (trans.Length - 1);

		int c;
		for (c = 0; c < trans.Length; c++)
		{
			if (OrientationMode == eOrientationMode.NODE)
			{
				interp.AddPoint(trans[c].position, trans[c].rotation, trans[c].localScale ,step * c, new Vector2(0, 1));
			}
			else if (OrientationMode == eOrientationMode.TANGENT)
			{
				Quaternion rot;
				if (c != trans.Length - 1)
					rot = Quaternion.LookRotation(trans[c + 1].position - trans[c].position, trans[c].up);
				else if (AutoClose)
					rot = Quaternion.LookRotation(trans[0].position - trans[c].position, trans[c].up);
				else
					rot = trans[c].rotation;

				interp.AddPoint(trans[c].position, rot, trans[c].localScale, step * c, new Vector2(0, 1));
			}
		}

		if (AutoClose)
			interp.SetAutoCloseMode(step * c);
	}


	/// <summary>
	/// Returns children transforms, sorted by name.
	/// </summary>
	Transform[] GetTransforms()
	{
		if (SplineRoot == null) return new Transform[] { };
		List<Component> components = new List<Component>(SplineRoot.GetComponentsInChildren(typeof(Transform)));
		List<Transform> transforms = components.ConvertAll(c => (Transform)c);

		transforms.Remove(SplineRoot.transform);
		transforms.Sort(delegate(Transform a, Transform b)
		{
			return a.name.CompareTo(b.name);
		});

		return transforms.ToArray();

	}

	/// <summary>
	/// Disables the spline objects, we don't need them outside design-time.
	/// </summary>
	void DisableTransforms()
	{
		if (SplineRoot != null)
		{
			SplineRoot.SetActive(false);
		}
	}


	/// <summary>
	/// Starts the interpolation
	/// </summary>
	public void FollowSpline()
	{
		if (mTransforms.Length > 0)
		{
			SetupSplineInterpolator(mSplineInterp, mTransforms);
			mSplineInterp.StartInterpolation(null, true, WrapMode, UseRigidBody, UseScaling);
		}
	}

	/// <summary>
	/// Adds the current transform to the spline controller root.
	/// </summary>
	public void AddSplineToRootObject()
	{
		if(SplineRoot == null)
		{
			Debug.Log("Spline root is missing");
			return;
		}

		int currChild = SplineRoot.transform.childCount;

		GameObject appendPos = new GameObject();
		int toChar = 65 + currChild;
		char c = (char)toChar;
		appendPos.name =  new string(c, 1);
		appendPos.transform.SetParent(SplineRoot.transform, false);
		appendPos.transform.position =  this.transform.position;
		appendPos.transform.rotation = this.transform.rotation;
		appendPos.transform.localScale = this.transform.localScale;

	}
}