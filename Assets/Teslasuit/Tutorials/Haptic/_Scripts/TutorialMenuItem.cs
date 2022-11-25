using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TeslasuitAPI.Tutorials
{
    public class TutorialMenuItem : MonoBehaviour, IPointerClickHandler
    {
        public Text title;
        public Image bg;

        public event Action Clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("clicked");
            if (Clicked != null)
                Clicked();
        }

        public void SetTitle(string titleString)
        {
            title.text = titleString;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}