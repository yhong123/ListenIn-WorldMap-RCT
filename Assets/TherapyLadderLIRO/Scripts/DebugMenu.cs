﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if DEBUG_LIRO

public class ComboBox
{
    private static bool forceToUnShow = false;
    private static int useControlID = -1;
    private bool isClickedComboButton = false;
    private int selectedItemIndex = 0;

    private Rect rect;
    private GUIContent buttonContent;
    private GUIContent[] listContent;
    private string buttonStyle;
    private string boxStyle;
    private GUIStyle listStyle;

    public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, GUIStyle listStyle)
    {
        this.rect = rect;
        this.buttonContent = buttonContent;
        this.listContent = listContent;
        this.buttonStyle = "button";
        this.boxStyle = "box";
        this.listStyle = listStyle;
    }

    public ComboBox(Rect rect, GUIContent buttonContent, GUIContent[] listContent, string buttonStyle, string boxStyle, GUIStyle listStyle)
    {
        this.rect = rect;
        this.buttonContent = buttonContent;
        this.listContent = listContent;
        this.buttonStyle = buttonStyle;
        this.boxStyle = boxStyle;
        this.listStyle = listStyle;
    }

    public int Show()
    {
        if (forceToUnShow)
        {
            forceToUnShow = false;
            isClickedComboButton = false;
        }

        bool done = false;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.MouseUp:
                {
                    if (isClickedComboButton)
                    {
                        done = true;
                    }
                }
                break;
        }

        if (GUI.Button(rect, buttonContent, buttonStyle))
        {
            if (useControlID == -1)
            {
                useControlID = controlID;
                isClickedComboButton = false;
            }

            if (useControlID != controlID)
            {
                forceToUnShow = true;
                useControlID = controlID;
            }
            isClickedComboButton = true;
        }

        if (isClickedComboButton)
        {
            Rect listRect = new Rect(rect.x, rect.y + listStyle.CalcHeight(listContent[0], 1.0f),
                      rect.width, listStyle.CalcHeight(listContent[0], 1.0f) * listContent.Length);

            GUI.Box(listRect, "", boxStyle);
            int newSelectedItemIndex = GUI.SelectionGrid(listRect, selectedItemIndex, listContent, 1, listStyle);
            if (newSelectedItemIndex != selectedItemIndex)
            {
                selectedItemIndex = newSelectedItemIndex;
                buttonContent = listContent[selectedItemIndex];
            }
        }

        if (done)
            isClickedComboButton = false;

        return selectedItemIndex;
    }

    public int SelectedItemIndex
    {
        get
        {
            return selectedItemIndex;
        }
        set
        {
            selectedItemIndex = value;
        }
    }
}

public class DebugMenu : MonoBehaviour {

    GUIContent[] comboBoxList;
    private ComboBox comboBoxControl;// = new ComboBox();
    private GUIStyle listStyle = new GUIStyle();
    string[] enumvalues;

    public float offsetFromRightSide = 0.0f;

    private void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || DEBUG_LIRO
        enumvalues = Enum.GetNames(typeof(TherapyLadderStep));
        comboBoxList = new GUIContent[enumvalues.Length];

        for (int i = 0; i < enumvalues.Length; i++)
        {
            comboBoxList[i] = new GUIContent(enumvalues[i]);
        }

        listStyle.normal.textColor = Color.white;
        listStyle.onHover.textColor = Color.yellow;
        listStyle.hover.background = new Texture2D(2, 2);
        listStyle.padding.left = 0;
        listStyle.padding.right = 0;
        listStyle.padding.top = 0; 
        listStyle.padding.bottom = 4;

        comboBoxControl = new ComboBox(new Rect(Screen.width - 240 - offsetFromRightSide, 20, 100, 30), comboBoxList[0], comboBoxList, "button", "box", listStyle);
#endif
    }

    private void OnGUI()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || DEBUG_LIRO
        GUI.Box(new Rect(Screen.width - 250 - offsetFromRightSide, 10, 240, 50), "");
        comboBoxControl.Show();
        if (GUI.Button(new Rect(Screen.width - 130 - offsetFromRightSide, 20, 110, 30), "Change"))
        {
            TherapyLIROManager.Instance.ChangeLIROSectionButton(enumvalues[comboBoxControl.SelectedItemIndex]);
        }
#endif
    }
}
#endif
