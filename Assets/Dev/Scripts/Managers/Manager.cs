namespace Dev.Scripts.Managers
{
    /// <summary>
    /// Represents a manager class that provides access to different managers in the application.
    /// </summary>
    public static class Manager
    {
        /// <summary>
        /// Gets the event manager instance.
        /// </summary>
        public static EventManager Events;

        /// <summary>
        /// Gets the canvas manager instance.
        /// </summary>
        public static CanvasManager Canvas;

        /// <summary>
        /// Gets the scriptable manager instance.
        /// </summary>
        public static ScriptableManager Scriptable;
    }
}