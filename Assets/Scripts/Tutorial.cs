﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public enum TutorialSteps
{
	WELCOME,
	START_FIRST_MISSION,
	TAP_START_ON_MISSIONINFO,
	LIGHTING_INFO,
	MARKER_ADVICE,
	SELECT_GATE,
	LOGIC_GATE_SELECTED,
	LOGIC_GATE_SELECTED_INFO,
	TRUTHTABLE_SHOWN,
	RESULT_IS_VALID
}

public class Tutorial : MonoBehaviour
{
	public static Tutorial Instance { get; private set; }

	[SerializeField] public List<OnNextStepActions> onNextStepActions;

	TutorialInfo tutorialInfo;
    TutorialSteps currentStep;
	int tutorialStepCount;

	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
            Instance = this;
	}

    void Start()
    {
		tutorialInfo = (TutorialInfo) UIManager.Instance.GetPopup(PopupType.TutorialInfo).script;
		currentStep = (TutorialSteps)(PlayerPrefs.HasKey("TutorialStep") ? PlayerPrefs.GetInt("TutorialStep") : 0);
		tutorialStepCount = Enum.GetValues(typeof(GateType)).Length;

		if (currentStep > 0)
            UIManager.Instance.Open(PopupType.TutorialReset);
        else
			tutorialInfo.SetupTutorialInfo(currentStep);
	}

    public void nextStep(TutorialSteps currStep)
    {
		if (currStep != currentStep)
			return;

		onNextStepActions.FirstOrDefault(x => x.step == currentStep)?.onNextStep?.Invoke();

        currentStep++;

        if ((int)currentStep < tutorialStepCount - 1)
			tutorialInfo.SetupTutorialInfo(currentStep);
    }

	public void nextStep(string currStep) => nextStep(Enum.GetValues(typeof(TutorialSteps)).Cast<TutorialSteps>().ToList().First(x => x.ToString() == currStep));


	public void ResetTutorial()
    {
        currentStep = 0;
        PlayerPrefs.SetInt("TutorialStep", (int)currentStep);
		tutorialInfo.SetupTutorialInfo(currentStep);
    }
}

[Serializable]
public class OnNextStepActions
{
	public TutorialSteps step;
	public UnityEvent onNextStep;
}
