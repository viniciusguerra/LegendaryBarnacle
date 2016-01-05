using UnityEngine;
using System.Collections.Generic;

namespace Test
{
    public class XboxOneInputTest : MonoBehaviour
    {
        string[] buttons = {
                        "p1_a","p1_b","p1_x","p1_y","p1_view","p1_menu","p1_lb","p1_rb","p1_ls","p1_rs"
                         };
        string[] axes = {
                     "p1_lx","p1_ly","p1_lt","p1_rt","p1_rx","p1_ry","p1_dpadx","p1_dpady"
                      };

        Dictionary<string, float> axesValues;

        public bool logButtons = true;
        public bool logAxisDelta;
        public bool logAxisFull;

        void Update()
        {
            if (logButtons)
            {
                foreach (string button in buttons)
                {
                    if (Input.GetButtonDown(button))
                        Debug.Log("Xbox One Input: " + button + " down");
                }
            }

            if (logAxisDelta)
            {
                foreach (string axis in axes)
                {
                    if (axesValues[axis] != Input.GetAxis(axis))
                        Debug.Log("Xbox One Input: " + axis + " axis changed");

                    axesValues[axis] = Input.GetAxis(axis);
                }
            }

            if (logAxisFull)
            {
                foreach (string axis in axes)
                {
                    Debug.Log("Xbox One Input: " + axis + " value = " + Input.GetAxis(axis));
                }
            }
        }

        void Start()
        {
            axesValues = new Dictionary<string, float>();

            foreach (string axis in axes)
            {
                axesValues.Add(axis, Input.GetAxis(axis));
            }
        }
    }
}