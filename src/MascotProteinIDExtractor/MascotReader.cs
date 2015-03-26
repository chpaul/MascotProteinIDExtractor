﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//https://github.com/princelab/mspire-mascot-dat/blob/master/spec/reference/dat_format_reference.md
namespace MascotProteinIDExtractor
{

    public class MascotReader
    {
        string _fileName;
        private string _boundary;
        private string _rawFileName;
        public MascotReader(string argFileName)
        {
            _fileName = argFileName;
        }

        private Dictionary<int, List<PeptideQuery>> dictPeptideQuery;
        private Dictionary<int, Query> dictQuery;
        private Dictionary<int, float> dictExperimentMZ;
        public Dictionary<int, List<PeptideQuery>> PeptideQueries
        {
            get { return dictPeptideQuery; }
        }

        public Dictionary<int, Query> Queries
        {
            get { return dictQuery; }
        }

        public Dictionary<int, float> ExperimentMZ
        {
            get { return dictExperimentMZ; }
        }
        public string RawFileName
        {
            get { return _rawFileName; }
        }
        public void Read()
        {
            StreamReader sr = new StreamReader(_fileName);
            sr.ReadLine();//MIME-Version: 1.0 (Generated by Mascot version 1.0);
            string tmpStr = sr.ReadLine(); //Content-Type: multipart/mixed; boundary=gc0p4Jq0M2Yt08jU534c0p
            _boundary = tmpStr.Substring(tmpStr.IndexOf("boundary=") + 9);
            List<string> sb = new List<string>();
            DATType Dattype = DATType.unknown;
            do
            {
                tmpStr = sr.ReadLine();
                if (tmpStr.StartsWith("FILE="))
                {
                    _rawFileName = tmpStr.Substring(tmpStr.LastIndexOf('\\') + 1);
                }
                if (tmpStr.Contains("Content-Type") && (tmpStr.Contains("name=\"peptides\"") || tmpStr.Contains("name=\"query") || tmpStr.Contains("name=\"summary")))
                {
                    if (tmpStr.Contains("peptides"))
                    {
                        Dattype = DATType.peptides;
                    }
                    else if (tmpStr.Contains("query"))
                    {
                        Dattype = DATType.query;
                    }
                    else if (tmpStr.Contains("summary"))
                    {
                        Dattype = DATType.summary;
                    }
                    sb.Clear();
                    sb.Add(tmpStr);
                    do
                    {
                        tmpStr = sr.ReadLine();
                        if (tmpStr.Contains(_boundary))
                        {
                            //Processing
                            ParseCSV(Dattype, sb);
                            break;
                        }
                        else
                        {
                            sb.Add(tmpStr);
                        }
                    } while (true);
                }

            } while (!sr.EndOfStream);
            sr.Close();
        }

