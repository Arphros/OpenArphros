using UnityEngine;

namespace Arphros.Gameplay
{
    public class Gamemode : MonoBehaviour
    {
        public PlayerMovement Player { get; internal set; }

        /// <summary>
        /// Indicates when the player has a keypress registered!
        /// </summary>
        /// <returns>Returns true if handled</returns>
        public virtual bool OnKeyPressed()
        {
            return false;
        }

        /// <summary>
        /// Called when the gamemode is initialized
        /// </summary>
        public virtual void OnInitialization()
        {

        }

        /// <summary>
        /// Called by the main player movement when it needs to be updated
        /// </summary>
        public virtual void OnUpdate()
        {

        }

        /// <summary>
        /// Called when the gamemode changes
        /// </summary>
        /// <param name="to">The gamemode it changed to</param>
        public virtual void OnGamemodeChanged(Gamemode to)
        {

        }
    }
}