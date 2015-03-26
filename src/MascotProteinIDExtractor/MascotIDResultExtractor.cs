using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COL.MassLib;
namespace MascotProteinIDExtractor
{

    public class MascotIDResultExtractor
    {
        private string _datFile;
        private string _rawFile;
        private MascotReader _MascotRdr;
        private ThermoRawReader _RawRdr;
        private StringBuilder _SB;
        private float _minMascotScore;
        public MascotIDResultExtractor(string argDATFile, string argRawFile)
        {
            _rawFile = argRawFile;
            _datFile = argDATFile;
        }

        public MascotReader MascotReader
        {
            get { return _MascotRdr; }
        }
        public float MinMascotScore
        {
            set { _minMascotScore = value; }
            get { return _minMascotScore; }
        }
        public void ReadMascotFile()
        {

            _MascotRdr = new MascotReader(_datFile);
            _MascotRdr.Read();
            _RawRdr = new ThermoRawReader(_rawFile);
            _SB= new StringBuilder();
            string Title = "Protein_Name,Peptide_Sequence,Peptide_Mass,Peptide m/z,Score,Query Scan,Query Time,Ion mz,Start Scan,Start_Search_Time_(Mins),End Scan,End_Search_Time_(Mins),Apex Scan,Apex Time";
            _SB.Append(Title).AppendLine();

        }

        public void ProcessAll()
        {
            foreach (int key in _MascotRdr.PeptideQueries.Keys)
            {
                ProcessOneQuery(key);
            }
        }
        public void Export(string argExportFile)
        {
            StreamWriter sw = new StreamWriter(argExportFile);
            sw.Write(_SB.ToString());
            sw.Close();
        }
        public void ProcessOneQuery(int argQueryNum)
        {
           // if (_MascotRdr.PeptideQueries.ContainsKey(argQueryNum))
            {
                //Get Scan Num
              
                foreach (PeptideQuery pepQuery in _MascotRdr.PeptideQueries[argQueryNum])
                {
                    if (pepQuery.PeptideNumber != 1 || pepQuery.Score<_minMascotScore )
                    {
                        continue;
                    }
                    if (_MascotRdr.Queries[argQueryNum].ScanNum == 13077)
                    {
                        
                    }
                    int IdentifiedScanNum = _MascotRdr.Queries[argQueryNum].ScanNum;
                    if (IdentifiedScanNum > _RawRdr.NumberOfScans)
                    {
                        _SB.Append(pepQuery.Protein + ",");
                        _SB.Append(pepQuery.PeptideStr + ",");
                        _SB.Append(pepQuery.PeptideMR + ",");
                        _SB.Append(_MascotRdr.ExperimentMZ[argQueryNum] + ",");
                        _SB.Append(pepQuery.Score + ",");
                        _SB.Append(_MascotRdr.Queries[argQueryNum].ScanNum + ",");
                        _SB.Append(_MascotRdr.Queries[argQueryNum].RTinSeconds/60.0 + ",");
                        _SB.Append("NA,");
                        _SB.Append( "NA,NA,");
                        _SB.Append("NA,NA,");
                        _SB.Append("NA,NA,");
                        _SB.AppendLine();
                    }
                    else
                    {
                        MSScan TargetScan = _RawRdr.ReadScan(IdentifiedScanNum);
                        List<double> range = GetRange(IdentifiedScanNum, TargetScan.ParentMZ);
                        _SB.Append(pepQuery.Protein + ",");
                        _SB.Append(pepQuery.PeptideStr + ",");
                        _SB.Append(pepQuery.PeptideMR + ",");
                        _SB.Append(_MascotRdr.ExperimentMZ[argQueryNum] + ",");
                        _SB.Append(pepQuery.Score + ",");
                        _SB.Append(_MascotRdr.Queries[argQueryNum].ScanNum + ",");
                        _SB.Append(_MascotRdr.Queries[argQueryNum].RTinSeconds/60.0 + ",");
                        _SB.Append(TargetScan.ParentMZ + ",");
                        _SB.Append(range[0] + "," + range[3] + ",");
                        _SB.Append(range[1] + "," + range[4] + ",");
                        _SB.Append(range[2] + "," + range[5] + ",");
                        _SB.AppendLine();
                    }
                    

                }
            }
        }

        private List<double> GetRange(int argScanNum,float argMZ)
        {
            int ScanInterval = 20;
            List<double> Result = new List<double>();
            int BeginScan = 0;
            double BeginScanTime = 0;
            int EndScan =  argScanNum-1;
            int StartScan = EndScan - ScanInterval;
            float MaxIntensity = 0;
            int MaxIntensityScan = 0;
            double MaxIntensityScanTime = 0;
            //Front
            bool FoundMZ = false;
            while(true)
            {
                if (StartScan <= 0)
                {
                    StartScan = 1;
                }
                List<MSScan> Scans = _RawRdr.ReadScanWMSLevel(StartScan, EndScan, 1);
                if (Scans.Count == 0)
                {
                    break;
                }
                for(int i = Scans.Count -1 ; i>=0;i--)
                {
                    int Idx = MassUtility.GetClosestMassIdx(Scans[i].RawMZs, argMZ);
                    if (MassUtility.GetMassPPM(Scans[i].RawMZs[Idx], argMZ) < 10)
                    {
                        BeginScan = Scans[i].ScanNo;
                        BeginScanTime = Scans[i].Time;
                        if (Scans[i].RawIntensities[Idx] > MaxIntensity)
                        {
                            MaxIntensityScan = BeginScan;
                            MaxIntensity = Scans[i].RawIntensities[Idx];
                            MaxIntensityScanTime = Scans[i].Time;
                        }
                    }
                }
                if (BeginScan != Scans[0].ScanNo)
                {
                    break;
                }
                else
                {
                    EndScan = StartScan - 1;
                    StartScan = StartScan - ScanInterval;
                }
            }
             //Back
            int LastScan = 0;
            double LastScanTime = 0;
            StartScan = argScanNum + 1;
            EndScan = StartScan + ScanInterval;
            while(true)
            {
                if (EndScan >= _RawRdr.NumberOfScans)
                {
                    EndScan =  _RawRdr.NumberOfScans;
                }
                List<MSScan> Scans = _RawRdr.ReadScanWMSLevel(StartScan, EndScan, 1);
                if (Scans.Count == 0)
                {
                    break;
                }
                for(int i = 0 ; i<Scans.Count;i++)
                {
                    int Idx = MassUtility.GetClosestMassIdx(Scans[i].RawMZs, argMZ);
                    if (MassUtility.GetMassPPM(Scans[i].RawMZs[Idx], argMZ) < 10)
                    {
                        LastScan = Scans[i].ScanNo;
                        LastScanTime = Scans[i].Time;
                        if (Scans[i].RawIntensities[Idx] > MaxIntensity)
                        {
                            MaxIntensityScan = LastScan;
                            MaxIntensity = Scans[i].RawIntensities[Idx];
                            MaxIntensityScanTime = Scans[i].Time;
                        }
                    }
                }
                if (LastScan!=Scans[Scans.Count-1].ScanNo)
                {
                    break;
                }
                else
                {
                    StartScan = EndScan + 1;
                    EndScan = StartScan + ScanInterval;
                }
            }

            Result.Add(BeginScan);
            Result.Add(LastScan);
            Result.Add(MaxIntensityScan);
            Result.Add(BeginScanTime);
            Result.Add(LastScanTime);
            Result.Add(MaxIntensityScanTime);
             return Result;
        }
    }
}
