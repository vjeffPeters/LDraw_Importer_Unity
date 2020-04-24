using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LDraw
{
    public class SubModel : MonoBehaviour
    {
        public int currPartIdx = 0;
        public bool isRoot;

        public Vector3 startPosition;
        public Vector3 finalPosition;

        void Awake()
        {
            if (!isRoot)
            {
                int stepIdx = transform.GetSiblingIndex() + 1;
                Vector3 animationDirection = transform.parent.GetChild(stepIdx).GetComponent<Step>().animationDirection;
                finalPosition = transform.localPosition;
                startPosition = finalPosition + (animationDirection * Step.ANIMATION_DISTANCE);
            }
        }

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

        public bool NextStep(bool animate)
        {
            while (currPartIdx < transform.childCount)
            {
                Transform currPart = transform.GetChild(currPartIdx);

                Step nestedStep = currPart.GetComponent<Step>();
                if (nestedStep != null)
                {
                    if (animate) nestedStep.PlayAnimations();
                    currPartIdx++;
                    if (currPartIdx == transform.childCount) break;
                    return true;
                }

                SubModel nestedSubModel = currPart.GetComponent<SubModel>();
                if (nestedSubModel != null)
                {
                    nestedSubModel.transform.localPosition = nestedSubModel.startPosition;
                    if (!nestedSubModel.NextStep(animate))
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
                    nestedSubModel.transform.localPosition = nestedSubModel.startPosition;
                    if (!nestedSubModel.PreviousStep())
                    {
                        currPartIdx -= 2;// Last step before the submodel
                    }
                    if (currPartIdx < 0) break;
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
