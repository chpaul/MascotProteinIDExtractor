using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MascotProteinIDExtractor
{
    public class PeptideQuery
    {
        private int _MissCleavage;
        private float _PeptideMR;
        private float _Delta;
        private int _NumOfMatch;
        private string _PeptideStr;
        private string _ModificationStr;
        private float _Score;
        private string _Term;
        private string _Protein;
        private int _PeptideNum;
        private int _QueryNum;
        public PeptideQuery(int argQueryNum, int argPeptideNum, int argMissCleavage,float argPeptideMR,float argDelta,int argNumOfMatch,string argPeptideStr,string argModificationStr,float argScore,string argProtein)
        {
            _PeptideNum = argPeptideNum;
            _MissCleavage = argMissCleavage;
            _PeptideMR = argPeptideMR;
            _Delta = argDelta;
            _NumOfMatch = argNumOfMatch;
            _PeptideStr = argPeptideStr;
            _ModificationStr = argModificationStr;
            _Score = argScore;
            _Protein = argProtein;
            _QueryNum = argQueryNum;

        }
        public int QueryNumber
        {
            get { return _QueryNum; }
        }
        public int PeptideNumber
        {
            get { return _PeptideNum; }
        }
        public int MissCleavage
        {
            get { return MissCleavage; }
        }

        public float PeptideMR
        {
            get { return _PeptideMR; }
        }

        public float Delta
        {
            get { return _Delta; }
        }

        public int NumOfMatch
        {
            get { return _NumOfMatch; }
        }

        public string PeptideStr
        {
            get { return _PeptideStr; }
        }

        public string ModificationStr
        {
            get { return _ModificationStr; }
        }

        public float Score
        {
            get { return _Score; }
        }

        public string Term
        {
            get { return _Term; }
            set { _Term = value; }
        }

        public string Protein
        {
            get { return _Protein; }
            set { _Protein = value; }
        }
    }
}
