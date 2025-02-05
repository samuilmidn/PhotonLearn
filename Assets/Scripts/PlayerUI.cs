using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonLearn
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [Tooltip("Pixel offset from the player target")]
        [SerializeField] private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField] private TMP_Text playerNameText;

        [Tooltip("UI Slider to display Player's Health")]
        [SerializeField] private Slider playerHealthSlider;

        private PlayerManager target;

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Renderer targetRenderer;
        CanvasGroup _canvasGroup;
        Vector3 targetPosition;

        #endregion

        #region MonoBehaviour Callbacks

        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            transform.SetParent(GameObject.Find("Canvas level ui").GetComponent<Transform>(), false);
        }

        void Update()
        {
            // Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            // Reflect the Player Health
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
            }
        }

        void LateUpdate()
        {
            // Do not show the UI if we are not visible to the camera, thus avoid potential bugs with seeing the UI, but not the player itself.
            if (targetRenderer != null)
            {
                _canvasGroup.alpha = targetRenderer.isVisible ? 1f : 0f;
            }

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }
        }

        #endregion

        #region Public Methods

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;

            targetTransform = target.GetComponent<Transform>();
            targetRenderer = target.GetComponent<Renderer>();

            CharacterController characterController = _target.GetComponent<CharacterController>();

            // Get data from the Player that won't change during the lifetime of this Component
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }

            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }

        #endregion
    }
}