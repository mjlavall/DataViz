using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataViz
{
    public class DataPoint
    {
        public DateTime Date { get; set; }
        public string TimeString => Date.ToLongTimeString();
        public Triplet<double> Acceleration { get; set; }
        public Triplet<double> Rotation { get; set; }

        public double AX => Acceleration.X;
        public double AY => Acceleration.Y;
        public double AZ => Acceleration.Z;
        public double RX => Rotation.X/100;
        public double RY => Rotation.Y/100;
        public double RZ => Rotation.Z/100;


        public DataPoint(long timestamp, double ax, double ay, double az, double rx, double ry, double rz)
        {
            Date = UnixTimeStampToDateTime(timestamp);
            Acceleration = new Triplet<double>(ax, ay, az);
            Rotation = new Triplet<double>(rx, ry, rz);
        }
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public class Triplet<T>
    {
        public T X { get; set; }
        public T Y { get; set; }
        public T Z { get; set; }

        public Triplet(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class DataSet
    {
        public List<DataPoint> DataPoints { get; set; }
        public DateTime Date { get; set; }

        public DataSet(long timestamp)
        {
            Date = DataPoint.UnixTimeStampToDateTime(timestamp);
            DataPoints = new List<DataPoint>();
        }
        public void Add(DataPoint data)
        {
            DataPoints.Add(data);
        }
    }
}
