using System;

namespace EntityFX.Gdcame.Presentation.ConsoleClient.Common
{
    public class MenuItem
    {
        public string MenuText { get; set; }

        public Action MenuAction { get; set; }
    }
}