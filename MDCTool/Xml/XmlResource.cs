/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;
using System.Linq;
using System.Data;
using System.IO;
using System.Xml;

namespace MDCTool.Xml
{
    /// <summary>
    /// Helper class to load resource data from disk.
    /// </summary>
    public class XmlResource
    {
        /**
         * Fields
         */
        private string sourceFile;
        private XmlDataSet set;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the entire data set for this XML resource.
        /// </summary>
        public XmlDataSet DataSet
        {
            get { return set; }
        }

        /// <summary>
        /// Gets the named data table from the XML resource.
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public WrappedDataTable this[string elementName]
        {
            get { return new WrappedDataTable(set.Tables[elementName]); }
        }

        /// <summary>
        /// Returns the XML source file this resource was created from.
        /// </summary>
        public string XmlSourceFile
        {
            get { return sourceFile; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResource"/> class.
        /// </summary>
        public XmlResource()
        {
            this.sourceFile = string.Empty;

            // generate a blank dataset schema
            set = new XmlDataSet();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResource"/> class.
        /// </summary>
        /// <param name="fileName"></param>
        public XmlResource(string fileName)
        {
            // generate a blank dataset schema
            set = new XmlDataSet();

            this.sourceFile = fileName;
            FileStream onDiskData = File.Open(fileName, FileMode.Open, FileAccess.Read);
            if (onDiskData != null)
            {
                if (onDiskData.CanRead)
                {
                    onDiskData.Position = 0;

                    // deserialize data out from the database file
                    set.DataSet.ReadXml(new StreamReader(onDiskData), XmlReadMode.ReadSchema);

                    ParseInclude();
                }
                onDiskData.Close();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlResource"/> class.
        /// </summary>
        /// <param name="set"></param>
        public XmlResource(XmlDataSet set)
        {
            // generate a blank dataset schema
            this.set = new XmlDataSet();

            // clone the dataset
            this.set.Clone(set);
        }

        /// <summary>
        /// Helper to parse the deserialized data for external XML includes. 
        /// </summary>
        private void ParseInclude()
        {
            // check if any of the tables have an include schema
            foreach (DataTable table in set.DataSet.Tables)
            {
                // iterate through columns
                foreach (DataColumn column in table.Columns)
                    if (column.ColumnName.EndsWith(XmlDataSet.XML_SOURCE_INCLUDE))
                    {
                        string dataColumnName = column.ColumnName.Split(new string[] { XmlDataSet.XML_SOURCE_INCLUDE }, StringSplitOptions.RemoveEmptyEntries)[0];

                        // iterate through rows for the table
                        foreach (DataRow row in table.Rows)
                        {
                            string externalXml = row[dataColumnName + XmlDataSet.XML_SOURCE_INCLUDE] as string;
                            XmlResource rsrc = new XmlResource(externalXml);
                            row[dataColumnName] = rsrc.DataSet;
                        }
                    }
            }
        }

        /// <summary>
        /// Create a new table schema on the resource.
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateTable(string tableName)
        {
            set.CreateTable(tableName);
        }

        /// <summary>
        /// Helper to determine if the named table exists in the schema.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool HasTable(string tableName)
        {
            // select table from schema
            if (set != null)
            {
                if (set.Tables.Count > 0)
                {
                    // perform LINQ query to get the table we want
                    var ret = (from DataTable item in set.Tables
                               where item.TableName == tableName
                               select item).ToArray();

                    if (ret != null)
                    {
                        if (ret[0] != null)
                            return true;

                        return false;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="valueName"></param>
        /// <param name="valueType"></param>
        public void Submit(string tableName, string valueName, Type valueType)
        {
            DataTable table = set.Tables[tableName];
            table.Columns.Add(valueName, valueType);
        }

        /// <summary>
        /// Adds a data row entry to the table.
        /// </summary>
        /// <param name="dataRow"></param>
        public void Submit(DataRow dataRow)
        {
            dataRow.Table.Rows.Add(dataRow);
        }

        /// <summary>
        /// Creates a new data row.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataRow NewData(string tableName)
        {
            return set.Tables[tableName].NewRow();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="columnName"></param>
        /// <param name="externalXml"></param>
        public void Include(DataRow data, string columnName, string externalXml)
        {
            set.Include(data, columnName, externalXml);
        }

        /// <summary>
        /// Saves XML data to disk.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="includeExternal"></param>
        public void SaveXml(string fileName, bool includeExternal = false)
        {
            // if we're not including external data, discard those columns to prevent
            // re-saving of external data
            if (!includeExternal)
            {
                // check if any of the tables have an include schema)
                foreach (DataTable table in set.DataSet.Tables)
                {
                    // iterate through columns
                    foreach (DataColumn column in table.Columns)
                        if (column.ColumnName.EndsWith(XmlDataSet.XML_SOURCE_INCLUDE))
                        {
                            string dataColumnName = column.ColumnName.Split(new string[] { XmlDataSet.XML_SOURCE_INCLUDE }, StringSplitOptions.RemoveEmptyEntries)[0];

                            // iterate through rows for the table
                            foreach (DataRow row in table.Rows)
                                row[dataColumnName] = null;
                        }
                }
            }

            // begin writing data
            FileStream onDiskData = File.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
            TextWriter writer = new StreamWriter(onDiskData);

            StringWriter strWriter = new StringWriter();
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true;
            writerSettings.IndentChars = "  ";
            writerSettings.Encoding = System.Text.Encoding.UTF8;

            XmlWriter xmlWriter = XmlWriter.Create(strWriter, writerSettings);

            // write XML header
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteComment(" DO NOT EDIT THIS FILE ");
            xmlWriter.Flush();

            writer.WriteLine(strWriter.ToString());
            xmlWriter.Close();
            strWriter.Close();

            // serialize data set data
            set.DataSet.WriteXml(writer, XmlWriteMode.WriteSchema);

            onDiskData.Flush();
            onDiskData.Close();
        }

        /// <summary>
        /// Saves XML data to another <see cref="XmlResource"/>.
        /// </summary>
        /// <param name="resource"></param>
        public void SaveXml(XmlResource resource)
        {
            set.Merge(resource.DataSet);
        }
    } // public class XmlResource
} // namespace Boilerplate.Framework
