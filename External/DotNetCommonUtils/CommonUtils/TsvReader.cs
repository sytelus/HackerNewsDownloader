using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public sealed class TsvReader : IDisposable
    {
        private Dictionary<string, int> columnIndexToNameMap;
        private StreamReader tsvFileStream;
        private string[] currentColumnValues;
        public TsvReader(bool hasHeaderRow, string[] columnNamesOrder, string tsvFilePath)
        {
            tsvFileStream = File.OpenText(tsvFilePath);

            if (hasHeaderRow)
            {
                var headerLine = tsvFileStream.ReadLine();
                if (headerLine == null)
                    throw new Exception("Header line was expected but not found in file {0}".FormatEx(tsvFilePath));

                if (columnNamesOrder == null)
                    columnNamesOrder = headerLine.Split(Utils.TabDelimiter);
            }

            this.columnIndexToNameMap = (columnNamesOrder ?? Utils.EmptyStringArray)
                .Select((columnName, index) => new {ColumnName = columnName, Index = index})
                .Where(columnKvp => !string.IsNullOrEmpty(columnKvp.ColumnName))
                .ToDictionary(columnKvp => columnKvp.ColumnName, columnKvp => columnKvp.Index);
        }

        public bool ReadNext()
        {
            var currentRowLine = tsvFileStream.ReadLine();
            if (currentRowLine != null)
            {
                this.currentColumnValues = currentRowLine.Split(Utils.TabDelimiter);
                return true;
            }
            else return false;
        }

        public string this[string columnName]
        {
            get { return currentColumnValues[columnIndexToNameMap[columnName]]; }
        }
        public string this[int columnIndex]
        {
            get { return currentColumnValues[columnIndex]; }
        }
        
        public static DataTable ToDataTable(bool hasHeaderRow, string[] columnNamesOrder, string tsvFilePath)
        {
            DataTable dataTable;
            using (var tsvReader = new TsvReader(hasHeaderRow, columnNamesOrder, tsvFilePath))
            {
                dataTable = tsvReader.ToDataTable();
            }

            return dataTable;
        }

        public DataTable ToDataTable()
        {
            DataTable dataTable = null;

            try
            {
                dataTable = new DataTable();
                foreach (var columnName in columnIndexToNameMap.Keys)
                    dataTable.Columns.Add(columnName, typeof(string));

                while (this.ReadNext())
                {
                    var row = dataTable.NewRow();
                    foreach (var columnNameIndexKvp in columnIndexToNameMap)
                    {
                        row[columnNameIndexKvp.Key] = this[columnNameIndexKvp.Value];
                    }
                    dataTable.Rows.Add(row);
                }
            }
            catch
            {
                if (dataTable != null)
                    dataTable.Dispose();
                throw;
            }
            return dataTable;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (tsvFileStream != null)
                tsvFileStream.Dispose();
        }

        #endregion
    }
}
