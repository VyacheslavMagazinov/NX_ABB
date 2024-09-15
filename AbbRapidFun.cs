using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NxToQuaternion
{
    public class AbbRapidFun
    {

        const string headROBTARGET = "!# ----------------------------------------------\r\n!# ------ ROBTARGET\r\n!# ----------------------------------------------\r\n";
        const string headTOOL_DATA = "!# -----------------------------------------------\r\n!# ------ TOOL DATA\r\n!# -----------------------------------------------\r\n";
        const string headWOBJ_DATA = "!# -----------------------------------------------\r\n!# ------ WOBJ DATA\r\n!# -----------------------------------------------\r\n";
        const string zeroHead = "!====================================\r\n \r\n!====================================\r\n";
        const string temp = "!Check hemming tool docked on\r\n   s_checkToolcode 5;\r\n   \r\n!Deactivate M8DM1\r\nDeactUnit M8DM1;\r\n!Activate M9DM1\r\nActUnit M9DM1;\r\n!Reduce Acc\r\nAccSet 30,30\\FinePointRamp:=30;\r\n";

        const string zeroHLIN = ", 0, 0, 0, 0, 0, 0, 0, ";
        const string zeroGHO = "GHO := [[0,0,0],[0,0,0],[0,0,0]];";
        const string zeroGLHemDia = "GLHemDia := 0;";

        List<string> ProgList = new List<String>();

        public class PosData
        {
            public translation translationPD;
            public rotation rotationPD;
            public robconf robconfPD;
            public extax extaxPD;

            public override string ToString()
            {
                //string ret = "[[,541.602,550.675],[0.689281,0.145873,0.128191,0.697983],[-1,-1,-1,0],[9E+09,9E+09,-180.003,9E+09,9E+09,9E+09]];";
                //string ret =  string.Format( "{{X:{0} Y:{1} Z:{2} W:{3}}}", "","", "", "");
                string ret =  string.Format("[[ {0}, {1}, {2}], [{3}, {4}, {5}, {6}], [{7}, {8}, {9}, {10}], [{11}, {12}, {13}, {14}, {15}, {16}]];", translationPD.X, translationPD.Y, translationPD.Z, rotationPD.W, rotationPD.X, rotationPD.Y, rotationPD.Z, robconfPD.cf1, robconfPD.cf4, robconfPD.cf6, robconfPD.cfx, extaxPD.e1, extaxPD.e2, extaxPD.e3, extaxPD.e4, extaxPD.e5, extaxPD.e6);
                return ret;
            }

        }

        public class robtarget
        {
            public string name;
            public PosData PosDataRT;
            public int Order;
        }

        public class MoveFun
        {
            public string Type;
            public PosData PosDataMF;
            public string VSpeed;
            public string VZone;
            public string Tool;
            public string WObj;
        }

        public class translation
        {
            public String X;
            public String Y;
            public String Z;
        }

        public class rotation
        {
            public String W;
            public String X;
            public String Y;
            public String Z;
        }

        public class robconf
        {
            public String cf1;
            public String cf4;
            public String cf6;
            public String cfx;
        }

        public class extax
        {
            public String e1 = "9E+09";
            public String e2 = "9E+09";
            public String e3 = "9E+09";
            public String e4 = "9E+09";
            public String e5 = "9E+09";
            public String e6 = "9E+09";
        }

        static void CreatePROC(string PROCleName, List<MoveFun> SpotListIN, ref List<string> ProgListIN)
        {
            ProgListIN.Add("PROC " + PROCleName + "(\\switch hide)");
            ProgListIN.Add(zeroHead);
            ProgListIN.Add(temp);
            ProgListIN.Add(" ");

            ProgListIN.Add("nhOffsRoller := [[0,0,0],OrientZYX(0,0,0)];");
            ProgListIN.Add("GHO := [[0,0,0],[0,0,0],[0,0,0]];");
            ProgListIN.Add("NHsetTool rhh2_r3cl, -67.14, -66.98, 466.79, 6, 0, 134.933, 0, 10, 10;");
            ProgListIN.Add("GLHemDia := 0;");

            for (int i = 0; i < SpotListIN.Count; i++)
            {

                

            }


            ProgListIN.Add("!Stop if requested from PLC");
            ProgListIN.Add("s_seoc;");
            ProgListIN.Add(" ");
            ProgListIN.Add("ENDPROC");
        }

        static void CreateMODULE(string ModuleName, List<MoveFun> SpotListIN, ref List<string> ProgListIN)
        {
            ProgListIN.Add("MODULE " + ModuleName);
            ProgListIN.Add(" ");
            ProgListIN.Add(headROBTARGET);
            ProgListIN.Add(" ");

            //----------

            ProgListIN.Add(headTOOL_DATA);
            ProgListIN.Add(headWOBJ_DATA);

            CreatePROC("Name", SpotListIN, ref ProgListIN);

            ProgListIN.Add("ENDMODULE");
        }

        public static void ExpotToMOD(List<MoveFun> SpotListIN)
        {

        }



    }
}
