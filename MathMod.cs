using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using NXOpen;

namespace NxToQuaternion
{
    public class MathMod
    {

        public static Quaternion MatrixToQuaternion(Matrix3x3 m3d_in)
        {
            Quaternion Qt;

            float t = (float)m3d_in.Xx + (float)m3d_in.Yy + (float)m3d_in.Zz;

            if (t > 0)
            {
                float s = (float)Math.Sqrt(t + 1) * 2;

                Qt.W = 0.25f * s;
                Qt.X = ((float)m3d_in.Zy - (float)m3d_in.Yz) / s;
                Qt.Y = ((float)m3d_in.Xz - (float)m3d_in.Zx) / s;
                Qt.Z = ((float)m3d_in.Yx - (float)m3d_in.Xy) / s;
                //Qt.Z = 1;
            }
            else if (m3d_in.Xx > m3d_in.Yy && m3d_in.Xx > m3d_in.Zz)
            {
                float s = (float)Math.Sqrt(1 + m3d_in.Xx - m3d_in.Yy - m3d_in.Zz) * 2;

                Qt.W = ((float)m3d_in.Zy - (float)m3d_in.Yz) / s;
                Qt.X = 0.25f * s;
                Qt.Y = ((float)m3d_in.Xy + (float)m3d_in.Yx) / s;
                Qt.Z = ((float)m3d_in.Xz + (float)m3d_in.Zx) / s;
                //Qt.Z = 2;
            }
            else if (m3d_in.Yy > m3d_in.Zz)
            {
                float s = (float)Math.Sqrt(1 + m3d_in.Yy - m3d_in.Xx - m3d_in.Zz) * 2;

                Qt.W = ((float)m3d_in.Xz - (float)m3d_in.Zx) / s;
                Qt.X = ((float)m3d_in.Xy + (float)m3d_in.Yx) / s;
                Qt.Y = 0.25f * s;
                Qt.Z = ((float)m3d_in.Yz + (float)m3d_in.Zy) / s;
                //Qt.Z = 3;
            }
            else
            {
                float s = (float)Math.Sqrt(1 + m3d_in.Zz - m3d_in.Xx - m3d_in.Yy) * 2;

                Qt.W = ((float)m3d_in.Yx - (float)m3d_in.Xy) / s;
                Qt.X = ((float)m3d_in.Xz + (float)m3d_in.Zx) / s;
                Qt.Y = ((float)m3d_in.Yz + (float)m3d_in.Zy) / s;
                Qt.Z = 0.25f * s;
            }

            return Qt;
        }

        public static Matrix3x3 QuaternionToMatrix(Quaternion Qt)
        {
            Matrix3x3 m3d;

            double w = Qt.W;
            double x = Qt.X;
            double y = Qt.Y;
            double z = Qt.Z;


            m3d.Xx = 1 - 2 * (y * y + z * z);
            m3d.Xy = 2 * (x * y - w * z);
            m3d.Xz = 2 * (x * z + w * y);

            m3d.Yx = 2 * (x * y + w * z);
            m3d.Yy = 1 - 2 * (x * x + z * z);
            m3d.Yz = 2 * (y * z - w * x);

            m3d.Zx = 2 * (x * z - w * y);
            m3d.Zy = 2 * (y * z + w * x);
            m3d.Zz = 1 - 2 * (x * x + y * y);

            return m3d;
        }

