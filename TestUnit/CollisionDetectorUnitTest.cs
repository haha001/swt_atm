using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TransponderLib;

namespace TestUnit
{
	[TestFixture]
	public class CollisionDetectorUnitTest
	{
		private List<Plane> _planes;
		private CollisionDetector _collisionDetector;
		[SetUp]
		public void SetUp()
		{
			_planes = new List<Plane>();
			_collisionDetector = new CollisionDetector();
		}

		[TestCase]
		public void CollisionEventArgs_TwoPlanesColliding_PlanesReturnedInEventhandler()
		{
			CollisionEventArgs ea = new CollisionEventArgs(null, null);
			_planes.Add(new Plane() { Altitude = 4000, Tag = "Hello", XCoord = 5000, YCoord = 5000, Speed = 1});
			_planes.Add(new Plane() { Altitude = 4000, Tag = "Wut", XCoord = 5000, YCoord = 5000, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) =>
			{
				ea.CollidingPlane1 = e.CollidingPlane1;
				ea.CollidingPlane2 = e.CollidingPlane2;
			};

			_collisionDetector.DetectCollision(_planes);
			
			Assert.That(_planes.Exists(o => o.Tag == ea.CollidingPlane1.Tag) && _planes.Exists(o => o.Tag == ea.CollidingPlane2.Tag));
		}

		[TestCase]
		public void DetectCollision_TwoPlanesAltitudeDifference300_NoEventRaised()
		{
			bool collision = false;
			_planes.Add(new Plane() {Altitude = 4000});
			_planes.Add(new Plane() {Altitude = 4300});

			_collisionDetector.SeparationEvent += (obj, e) => collision = true;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collision == false);
		}

		[TestCase]
		public void DetectCollision_TwoPlanesAltitudeDifference299_EventRaised()
		{
			bool collision = false;
			_planes.Add(new Plane() { Altitude = 4000, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 4299, XCoord = 5001, YCoord = 5001, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) => collision = true;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collision == true);
		}

		[TestCase]
		public void DetectCollision_TwoPlanesHorizontalDifference5000_NoEventRaised()
		{
			bool collision = false;
			_planes.Add(new Plane() { XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { XCoord = 5000, YCoord = 10000, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) => collision = true;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collision == false);
		}

		[TestCase]
		public void DetectCollision_TwoPlanesHorizontalDifference4999_EventRaised()
		{
			bool collision = false;
			_planes.Add(new Plane() { XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { XCoord = 9999, YCoord = 5000, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) => collision = true;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collision == true);
		}

		[TestCase]
		public void DetectCollision_TwoPlanesWithin5000MetersOver300Altitude_NoEventRaised()
		{
			bool collision = false;
			_planes.Add(new Plane() { Altitude = 4000, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 8000, XCoord = 5001, YCoord = 5001, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) => collision = true;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collision == false);
		}

		[TestCase]
		public void DetectCollision_FourPlanesOneCollision_OneEventRaised()
		{
			int collisions = 0;
			_planes.Add(new Plane() { Altitude = 4000, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 8000, XCoord = 5001, YCoord = 5001, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 4200, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 9000, XCoord = 5001, YCoord = 5001, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) => ++collisions;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collisions == 1);
		}

		[TestCase]
		public void DetectCollision_FourPlanesThreeCollisions_ThreeEventsRaised()
		{
			int collisions = 0;
			_planes.Add(new Plane() { Altitude = 4000, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 4100, XCoord = 5001, YCoord = 5001, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 4200, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 9000, XCoord = 5001, YCoord = 5001, Speed = 1 });

			_collisionDetector.SeparationEvent += (obj, e) => ++collisions;

			_collisionDetector.DetectCollision(_planes);

			Assert.That(collisions == 3);
		}

		[TestCase]
		public void VerifyCollision_TwoPlanesStillCollide_NoEventRaised()
		{
			bool eventRaised = false;
			_planes.Add(new Plane() { Altitude = 4000, XCoord = 5000, YCoord = 5000, Speed = 1 });
			_planes.Add(new Plane() { Altitude = 4100, XCoord = 5001, YCoord = 5001, Speed = 1 });
			
			_collisionDetector.NoSeperationEvent += (obj, e) => eventRaised = true;
			
			_collisionDetector.DetectCollision(_planes);
			_collisionDetector.VerifyCollisions();

			Assert.That(eventRaised == false);
		}

		[TestCase]
		public void VerifyCollision_TwoPlanesNoLongerCollide_EventRaised()
		{
			bool eventRaised = false;
			_planes.Add(new Plane() {Altitude = 4000, XCoord = 5000, YCoord = 5000, Speed = 1});
			_planes.Add(new Plane() {Altitude = 4100, XCoord = 5001, YCoord = 5001, Speed = 1});

			_collisionDetector.NoSeperationEvent += (obj, e) => eventRaised = true;

			_collisionDetector.DetectCollision(_planes);

			_planes[0].XCoord = 100000;
			_collisionDetector.VerifyCollisions();

			Assert.That(eventRaised == true);
		}
	}
}
