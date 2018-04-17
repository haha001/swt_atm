using System;
using System.Collections.Generic;
using System.Text;

namespace TransponderLib
{
	//This class is used to detect whether or not any planes are colliding
	public class CollisionDetector
	{
		public event EventHandler<CollisionEventArgs> SeparationEvent;
		public event EventHandler<CollisionEventArgs> NoSeperationEvent;
		public List<Tuple<Plane, Plane>> Collisions = new List<Tuple<Plane, Plane>>();

		public void DetectCollision(List<Plane> planes)
		{
			//We don't want to deal with empty lists or less than 2 planes
			if (planes == null || planes.Count < 2)
				return;


			//Check if any new collisions have occured (The dreaded n^2 loop)
			for (int i = 0; i < planes.Count - 1; ++i)
			{
				for (int j = i + 1; j < planes.Count; ++j)
				{
					if (planes[j].Speed == 0)
						break; //Break in case of new plane


					int differenceInAltitude = planes[j].Altitude - planes[i].Altitude;
					if (differenceInAltitude < 300 && differenceInAltitude > -300)
					{
						//Planes are within 300 meters in altitude, otherwise just ignore
						double distance = CalculateDistance(planes[i], planes[j]);

						if (distance < 5000)
						{
							//Verify that collision is not already added to list
							if (Collisions.Contains(Tuple.Create(planes[i], planes[j])))
								break;

							Collisions.Add(Tuple.Create(planes[i], planes[j]));
							SeparationEvent?.Invoke(this, new CollisionEventArgs(planes[i], planes[j]));
						}
					}
				}
			}
		}

		public void VerifyCollisions()
		{
			foreach (var collision in Collisions.ToArray())
			{
				if (CalculateDistance(collision.Item1, collision.Item2) >= 5000)
				{
					NoSeperationEvent?.Invoke(this, new CollisionEventArgs(collision.Item1, collision.Item2));
					Collisions.Remove(collision);

				}
			}
		}

		private double CalculateDistance(Plane plane1, Plane plane2)
		{
			//Calculate euclidian distance
			double xDifference = plane2.XCoord - plane1.XCoord;
			double yDifference = plane2.YCoord - plane1.YCoord;

			double hypotenuse = Math.Pow(xDifference, 2) + Math.Pow(yDifference, 2);

			return Math.Sqrt(hypotenuse);


		}

	}


	public class CollisionEventArgs : EventArgs
	{
		public Plane CollidingPlane1 { get; set; }
		public Plane CollidingPlane2 { get; set; }

		public CollisionEventArgs(Plane plane1, Plane plane2)
		{
			CollidingPlane1 = plane1;
			CollidingPlane2 = plane2;
		}
	}
}