        public static Quaternion EulerToQuaternion(double yaw, double pitch, double roll, string order)
        {
            Quaternion Qt;
            Qt.X = 1;
            Qt.Y = 1;
            Qt.Z = 1;
            Qt.W = 1;

            yaw = yaw / 2;     //Z
            pitch = pitch / 2; //Y
            roll = roll / 2;   //X

            switch (order)
            {
                case "ZYXr":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Sin(roll) * Math.Cos(yaw) - Math.Sin(pitch) * Math.Cos(roll) * Math.Sin(yaw));
                    Qt.Y = (float)(Math.Cos(pitch) * Math.Sin(roll) * Math.Sin(yaw) + Math.Sin(pitch) * Math.Cos(roll) * Math.Cos(yaw));
                    Qt.Z = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "XYXr":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Sin(yaw) + Math.Cos(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.Y = (float)(Math.Sin(pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    Qt.Z = (float)(Math.Sin(pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Cos(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "YZXr":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Sin(roll) * Math.Cos(yaw) - Math.Sin(-pitch) * Math.Cos(roll) * Math.Sin(yaw));
                    Qt.Y = (float)(-Math.Cos(-pitch) * Math.Sin(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Cos(roll) * Math.Cos(yaw));
                    Qt.Z = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "XZXr":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Sin(yaw) + Math.Cos(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.Y = (float)(-Math.Sin(-pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    Qt.Z = (float)(Math.Sin(-pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Cos(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "XZYr":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Sin(roll) * Math.Cos(yaw) - Math.Sin(pitch) * Math.Cos(roll) * Math.Sin(yaw));
                    Qt.Y = (float)(Math.Cos(pitch) * Math.Sin(roll) * Math.Sin(yaw) + Math.Sin(pitch) * Math.Cos(roll) * Math.Cos(yaw));
                    Qt.Z = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "YZYr":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Sin(yaw) + Math.Cos(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.Y = (float)(Math.Sin(pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    Qt.Z = (float)(Math.Sin(pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Cos(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "ZXYr":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Sin(roll) * Math.Cos(yaw) - Math.Sin(-pitch) * Math.Cos(roll) * Math.Sin(yaw));
                    Qt.Y = (float)(-Math.Cos(-pitch) * Math.Sin(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Cos(roll) * Math.Cos(yaw));
                    Qt.Z = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "YXYr":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Sin(yaw) + Math.Cos(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.Y = (float)(-Math.Sin(-pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    Qt.Z = (float)(Math.Sin(-pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Cos(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "YXZr":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Sin(roll) * Math.Cos(yaw) - Math.Sin(pitch) * Math.Cos(roll) * Math.Sin(yaw));
                    Qt.Y = (float)(Math.Cos(pitch) * Math.Sin(roll) * Math.Sin(yaw) + Math.Sin(pitch) * Math.Cos(roll) * Math.Cos(yaw));
                    Qt.Z = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "ZXZr":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Sin(yaw) + Math.Cos(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.Y = (float)(Math.Sin(pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    Qt.Z = (float)(Math.Sin(pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Cos(pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "XYZr":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Sin(roll) * Math.Cos(yaw) - Math.Sin(-pitch) * Math.Cos(roll) * Math.Sin(yaw));
                    Qt.Y = (float)(-Math.Cos(-pitch) * Math.Sin(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Cos(roll) * Math.Cos(yaw));
                    Qt.Z = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Cos(yaw) + Math.Sin(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "ZYZr":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Sin(yaw) + Math.Cos(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.Y = (float)(-Math.Sin(-pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    Qt.Z = (float)(Math.Sin(-pitch) * Math.Cos(roll) * Math.Sin(yaw) - Math.Sin(-pitch) * Math.Sin(roll) * Math.Cos(yaw));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(roll) * Math.Cos(yaw) - Math.Cos(-pitch) * Math.Sin(roll) * Math.Sin(yaw));
                    break;

                case "ZYXs":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Sin(yaw) * Math.Cos(roll) - Math.Sin(-pitch) * Math.Cos(yaw) * Math.Sin(roll));
                    Qt.Y = (float)(-Math.Cos(-pitch) * Math.Sin(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Cos(yaw) * Math.Cos(roll));
                    Qt.Z = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "XYXs":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Sin(roll) + Math.Cos(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.Y = (float)(Math.Sin(pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    Qt.Z = (float)(Math.Sin(pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Cos(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "YZXs":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Sin(yaw) * Math.Cos(roll) - Math.Sin(pitch) * Math.Cos(yaw) * Math.Sin(roll));
                    Qt.Y = (float)(Math.Cos(pitch) * Math.Sin(yaw) * Math.Sin(roll) + Math.Sin(pitch) * Math.Cos(yaw) * Math.Cos(roll));
                    Qt.Z = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "XZXs":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Sin(roll) + Math.Cos(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.Y = (float)(-Math.Sin(-pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    Qt.Z = (float)(Math.Sin(-pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Cos(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "XZYs":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Sin(yaw) * Math.Cos(roll) - Math.Sin(-pitch) * Math.Cos(yaw) * Math.Sin(roll));
                    Qt.Y = (float)(-Math.Cos(-pitch) * Math.Sin(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Cos(yaw) * Math.Cos(roll));
                    Qt.Z = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "YZYs":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Sin(roll) + Math.Cos(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.Y = (float)(Math.Sin(pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    Qt.Z = (float)(Math.Sin(pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Cos(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "ZXYs":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Sin(yaw) * Math.Cos(roll) - Math.Sin(pitch) * Math.Cos(yaw) * Math.Sin(roll));
                    Qt.Y = (float)(Math.Cos(pitch) * Math.Sin(yaw) * Math.Sin(roll) + Math.Sin(pitch) * Math.Cos(yaw) * Math.Cos(roll));
                    Qt.Z = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "YXYs":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Sin(roll) + Math.Cos(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.Y = (float)(-Math.Sin(-pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    Qt.Z = (float)(Math.Sin(-pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Cos(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "YXZs":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Sin(yaw) * Math.Cos(roll) - Math.Sin(-pitch) * Math.Cos(yaw) * Math.Sin(roll));
                    Qt.Y = (float)(-Math.Cos(-pitch) * Math.Sin(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Cos(yaw) * Math.Cos(roll));
                    Qt.Z = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "ZXZs":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Sin(roll) + Math.Cos(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.Y = (float)(Math.Sin(pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    Qt.Z = (float)(Math.Sin(pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Cos(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "XYZs":
                    Qt.X = (float)(Math.Cos(pitch) * Math.Sin(yaw) * Math.Cos(roll) - Math.Sin(pitch) * Math.Cos(yaw) * Math.Sin(roll));
                    Qt.Y = (float)(Math.Cos(pitch) * Math.Sin(yaw) * Math.Sin(roll) + Math.Sin(pitch) * Math.Cos(yaw) * Math.Cos(roll));
                    Qt.Z = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(pitch) * Math.Cos(yaw) * Math.Cos(roll) + Math.Sin(pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                case "ZYZs":
                    Qt.X = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Sin(roll) + Math.Cos(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.Y = (float)(-Math.Sin(-pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    Qt.Z = (float)(Math.Sin(-pitch) * Math.Cos(yaw) * Math.Sin(roll) - Math.Sin(-pitch) * Math.Sin(yaw) * Math.Cos(roll));
                    Qt.W = (float)(Math.Cos(-pitch) * Math.Cos(yaw) * Math.Cos(roll) - Math.Cos(-pitch) * Math.Sin(yaw) * Math.Sin(roll));
                    break;

                default:
                    break;
            }
            return Qt;
        }

        public static (float yaw, float pitch, float roll) QuaternionToEuler(Quaternion q)
        {
            float yaw = (float)Math.Atan2(2.0f * (q.Y * q.Z + q.W * q.X), q.W * q.W - q.X * q.X - q.X * q.X + q.Z * q.Z);    //Z
            float pitch = (float)Math.Asin(-2.0f * (q.X * q.Z - q.W * q.Y));                                                 //Y
            float roll = (float)Math.Atan2(2.0f * (q.X * q.Y + q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z);   //X

            return (yaw, pitch, roll);
        }

    }
}
