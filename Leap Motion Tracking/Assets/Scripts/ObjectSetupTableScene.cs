﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleObjectList
{
    public GameObject gameObject;
    public Transform SpawnPosition;
}

[System.Serializable]
public class ObjectControl
{
    public GameObject gameObject;
    public GameObject onButton;
    public GameObject offButton;
}

public class ObjectSetupTableScene : MonoBehaviour {

    public ObjectControl patient;
    public ObjectControl heart;
    public ObjectControl MRI;
	public ObjectControl dicom;
    public GameObject dicomSlider1;
    public GameObject dicomSlider2;
    public List<SimpleObjectList> simpleObjects;

    private List<GameObject> simpleObjectsOnScene = new List<GameObject>();
    
    private List<Transform> patientParts = new List<Transform>();
    private List<Transform> MRIParts = new List<Transform>();
	private List<Transform> bonesParts = new List<Transform>();
	

    void Start()
    {
        var patientPartsTmp = new List<Transform>(patient.gameObject.GetComponentsInChildren<Transform>());
        var MRIPartsTmp = new List<Transform>(MRI.gameObject.GetComponentsInChildren<Transform>());
		var bonesPartsTmp = new List<Transform>(dicom.gameObject.GetComponentsInChildren<Transform>());
        foreach(var part in patientPartsTmp)
        {
            if (part.parent != patient.gameObject.transform.parent)
                patientParts.Add(part);
        }
        foreach (var part in MRIPartsTmp)
        {
            if (part.parent != MRI.gameObject.transform.parent)
                MRIParts.Add(part);
        }
		foreach (var part in bonesPartsTmp)
        {
            if (part.parent != dicom.gameObject.transform.parent)
                bonesParts.Add(part);
        }

        // Instantly disappear at start
        patient.gameObject.SetActive(false);
        MRI.gameObject.SetActive(false);

        SwitchMRI(false);
        SwitchPatient(false);
        SwitchHeart(false);
		SwitchDicom(false);
        SwitchSimpleObjects(false);
    }


    private IEnumerator SwapButtons(ObjectControl buttons, bool state)
    {
        yield return new WaitForEndOfFrame();
        buttons.onButton.SetActive(!state);
        buttons.offButton.SetActive(state);
    }

	public void ButtonDicom(bool state){
		SwitchMRI(false);
        SwitchPatient(false);
        SwitchHeart(false);
		
		SwitchDicom(state);
	}
	public void ButtonPatient(bool state){
		SwitchMRI(false);
        SwitchDicom(false);
        SwitchHeart(false);
		
		SwitchPatient(state);
	}
	public void ButtonHeart(bool state){
		SwitchMRI(false);
        SwitchPatient(false);
		SwitchDicom(false);
		
		SwitchHeart(state);
	}
	public void ButtonMRI(bool state){
        SwitchPatient(false);
        SwitchHeart(false);
		SwitchDicom(false);
		
		SwitchMRI(state);
	}
	public void ButtonSimpleObjects(bool state){
		SwitchSimpleObjects(state);
	}	
	
	


    public void SwitchDicom(bool state)
    {
        dicom.gameObject.SetActive(state);
		dicomSlider1.gameObject.SetActive(state);
        dicomSlider2.gameObject.SetActive(state);
        StartCoroutine(SwitchBonesCoroutine(state));
        StartCoroutine(SwapButtons(dicom,state));
    }
    private IEnumerator SwitchBonesCoroutine(bool state)
    {
        if (state == true)
            dicom.gameObject.SetActive(state);
        foreach (var part in bonesParts)
        {
            part.gameObject.SetActive(state);
            yield return null;
        }
        if (state == false)
            dicom.gameObject.SetActive(state);
    }


    public void SwitchHeart(bool state)
    {
        heart.gameObject.SetActive(state);
        StartCoroutine(SwapButtons(heart, state));
    }

    public void SwitchPatient(bool state)
    {
        StartCoroutine(SwitchPatientCoroutine(state));
    }

    private IEnumerator SwitchPatientCoroutine(bool state)
    {
        if (state == true)
            patient.gameObject.SetActive(state);
        foreach (var part in patientParts)
        {
            part.gameObject.SetActive(state);
            yield return null;
        }
        if (state == false)
            patient.gameObject.SetActive(state);

        StartCoroutine(SwapButtons(patient, state));
    }

    public void SwitchMRI(bool state)
    {
        StartCoroutine(SwitchMRICoroutine(state));
    }

    private IEnumerator SwitchMRICoroutine(bool state)
    {
        if(state == true)
            MRI.gameObject.SetActive(state);
        foreach (var part in MRIParts)
        {
            part.gameObject.SetActive(state);
            yield return null;
        }
        if (state == false)
            MRI.gameObject.SetActive(state);

        StartCoroutine(SwapButtons(MRI, state));
    }

    public void SwitchSimpleObjects(bool state)
    {
        StartCoroutine(SwitchSimpleObjectsCoroutine(state));
    }

    private IEnumerator SwitchSimpleObjectsCoroutine(bool state)
    {
        if (state == true)
            foreach (var obj in simpleObjects)
            {
                GameObject newObj = Instantiate(obj.gameObject, obj.SpawnPosition.parent);
                newObj.transform.position = obj.SpawnPosition.transform.position;
                if (Configuration.IsGravityOn)
                {
                    newObj.GetComponent<Rigidbody>().isKinematic = false;
                }
                simpleObjectsOnScene.Add(newObj);
                yield return null;
            }
        else
        {
            foreach (var obj in simpleObjectsOnScene)
            {
                Destroy(obj);
                yield return null;
            }
            simpleObjectsOnScene = new List<GameObject>();
        }
            
    }
	
	
	
	
	public void RotateDicom(float value){
        
        var rot = dicom.gameObject.transform.eulerAngles;
        dicom.gameObject.transform.eulerAngles = new Vector3(rot.x, value, rot.z);
    }

}