        private void ParseCSV(DATType argDATType, List<string> argStrBlock)
        {
            if (argDATType == DATType.peptides && dictPeptideQuery == null)
            {
                dictPeptideQuery = new Dictionary<int, List<PeptideQuery>>();
            }
            if (argDATType == DATType.query && dictQuery == null)
            {
                dictQuery = new Dictionary<int, Query>();
            }
            if (argDATType == DATType.summary && dictExperimentMZ == null)
            {
                dictExperimentMZ = new Dictionary<int, float>();
            }
            int LineNum = 2;
            switch (argDATType)
            {
                #region Case:Peptide
                case DATType.peptides:
                    int PreviousQueryNum = 0;
                    int PreviousPeptideNum = 0;
                    int QueryNum = 0;
                    int PeptideNum = 0;
                    PeptideQuery pepQuery = null;
                    do
                    {
                        if (argStrBlock[LineNum] != "")
                        {
                            //Get Query number and peptide number;
                            QueryNum = Convert.ToInt32(argStrBlock[LineNum].Split('=')[0].Split('_')[0].Substring(1));
                            PeptideNum = Convert.ToInt32(argStrBlock[LineNum].Split('=')[0].Split('_')[1].Substring(1));
                        }

                        if (PreviousQueryNum == QueryNum && PreviousPeptideNum != PeptideNum)
                        {
                            if (pepQuery != null)
                            {
                                dictPeptideQuery[QueryNum].Add(pepQuery);
                                PreviousPeptideNum = PeptideNum;
                            }
                            //New Peptide
                            string[] tmpAry = argStrBlock[LineNum].Split('=')[1].Split(',');
                            string protein = "";
                            if (argStrBlock[LineNum].Split(';').Length >= 2)
                            {
                                protein = argStrBlock[LineNum].Split(';')[1].Replace(',',';');
                            }
                            pepQuery = new PeptideQuery(QueryNum,PeptideNum,
                                Convert.ToInt32(tmpAry[0]),
                                Convert.ToSingle(tmpAry[1]),
                                Convert.ToSingle(tmpAry[2]),
                                Convert.ToInt32(tmpAry[3]),
                                tmpAry[4],
                                tmpAry[6],
                                Convert.ToSingle(tmpAry[7]),
                                protein);
                        }
                        else if (PreviousQueryNum != QueryNum)
                        {
                            //New Query

                            string[] tmpAry = argStrBlock[LineNum].Split('=')[1].Split(',');
                            string protein = "";
                            if (argStrBlock[LineNum].Split(';').Length >= 2)
                            {
                                protein = argStrBlock[LineNum].Split(';')[1].Replace(',', ';');
                            }
                            if (tmpAry.Length > 1)
                            {
                                dictPeptideQuery.Add(QueryNum, new List<PeptideQuery>());
                                pepQuery = new PeptideQuery(QueryNum,PeptideNum,
                                                        Convert.ToInt32(tmpAry[0]),
                                                        Convert.ToSingle(tmpAry[1]),
                                                        Convert.ToSingle(tmpAry[2]),
                                                        Convert.ToInt32(tmpAry[3]),
                                                        tmpAry[4],
                                                        tmpAry[6],
                                                        Convert.ToSingle(tmpAry[7]),
                                                        protein);
                            }

                            PreviousQueryNum = QueryNum;
                            PreviousPeptideNum = PeptideNum;
                        }
                        else if (PreviousQueryNum == QueryNum && PreviousPeptideNum == PeptideNum)
                        {
                            if (argStrBlock[LineNum].Split('=')[0].Contains("terms"))
                            {
                                pepQuery.Term = argStrBlock[LineNum].Split('=')[1];
                            }
                        }
                        else
                        {
                            //Error
                            throw new Exception("Parsing Error in Peptide Section");
                        }

                        LineNum++;
                    } while (LineNum != argStrBlock.Count);
                    dictPeptideQuery[QueryNum].Add(pepQuery);
                    break;
                #endregion
                #region Case: query
                case DATType.query:
                    int QNum =
                        Convert.ToInt32(argStrBlock[0].Substring(argStrBlock[0].IndexOf("query") + 5).TrimEnd('\"'));
                    Query q = new Query(QNum);
                    LineNum = 2;
                    do
                    {
                        string Tag = argStrBlock[LineNum].Split('=')[0];
                        string Data = argStrBlock[LineNum].Split('=')[1];
                        if (Tag.Contains("title"))
                        {
                            q.Title = Data;
                        }
                        else if (Tag.Contains("rtinseconds"))
                        {
                            q.RTinSeconds = Convert.ToInt32(Data);
                        }
                        else if (Tag.Contains("scans"))
                        {
                            q.ScanNum = Convert.ToInt32(Data);
                        }
                        else if (Tag.Contains("charge"))
                        {
                            q.Charge = Convert.ToInt32(Data.TrimEnd('+'));
                        }
                        else if (Tag.Contains("mass_min"))
                        {
                            q.Massmin = Convert.ToSingle(Data);
                        }
                        else if (Tag.Contains("mass_max"))
                        {
                            q.Massmax = Convert.ToSingle(Data);
                        }
                        else if (Tag.Contains("int_min"))
                        {
                            q.Intmin = Convert.ToSingle(Data);
                        }
                        else if (Tag.Contains("int_max"))
                        {
                            q.intmax = Convert.ToSingle(Data);
                        }
                        LineNum++;
                    } while (LineNum != argStrBlock.Count);
                    dictQuery.Add(QNum, q);
                    break;
                #endregion
                #region:Summary
                case DATType.summary:
                    LineNum = 2;
                    do
                    {
                        if (argStrBlock[LineNum].StartsWith("qexp"))
                        {
                            int qNum = Convert.ToInt32(argStrBlock[LineNum].Split('=')[0].Substring(4));
                            float Expmz =Convert.ToSingle(argStrBlock[LineNum].Split('=')[1].Split(',')[0]);
                            dictExperimentMZ.Add(qNum,Expmz);
                        }
                        LineNum++;
                    } while (LineNum != argStrBlock.Count);
                    break;
                #endregion
            }
        }

        enum DATType
        {
            unknown = 0, parameters, masses, quantitation, unimod, enzyme, taxonomy, header, summary, mixture, peptides, proteins, query, index
        }
    }
}
