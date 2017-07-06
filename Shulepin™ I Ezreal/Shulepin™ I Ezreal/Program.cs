namespace Ezreal    
{
    #region Imports

    using System;

    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;

    #endregion

    /// <summary>
    ///     The Program
    /// </summary>
    class Program
    {
        /// <summary>
        ///     The Entry Point
        /// </summary>
        /// <param name="args">The Args</param>
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += (EventArgs) =>
            {
                Core.DelayAction(() 
                    => new Components.Core.Bootstrap()
                    , new Random().Next(1500, 3000));
            };
        }
    }
}
