using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace TeslasuitAPI.Tutorials
{
    public class PopupMessage : MonoBehaviour
    {

        public Button prevButton;
        public Button nextButton;
        public Button closeButton;

        public event Action Next = delegate { };
        public event Action Prev = delegate { };

        // Use this for initialization
        void Start()
        {
            nextButton.onClick.AddListener(new UnityEngine.Events.UnityAction(Next));
            prevButton.onClick.AddListener(new UnityEngine.Events.UnityAction(Prev));
            closeButton.onClick.AddListener(new UnityEngine.Events.UnityAction(Close));
        }

        // Update is called once per frame
        void Update()
        {

        }



        public void Close()
        {
            Destroy(gameObject);
        }
    }


}
