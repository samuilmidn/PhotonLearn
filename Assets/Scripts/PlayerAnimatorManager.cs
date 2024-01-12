using Photon.Pun;
using UnityEngine;

namespace PhotonLearn
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Serializable Fields

        [SerializeField] private float directionDampTime = 0.25f;

        #endregion

        #region Private Fields

        private Animator animator;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        private void Update()
        {
            // Prevent control is connected to Photon and represent the localPlayer
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            
            // failSafe is missing Animator component on GameObject
            if (!animator) return;

            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            if (v < 0f) v = 0f;

            animator.SetFloat("Speed", h*h + v*v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }

        #endregion
    }
}