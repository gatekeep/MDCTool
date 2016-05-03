/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace MDCTool.Xml
{
    /// <summary>
    /// Implements a container that holds data from an XML document.
    /// </summary>
    public class XmlDataSet : IXmlSerializable
    {
        /**
         * Fields
         */
        public const string DEFAULT_NAMESPACE = "Resource";
        public const string XML_SOURCE_INCLUDE = "XmlSource";

        private DataSet set;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the entire data set for this <see cref="XmlDataSet"/>.
        /// </summary>
        public DataSet DataSet
        {
            get { return set; }
        }

        /// <summary>
        /// Gets the tables contained in this <see cref="XmlDataSet"/>.
        /// </summary>
        public DataTableCollection Tables
        {
            get { return set.Tables; }
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

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDataSet"/> class.
        /// </summary>
        public XmlDataSet()
        {
            // generate a new dataset
            set = new DataSet();
            set.Namespace = DEFAULT_NAMESPACE;
            set.DataSetName = DEFAULT_NAMESPACE;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            string xmlString = reader.ReadInnerXml();
            StringReader strReader = new StringReader(xmlString);

            set.ReadXml(strReader);

            strReader.Close();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            // write the xml out to a raw string
            StringWriter strWriter = new StringWriter();
            set.WriteXml(strWriter, XmlWriteMode.WriteSchema);

            // we need to read it back in so we can inject it as nodes
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            XmlReader reader = XmlReader.Create(new StringReader(strWriter.ToString()), settings);

            writer.WriteNode(reader, true);
            strWriter.Close();
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the <see cref="IXmlSerializable"/> interface,
        /// you should return null from this method, and instead, if specifying a custom schema is required, apply
        /// the <see cref="XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return (null);
        }

        /// <summary>
        /// Create a new table schema on the resource.
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateTable(string tableName)
        {
            DataTable table = new DataTable(tableName);
            set.Tables.Add(table);
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
        /// Renames a table in the data set.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="newName"></param>
        public void RenameTable(string tableName, string newName)
        {
            // clone existing table
            DataTable table = set.Tables[tableName].Clone();
            table.TableName = newName;

            // remove old table and readd renamed table
            set.Tables.Remove(tableName);
            set.Tables.Add(table);
        }

        /// <summary>
        /// Adds a column to the table.
        /// </summary>
        /// <param name="elementName"></param>
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
        /// <param name="elementName"></param>
        /// <returns></returns>
        public DataRow NewData(string tableName)
        {
            return set.Tables[tableName].NewRow();
        }

        /// <summary>
        /// Adds a data column as an external XML source.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="columnName"></param>
        /// <param name="externalXml"></param>
        public void Include(DataRow data, string columnName, string externalXml)
        {
            DataTable table = data.Table;
            if (!table.Columns.Contains(columnName))
                table.Columns.Add(columnName, typeof(XmlDataSet));
            if (!table.Columns.Contains(columnName + XML_SOURCE_INCLUDE))
                table.Columns.Add(columnName + XML_SOURCE_INCLUDE, typeof(string));
            data[columnName] = null;
            data[columnName + XML_SOURCE_INCLUDE] = externalXml;
        }

        /// <summary>
        /// Clones (both schema and data) the given <see cref="XmlDataSet"/> into this set.
        /// </summary>
        /// <param name="set"></param>
        public void Clone(XmlDataSet set)
        {
            this.set = set.DataSet.Clone();
            this.set.Merge(set.DataSet);
        }

        /// <summary>
        /// Merge a separate <see cref="XmlDataSet"/> into this <see cref="XmlDataSet"/>.
        /// </summary>
        /// <param name="set"></param>
        public void Merge(XmlDataSet set)
        {
            this.set.Merge(set.DataSet);
        }
    } // public class XmlDataSet : IXmlSerializable
} // namespace Boilerplate.Framework.Xml
