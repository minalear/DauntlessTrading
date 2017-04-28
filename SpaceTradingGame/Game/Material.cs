using System;


namespace SpaceTradingGame.Game
{
    /// <summary>
    /// Materials generated and traded for money or other materials.  Trade goods.
    /// </summary>
    public class Material
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public double BaseValue { get; set; }
        public double Weight { get; set; }
    }
}
