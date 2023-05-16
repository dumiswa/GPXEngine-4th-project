using System;
namespace GXPEngine
{
    public class LaserPath : LineSegment
    {

		LineSegment intersectedLine = null;
		bool hasReached = false;
		float speed = 0;

		MyGame myGame;
		public Vec2 direction;

		public LaserPath(Vec2 pStart, Vec2 pEnd, Vec2 pDirection) : base(pStart, pEnd, 0xffff33ff, 4)
        {
			direction = pDirection;
			myGame = (MyGame)game;
			//CheckForIntersections();
        }

		void Update() {


			for (int i = 0; i < 8; i++)
			{

				if (new Vec2(end.x - myGame.rocks[i].x, end.y - myGame.rocks[i].y).Length() <= 50)
				{
					myGame.DestroyLaserPath();
				}
			}

			if (new Vec2(end.x - myGame.smallAsteroid.x, end.y - myGame.smallAsteroid.y).Length() <= 50) {
				myGame.toExplode = true;
			}

			if (intersectedLine == null)
			{
				
				end = start + direction * speed;
				speed = speed + 60f;
				CheckForIntersections();
			}
			else {

				if (!hasReached)
				{
					Vec2 intersection = GetIntersectionPoint(intersectedLine);

					end = start + (intersection - start).Normalized() * ((intersection - start).Length() - 30);

					Vec2 lineSegmentVector = intersectedLine.end - intersectedLine.start;
					Vec2 incident = end - start;

					Vec2 newLaserVector = incident - 2 * incident.Dot(lineSegmentVector.Normal()) * lineSegmentVector.Normal();

					myGame.AddLaserPath(end, newLaserVector.Normalized());

					hasReached = true;
				}
			}

            if (end.x < -200 || end.x > game.width + 200 || end.y < -200 || end.y > game.height + 200)
            {
                myGame.DestroyLaserPath();
            }

		}

		void CheckForIntersections()
		{

			float distanceToCheck = float.MaxValue;
			
			for (int i = 0; i < myGame.pannels.Count; i++)
			{
				if (LineSegmentsIntersect(myGame.pannels[i]))
				{
					intersectedLine = myGame.pannels[i];

					if (Distance(start, myGame.pannels[i]) < distanceToCheck)
					{
						distanceToCheck = Distance(start, myGame.pannels[i]);
						intersectedLine = myGame.pannels[i];
					}
				}

			}

		}

		public bool LineSegmentsIntersect(LineSegment pannel)
		{
			Vec2 A = start;
			Vec2 B = end;
			Vec2 C = pannel.start;
			Vec2 D = pannel.end;

			Vec2 AB = B - A;
			Vec2 AC = C - A;
			Vec2 AD = D - A;
			Vec2 CD = D - C;
			Vec2 CA = A - C;
			Vec2 CB = B - C;

			if ((AB.CrossProduct(AC) * AB.CrossProduct(AD) < 0) && (CD.CrossProduct(CA) * CD.CrossProduct(CB) < 0))
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		public Vec2 GetIntersectionPoint(LineSegment pannel)
		{
			Vec2 A = start;
			Vec2 B = end;
			Vec2 C = pannel.start;
			Vec2 D = pannel.end;

			Vec2 AB = B - A;
			Vec2 AC = C - A;
			Vec2 CD = D - C;

			float cross_AB_AC = AB.CrossProduct(AC);
			float cross_AB_CD = AB.CrossProduct(CD);

			float t1 = -CD.CrossProduct(AC) / cross_AB_CD;

			Vec2 intersection = A + t1 * AB;
			return intersection;

		}

		float Distance(Vec2 _position, LineSegment _line)
		{

			Vec2 difference = _position - _line.start;
			Vec2 lineSegmentVector = _line.end - _line.start;
			Vec2 unitSegmentVector = lineSegmentVector.Normalized();
			float vectorPLength = difference.Dot(unitSegmentVector);
			Vec2 projection = unitSegmentVector * vectorPLength;

			return (difference - projection).Length();
		}

		

	}
}
