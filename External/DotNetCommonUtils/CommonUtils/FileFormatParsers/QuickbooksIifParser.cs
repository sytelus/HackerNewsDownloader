using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils.FileFormatParsers
{
    public class QuickbooksIifParser : IDisposable
    {
        DataSet iifSet;
        public DataSet Parse(string filePath)
        {
            iifSet = new DataSet();
            var fileText = File.ReadAllText(filePath);
            
            //IIF allows line breaks in column values
            fileText.Replace("\r\n", "\n");
            fileText.Replace('\r', '\n');

            //split along tabs
            string[] lines = fileText.Split('\n');

            this.CreateTables(lines, iifSet);
            this.FillSet(lines, iifSet);

            return iifSet;
        }

        /// <summary>
        /// Reads an array of lines and parses them into tables for the dataset
        /// </summary>
        /// <param name="lines">String Array of lines from the iif file</param>
        /// <param name="iifSet">DataSet to be manipulated</param>
        private void FillSet(string[] lines, DataSet set)
        {
            //CODING HORROR
            //WARNING: I will monkey with the for loop index, be prepared!
            for (int i = 0; i < lines.Length; i++)
            {
                if (this.IsTableHeader(lines[i]))
                {
                    //ignore this line, tables are alread defined
                    continue;
                }
                if (lines[i] == "" || lines[i] == "\r" || lines[i] == "\n\r" || lines[i] == "\n")
                {
                    //ignore lines that are empty if not inside a record
                    //probably the end of the file, it always ends with a blank line break
                    continue;
                }

                if (lines[i].IndexOf(";__IMPORTED__") != -1)
                {
                    continue;
                    //just signifying that it's been imported by quickbook's timer before, don't need it
                }

                //IIF allows linebreaks in some of the fields
                string line = lines[i];
                while (!IsFullLine(line, set))
                {
                    i++;            //<--------------------------- MONKEYING done here!
                    line += lines[i];
                }
                //now, the line should be complete, we can parse it by tabs now
                this.ParseRecord(line, set);
            }
        }

        private void ParseRecord(string line, DataSet set)
        {
            if (IsTableHeader(line))
            {
                //we don't want to deal with headers here
                return;
            }

            String tablename = line.Split('\t')[0];
            //this just removes the first value and the line break from the last value
            String[] parameters = this.CreateDataRowParams(line, set.Tables[tablename].Columns.Count);

            if (parameters.Length > 0)  //add it to the dataset
                set.Tables[tablename].Rows.Add(parameters);
        }

        private bool IsFullLine(string line, DataSet set)
        {
            if (IsTableHeader(line))
            {
                return true;    //assumes table headers won't have line breaks
            }
            int values = line.Split('\t').Length;
            string tableName = line.Split('\t')[0];
            int columns = set.Tables[tableName].Columns.Count;

            if (values < columns)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CreateTables(string[] lines, DataSet set)
        {
            for (int index = 0; index < lines.Length; index++)
            {
                if (this.IsTableHeader(lines[index]))
                {
                    set.Tables.Add(CreateTable(lines[index]));
                }
            }
        }

        private bool IsTableHeader(string tab)
        {
            if (tab.StartsWith("!"))
                return true;
            else
                return false;
        }

        private bool IsNewLine(string p)
        {
            if (p.StartsWith("!"))
                return true;
            if (iifSet.Tables[p.Split('\t')[0]] != null)    //that little mess there grabs the first record in the line, sorry about the mess
                return true;
            return false;
        }

        private DataTable CreateTable(string line)
        {
            String[] values = line.Split('\t');

            //first value is always the title
            //remove the ! from it
            values[0] = values[0].Substring(1);     //remove the first character
            DataTable dt = new DataTable(values[0]);
            values[0] = null;   //hide first title so it doesn't get used, cheaper than resetting array
            foreach (String name in values)
            {
                if (name == null || name == "")
                    continue;
                DataColumn dc = new DataColumn(name, typeof(String));
                try
                {
                    dt.Columns.Add(dc);
                }
                catch (DuplicateNameException)
                {
                    //odd
                    dc = new DataColumn(name + "_duplicateCol" + dt.Columns.Count.ToString());
                    dt.Columns.Add(dc);
                    //if there is a triple, just throw it
                }
            }

            return dt;
        }

        private string GetTableName(string line)
        {
            String[] values = line.Split('\t');

            //first value is always the title
            if (values[0].StartsWith("!"))
            {
                //remove the ! from it
                values[0] = values[0].Substring(1);     //remove the first character
            }
            return values[0];
        }

        private string[] CreateDataRowParams(string line, int maxLength)
        {
            string[] raw = line.Split('\t');

            int length = raw.Length - 1;
            if (length == 0 || maxLength == 0)
                return Utils.EmptyStringArray;

            if (length > maxLength)
                length = maxLength;

            string[] values = new string[length];
            for (int i = 0; i < length; i++)
            {
                values[i] = raw[i + 1];
            }

            if (values[values.Length - 1].EndsWith("\n"))
            {
                values[values.Length - 1] = values[values.Length - 1].Substring(0, values[values.Length - 1].LastIndexOf('\n'));
            }
            else if (values[values.Length - 1].EndsWith("\n\r"))
            {
                values[values.Length - 1] = values[values.Length - 1].Substring(0, values[values.Length - 1].LastIndexOf("\n\r"));
            }
            else if (values[values.Length - 1].EndsWith("\r"))
            {
                values[values.Length - 1] = values[values.Length - 1].Substring(0, values[values.Length - 1].LastIndexOf('\r'));
            }

            return values;
        }

        protected virtual void Dispose(bool cleanAll)
        {
            if (this.iifSet != null)
            {
                this.iifSet.Dispose();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
