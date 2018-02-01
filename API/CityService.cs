using System.Drawing;

namespace mejor_precio_2.API

{
    public class CityService
    {
        private PointF[] bsAs = {new PointF((float)-34.529187,(float)-58.460999),
                                new PointF((float)-34.548418,(float)-58.499451),
                                new PointF((float)-34.617387,(float)-58.533783),
                                new PointF((float)-34.655804,(float)-58.526917),
                                new PointF((float)-34.704082,(float)-58.462029),
                                new PointF((float)-34.662864,(float)-58.424950),
                                new PointF((float)-34.658628,(float)-58.412933),
                                new PointF((float)-34.662582,(float)-58.407097),
                                new PointF((float)-34.660322,(float)-58.390274),
                                new PointF((float)-34.654957,(float)-58.370705),
                                new PointF((float)-34.646766,(float)-58.359375),
                                new PointF((float)-34.638293,(float)-58.355942),
                                new PointF((float)-34.632643,(float)-58.346672),
                                new PointF((float)-34.625863,(float)-58.328476),
                                new PointF((float)-34.552377,(float)-58.388901)};

        private bool IsPointInPolygon(PointF[] polygon, PointF point)

        {

            bool isInside = false;

            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)

            {

                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&

                (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))

                {

                    isInside = !isInside;

                }

            }

            return isInside;

        }

        public bool IsInBsAs(PointF point)
        {
            return IsPointInPolygon(bsAs, point);
        }
    }
}