using System;
using System.Globalization;

namespace TransponderLib
{
    public interface ITransponderDataParser
    {
        DateTime ParseTime(string time);

        void ParseData(string inputString, out string tag, out int Xcoord, out int Ycoord, out int altitude,
            out DateTime time);
    }

    public class TransponderDataParser : ITransponderDataParser
    {
        public DateTime ParseTime(string time) => DateTime.ParseExact(time, "yyyyMMddHHmmssfff", CultureInfo.InvariantCulture);


        public void ParseData(string inputString, 
            out string tag, 
            out int xCoord, 
            out int yCoord, 
            out int altitude,
            out DateTime time)
        {
            if (inputString == null) throw new NullReferenceException("Den skal ikke være null, din skovl");

            inputString = inputString.Replace(" ", string.Empty);

            string[] strings = inputString.Split(';');

            if (strings.Length != 5) throw new ArgumentException("Wrong input");

            tag = strings[0];
            xCoord = int.Parse(strings[1]);
            yCoord = int.Parse(strings[2]);
            altitude = int.Parse(strings[3]);
            time = ParseTime(strings[4]);
        }
    }
}