using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDraw
{
    public class SubModel : MonoBehaviour
    {
        public int currPartIdx = 0;

        public void ClearSteps()
        {
            currPartIdx = 0;

            while (currPartIdx < transform.childCount)
            {
                Transform currPart = transform.GetChild(currPartIdx++);

                Step nestedStep = currPart.GetComponent<Step>();
                if (nestedStep != null)
                {
                    continue;
                }

                SubModel nestedSubModel = currPart.GetComponent<SubModel>();
                if (nestedSubModel != null)
                {
                    nestedSubModel.ClearSteps();
                    continue;
                }

                currPart.gameObject.SetActive(false);
            }

            currPartIdx = 0;
        }

        public bool NextStep()
        {
            while (currPartIdx < transform.childCount)
            {
                Transform currPart = transform.GetChild(currPartIdx);

                Step nestedStep = currPart.GetComponent<Step>();
                if (nestedStep != null)
                {
                    currPartIdx++;
                    if (currPartIdx == transform.childCount) break;
                    return true;
                }

                SubModel nestedSubModel = currPart.GetComponent<SubModel>();
                if (nestedSubModel != null)
                {
                    if (!nestedSubModel.NextStep())
                    {
                        currPartIdx++;
                    }
                    if (currPartIdx == transform.childCount) break;
                    return true;
                }

                currPartIdx++;
                currPart.gameObject.SetActive(true);
            }

            currPartIdx = transform.childCount - 1;
            return false;
        }

        public bool PreviousStep()
        {
            while (currPartIdx >= 0)
            {
                Transform currPart = transform.GetChild(currPartIdx);

                Step nestedStep = currPart.GetComponent<Step>();
                if (nestedStep != null)
                {
                    currPartIdx--;
                    if (currPartIdx == -1) break;
                    return true;
                }

                SubModel nestedSubModel = currPart.GetComponent<SubModel>();
                if (nestedSubModel != null)
                {
                    if (!nestedSubModel.PreviousStep())
                    {
                        currPartIdx--;
                    }
                    if (currPartIdx == -1) break;
                    return true;
                }

                currPartIdx--;
                currPart.gameObject.SetActive(false);
            }

            currPartIdx = 0;
            return false;
        }
    }
}
