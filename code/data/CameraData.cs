using System;

namespace data
{
    public class CameraData
    {
        public int Number { get; set; }
        public string Camera { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Number} | {Camera} | {Latitude} | {Longitude}";
        }
    }
}
