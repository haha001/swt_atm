﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TransponderReceiver;

[assembly: InternalsVisibleTo("TestUnit")]
[assembly: InternalsVisibleTo("ATM.Test.Integration")]

namespace TransponderLib
{
    public class ATM
    {
        private readonly ITransponderReceiver _receiver;
        private ITransponderDataParser _dataParser;
        private IOutput _output;
        private ICollisionDetector _detector;

        internal List<Plane> _planes = new List<Plane>();

        public ATM(ITransponderReceiver receiver, ITransponderDataParser parser, IOutput output, ICollisionDetector detector)
        {
            _detector = detector;
            _detector.SeparationEvent += DetectorOnSeparationEvent;
            _detector.NoSeperationEvent += DetectorOnNoSeperationEvent;
            _dataParser = parser;
            _receiver = receiver;
            _receiver.TransponderDataReady += ReceiverOnTransponderDataReady;
            _output = output;
                
            UpdateScreen();
        }

        private void DetectorOnNoSeperationEvent(object sender, CollisionEventArgs e)
        {
            e.CollidingPlane1.Separation = false;
            e.CollidingPlane2.Separation = false;
        }

        private void DetectorOnSeparationEvent(object sender, CollisionEventArgs e)
        {
            e.CollidingPlane1.Separation = true;
            e.CollidingPlane1.SeparationTime = e.CollidingPlane1.LastUpdated;
            e.CollidingPlane2.Separation = true;
            e.CollidingPlane2.SeparationTime = e.CollidingPlane2.LastUpdated;
        }

        internal void UpdateScreen()
        {
            _output.Reset();
            _output.Print(String.Format("{0,-2}Plane Tag {0,-2} | {0,-2}Plane Speed {0,-2} | {0,-2}Plane Course {0,-2} | {0,-2}Plane Separation {0,-2} | {0,-2}Plane Updated {0,-2} | {0,-2}Planes total: {1,-2:D} {0,-2}",
                string.Empty, _planes.Count));


            foreach (var plane in _planes)
            {
                _output.Print(String.Format(
                    "{0,-2}{1,-9} {0,-2} | {0,-5}{2,-6:0.##} {0,-4} | {0,-5}{3,-6:0.##} {0,-5} | {0,-7}{4,-7} {0,-6} | {0,-2}{5,-10:g} {0,-2}",
                    string.Empty, plane.Tag, plane.Speed, plane.Course, plane.Separation ? "WARNING" : "  ---  ", plane.LastUpdated.TimeOfDay));
            }

        }

        internal void UpdatePlane(Plane planeToUpdate, int xCoord, int yCoord, int altitude, DateTime time)
        {
            // Checking if new time is a valid time. Here it must be later than LastUpdated
            if (DateTime.Compare(time, planeToUpdate.LastUpdated) <= 0) return;


            int deltaXCoord = Math.Abs(xCoord - planeToUpdate.XCoord);
            int deltaYCoord = Math.Abs(yCoord - planeToUpdate.YCoord);

            double euclidianDistance = Math.Sqrt(deltaXCoord * deltaXCoord + deltaYCoord * deltaYCoord);

            double deltaTime = time.TimeOfDay.TotalMilliseconds - planeToUpdate.LastUpdated.TimeOfDay.TotalMilliseconds;

            double direction = (Math.Atan2(deltaYCoord, deltaXCoord) * (180 / Math.PI));

            double speed = euclidianDistance / (deltaTime / 1000);


            planeToUpdate.LastUpdated = time;
            planeToUpdate.Speed = speed;
            planeToUpdate.XCoord = xCoord;
            planeToUpdate.YCoord = yCoord;
            planeToUpdate.Altitude = altitude;
            planeToUpdate.Course = direction;
        }
        internal void HandleData(string data)
        {
            string tag;
            int xCoord;
            int yCoord;
            int altitude;
            DateTime time;

            try
            {
                _dataParser.ParseData(data, out tag, out xCoord, out yCoord, out altitude, out time);
            }
            catch (Exception e)
            {
                _output.Print(e.Message);
                return;
            }

            // Checking if inside appropriate airspace
            if (CheckAirspace(xCoord, yCoord) && _planes.Exists(s => s.Tag == tag))
                UpdatePlane(_planes.Find(p => p.Tag == tag), xCoord, yCoord, altitude, time);

            else if (CheckAirspace(xCoord, yCoord))
                _planes.Add(new Plane()
                {
                    Tag = tag,
                    XCoord = xCoord,
                    YCoord = yCoord,
                    Altitude = altitude,
                    LastUpdated = time,
                    Course = 0,
                    Speed = 0
                });
            else
                _planes.Remove(_planes.Find(s => s.Tag == tag));
        }

        internal bool CheckAirspace(int xCoord, int yCoord)
        {
            return xCoord <= 90000 & xCoord >= 10000 && yCoord <= 90000 & yCoord >= 10000;
        }

        internal void ReceiverOnTransponderDataReady(object sender, RawTransponderDataEventArgs rawTransponderDataEventArgs)
        {
            foreach (var data in rawTransponderDataEventArgs.TransponderData)
            {
                HandleData(data);
            }
            _detector.VerifyCollisions();
            _detector.DetectCollision(_planes);
            // Used to check planes

            UpdateScreen();
        }
    }

}

