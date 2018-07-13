using System;
using UnityEngine;
using UnityEngine.UI;

    public class UI_Keyboard : MonoBehaviour
    {
        private InputField input;
        public bool m_isEntered;
        public String m_name;

        public void ClickKey(string character)
        {
            input.text += character;
        }

        public void Backspace()
        {
            if (input.text.Length > 0)
            {
                input.text = input.text.Substring(0, input.text.Length - 1);

            }
        }

        public void Enter()
        {
            m_isEntered = true;
            m_name = input.text;

        }

        private void Start()
        {
            input = GetComponentInChildren<InputField>();
            m_isEntered = false;
        }
    }
