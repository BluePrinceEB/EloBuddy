namespace Ezreal.Components.Core
{
    #region Imports

    using EloBuddy;

    using Components.Utility;

    using Enumerations;
    using Utils;

    #endregion

    /// <summary>
    ///     The Bootstrap
    /// </summary>
    internal class Bootstrap
    {
        #region Constructors and Destructors

        /// <summary>
        ///    Bootstrap Components
        /// </summary>
        public Bootstrap()
        {
            if (Extensions.Player.Hero.Equals(Champion.Ezreal))
            {
                new Spells();
                new Config();
                new Events();

                Logging.AddEntry(LoggingEntryType.Info, "All components are loaded");
                Chat.Print("<font color='#FFFFF0'>[Shulepin Ezreal]:</font><font color='#00FF7F'> Loaded! </font>");
            }
        }

        #endregion
    }
}
