using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI; // UI hiển thị thông tin
    public TMP_Text interaction_text;     // TextMeshPro component
    public bool onTarget;


    public GameObject selectObject;
    public static SelectionManager Instance { get; set; }
        

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TMP_Text>();

        if (interaction_text == null)
        {
            Debug.LogError("TMP_Text component not found on interaction_Info_UI!");
        }
    }



    private void Awake()
    {
        if(Instance!=null && Instance != this)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Vẽ tia Ray trong Scene View (chỉ hiển thị trong Scene)
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            if (selectionTransform.GetComponent<InteractableObject>() && selectionTransform.GetComponent<InteractableObject>().playerInRange)
            {
                selectObject = selectionTransform.GetComponent<InteractableObject>().gameObject;
                interaction_text.text = selectionTransform.GetComponent<InteractableObject>().GetItemName();
                interaction_Info_UI.SetActive(true);

                
                onTarget = true;
            }
            else
            {
                interaction_Info_UI.SetActive(false);
                onTarget= false;
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
        }
    }
}
