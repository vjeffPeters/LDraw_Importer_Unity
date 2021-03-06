﻿using System.Collections;
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

        public bool NextStep()
        {
            Transform currPart = transform.GetChild(currPartIdx);
            if (currPart.GetComponent<Step>() != null) currPartIdx++;

            for (; currPartIdx < transform.childCount; currPartIdx++)
            {
                currPart = transform.GetChild(currPartIdx);

                Step nextStep = currPart.GetComponent<Step>();
                if (nextStep != null)
                {
                    nextStep.PlayAnimations();
                    return true;
                }

                SubModel nestedSubModel = currPart.GetComponent<SubModel>();
                if (nestedSubModel != null)
                {
                    nestedSubModel.transform.localPosition = nestedSubModel.startPosition;
                    if (!nestedSubModel.NextStep())
                    {
                        continue;
                    }
                    return true;
                }

                currPart.gameObject.SetActive(true);
            }

            currPartIdx = transform.childCount - 1;
            return false;
        }

        public bool PreviousStep()
        {
            Transform currPart = transform.GetChild(currPartIdx);
            if (currPart.GetComponent<Step>() != null)
            {
                currPartIdx--;
                // Edge case: When we step back into a sub model first move it
                // into start position without executing any steps
                currPart = transform.GetChild(currPartIdx);
                SubModel currentSubModel = currPart.GetComponent<SubModel>();
                if (currentSubModel != null)
                {
                    currentSubModel.transform.localPosition = currentSubModel.startPosition;
                    return true;
                }
            }

            for (; currPartIdx >= 0; currPartIdx--)
            {
                currPart = transform.GetChild(currPartIdx);

                Step previousStep = currPart.GetComponent<Step>();
                if (previousStep != null)
                {
                    return true;
                }

                SubModel nestedSubModel = currPart.GetComponent<SubModel>();
                if (nestedSubModel != null)
                {
                    nestedSubModel.transform.localPosition = nestedSubModel.startPosition;
                    if (!nestedSubModel.PreviousStep())
                    {
                        continue;
                    }
                    return true;
                }

                currPart.gameObject.SetActive(false);
            }

            currPartIdx = 0;
            return false;
        }
    }
}
