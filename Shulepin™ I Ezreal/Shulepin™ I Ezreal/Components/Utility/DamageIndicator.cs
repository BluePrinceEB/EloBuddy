namespace Ezreal.Components.Utility
{
    #region Imports

    using System;

    using SharpDX;
    using SharpDX.Direct3D9;

    using EloBuddy;

    using Color = System.Drawing.Color;

    #endregion

    /// <summary>
    ///     The Damage Indicator
    /// </summary>
    internal class DamageIndicator
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Damage Indicator Constructor
        /// </summary>
        public DamageIndicator()
        {
            DxLine = new Line(DxDevice) { Width = 9 };

            Drawing.OnPreReset += OnPreReset;
            Drawing.OnPostReset += OnPostReset;
            AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
            AppDomain.CurrentDomain.ProcessExit += OnDomainUnload;
        }

        #endregion

        #region Public Static Fields

        /// <summary>
        ///     The Line
        /// </summary>
        public static Line DxLine { get; set; }

        /// <summary>
        ///     The Device
        /// </summary>
        public static Device DxDevice = Drawing.Direct3DDevice;

        #endregion

        #region Public Properties

        /// <summary>
        ///     The Start Position
        /// </summary>
        public Vector2 StartPosition => new Vector2(Unit.HPBarPosition.X + Offset.X, Unit.HPBarPosition.Y + Offset.Y);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The Higth
        /// </summary>
        public float Hight = 9;

        /// <summary>
        ///     The Width
        /// </summary>
        public float Width = 104;

        /// <summary>
        ///     The Unit
        /// </summary>
        public AIHeroClient Unit { get; set; }

        /// <summary>
        ///     The Offset
        /// </summary>
        public Vector2 Offset
        {
            get
            {
                if (Unit != null)
                {
                    return Unit.IsAlly ? new Vector2(0, 4) : new Vector2(4f, 4.5f);
                }
                return new Vector2();
            }
        }

        /// <summary>
        ///     Draw Damage
        /// </summary>
        /// <param name="dmg">The Damage</param>
        /// <param name="color">The Color</param>
        public void DrawDamage(float dmg, ColorBGRA color)
        {
            var hpPosNow = GetHpPosAfterDmg(0);
            var hpPosAfter = GetHpPosAfterDmg(dmg);

            FillHpBar(hpPosNow, hpPosAfter, color);
        }

        #endregion

        #region Private Methods and Operators

        /// <summary>
        ///     On Domain Unload
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="eventArgs">The Args</param>
        private static void OnDomainUnload(object sender, EventArgs eventArgs)
        {
            DxLine.Dispose();
        }

        /// <summary>
        ///     On Post Reset
        /// </summary>
        /// <param name="args">The Args</param>
        private static void OnPostReset(EventArgs args)
        {
            DxLine.OnResetDevice();
        }

        /// <summary>
        ///     On Pre Reset
        /// </summary>
        /// <param name="args">The Args</param>
        private static void OnPreReset(EventArgs args)
        {
            DxLine.OnLostDevice();
        }

        /// <summary>
        ///     Fill HP Bar
        /// </summary>
        /// <param name="to"></param>
        /// <param name="from"></param>
        /// <param name="color">The Color</param>
        private void FillHpBar(int to, int from, Color color)
        {
            var sPos = StartPosition;
            for (var i = from; i < to; i++)
            {
                Drawing.DrawLine(sPos.X + i, sPos.Y, sPos.X + i, sPos.Y + 9, 1, color);
            }
        }

        /// <summary>
        ///     Fill HP Bar
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="color">The Color</param>
        private void FillHpBar(Vector2 from, Vector2 to, ColorBGRA color)
        {
            DxLine.Begin();

            DxLine.Draw(
                new[] { new Vector2((int)from.X, (int)from.Y + 4f), new Vector2((int)to.X, (int)to.Y + 4f) },
                color);     

            DxLine.End();
        }

        /// <summary>
        ///     The HP After Damage
        /// </summary>
        /// <param name="dmg">The Damage</param>
        /// <returns></returns>
        private Vector2 GetHpPosAfterDmg(float dmg)
        {
            var w = GetHpProc(dmg) * Width;
            return new Vector2(StartPosition.X + w, StartPosition.Y);
        }

        /// <summary>
        ///     HP Proc
        /// </summary>
        /// <param name="dmg">The Damage</param>
        /// <returns></returns>
        private float GetHpProc(float dmg)
        {
            var health = Unit.Health - dmg > 0 ? Unit.Health - dmg : 0;
            return health / Unit.MaxHealth;
        }

        #endregion
    }
}
