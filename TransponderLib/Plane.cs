using System;
using System.Runtime.CompilerServices;

namespace TransponderLib
{
    public class Plane
    {
        public string Tag { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }

        private int _altitude;
        public int Altitude
        {
            get => this._altitude;
            set => _altitude = value >= 0 ? value : 0;
        }

        public double Speed { get; set; }
        public double Course { get; set; }
        public DateTime LastUpdated { get; set; }

		public DateTime SeparationTime { get; set; }
		public bool Separation { get; set; }

        public Plane()
        {
            // uwotm8
        }
    }
}