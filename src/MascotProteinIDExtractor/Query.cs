using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace MascotProteinIDExtractor
{
    public class Query
    {
        private int _QueryNum;
        private string _Title;
        private int _RTinseconds;
        private int _Scans;
        private int _Charge;
        private float _Mass_min;
        private float _Mass_max;
        private float _Int_min;
        private float _int_max;

        public Query(int argQueryNum)
        {
            _QueryNum = argQueryNum;
        }

        public int QueryNum
        {
            get { return _QueryNum; }
            set { _QueryNum = value; }
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public int RTinSeconds
        {
            get { return _RTinseconds; }
            set { _RTinseconds = value; }
        }

        public int ScanNum
        {
            get { return _Scans; }
            set { _Scans = value; }
        }

        public int Charge
        {
            get { return _Charge; }
            set { _Charge = value; }
        }

        public float Massmin
        {
            get { return _Mass_min; }
            set { _Mass_min = value; }
        }

        public float Massmax
        {
            get { return _Mass_max; }
            set { _Mass_max = value; }
        }

        public float Intmin
        {
            get { return _Int_min; }
            set { _Int_min = value; }
        }

        public float intmax
        {
            get { return _int_max; }
            set { _int_max = value; }
        }
    }
}
