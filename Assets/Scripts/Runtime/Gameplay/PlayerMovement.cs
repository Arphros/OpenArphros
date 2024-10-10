using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Arphros.Gameplay
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")]
        public MeshFilter meshFilter;
        public new MeshRenderer renderer;
        public new BoxCollider collider;
        public AudioSource audioSource;
        public new Rigidbody rigidbody;

        public GameObject CurrentGround { get; private set; }

        [Header("States")]
        public bool isStarted;
        public bool isControllable = true;
        public bool isAlive = true;
        public bool isFinished;

        [Header("Properties")]
        public float speed = 12;
        public LayerMask groundLayers;

        [Header("Events")]
        public UnityEvent onStarted;
        public UnityEvent onTap;
        public UnityEvent onTurn;
        public UnityEvent onKilled;
        public UnityEvent onFinished;

        [Header("Gamemode")]
        [SerializeField] private int currentGamemodeIndex = 0;
        [SerializeField] private List<Gamemode> gamemodes = new List<Gamemode>();

        public Gamemode CurrentGamemode => gamemodes.Count > currentGamemodeIndex ? gamemodes[currentGamemodeIndex] : null;
        private int inputQueue;

        private void Start()
        {
            CurrentGamemode.Player = this;
            foreach (var gamemode in gamemodes)
                gamemode.OnInitialization();
        }

        private void Update()
        {
            if (inputQueue > 0)
            {
                if (CurrentGamemode)
                    CurrentGamemode.OnKeyPressed();
                inputQueue--;
            }

            CurrentGamemode.OnUpdate();
        }

        /// <summary>
        /// Changes the gamemode according to the passed mode, if it's not found in the player instance, it ignored the changes
        /// </summary>
        public void ChangeGamemode(Gamemode mode)
        {
            int index = gamemodes.IndexOf(mode);
            if (index > -1)
                ChangeGamemode(index);
        }

        /// <summary>
        /// Changes the gamemode based on it's index in the player instance
        /// </summary>
        public void ChangeGamemode(int index)
        {
            var newGamemode = gamemodes[index];
            if (CurrentGamemode == newGamemode) return;

            CurrentGamemode?.OnGamemodeChanged(newGamemode);
            currentGamemodeIndex = index;
        }

        /// <summary>
        /// Simulates an input to the player, despite the player not being controllable
        /// </summary>
        public void SimulateInput() =>
            inputQueue++;

        /// <summary>
        /// Checks if the player is on the ground or not
        /// </summary>
        public bool IsGrounded()
        {
            transform.position = transform.position;
            if (!Physics.Raycast(transform.position, -transform.up, out var hitInfo,
                    (collider.size.y / 2) + 0.03f, groundLayers)) return false;
            CurrentGround = hitInfo.collider.gameObject;
            return true;
        }

        /// <summary>
        /// Checks if mouse is over UI
        /// </summary>
        /// <param name="keyInt">The key code</param>
        /// <returns>True if mouse is over UI</returns>
        private static bool IsMouseAndTouchOverUI(int keyInt)
        {
            if (!IsKeyCodeMouse(keyInt)) return false;

            var touches = Input.touches;
            return touches.Any(touch => EventSystem.current.IsPointerOverGameObject(touch.fingerId)) || EventSystem.current.IsPointerOverGameObject();
        }

        /// <summary>
        /// Checks if mouse is over UI
        /// </summary>
        /// <param name="keyInt">The key code</param>
        /// <returns>True if mouse is over UI</returns>
        private static bool IsKeyCodeMouse(int keyInt) => keyInt is <= 330 and >= 322;

        /// <summary>
        /// Checks if mouse is over UI
        /// </summary>
        /// <param name="keyInt">The key code</param>
        /// <returns>True if mouse is over UI</returns>
        private static bool IsKeyCodeMouse(KeyCode key) => ((int)key) is <= 330 and >= 322;
    }
}