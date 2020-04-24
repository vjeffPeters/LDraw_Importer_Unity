using UnityEngine;

namespace LDraw
{
    [RequireComponent(typeof(SubModel))]
    public class Stepper : MonoBehaviour
    {
        public bool ready = false;
        public bool forward = true;
        private SubModel rootModel;

        void Start()
        {
            rootModel = GetComponent<SubModel>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                rootModel.ClearSteps();
                ready = true;
            }

            if (ready && Input.GetKeyDown(KeyCode.F))
            {
                if (!forward) rootModel.NextStep(false);
                forward = true;
                rootModel.NextStep(true);
            }

            if (ready && Input.GetKeyDown(KeyCode.R))
            {
                if (forward) rootModel.PreviousStep();
                forward = false;
                rootModel.PreviousStep();
            }
        }
    }

}
