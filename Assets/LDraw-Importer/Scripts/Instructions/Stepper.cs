using UnityEngine;

namespace LDraw
{
    [RequireComponent(typeof(SubModel))]
    public class Stepper : MonoBehaviour
    {
        public bool ready = false;
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
                rootModel.NextStep();
            }

            if (ready && Input.GetKeyDown(KeyCode.R))
            {
                rootModel.PreviousStep();
            }
        }
    }

}
