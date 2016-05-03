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

namespace MDCTool.Xml
{
    /// <summary>
    /// Helper that wraps the <see cref="DataTable"/> class.
    /// </summary>
    public class WrappedDataTable
    {
        /**
         * Fields
         */
        private DataTable table;

        /**
         * Properties
         */
        /// <summary>
        /// Gets the contained table.
        /// </summary>
        public DataTable Table
        {
            get { return table; }
        }

        /// <summary>
        /// Gets the named column in the first data entry in the wrapped data table.
        /// </summary>
        public object this[string columnName]
        {
            get { return table.Rows[0][columnName]; }
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappedDataTable"/> class.
        /// </summary>
        /// <param name="table"></param>
        public WrappedDataTable(DataTable table)
        {
            this.table = table;
        }

        /// <summary>
        /// Helper to determine if the named column exists in the schema.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool HasColumn(string columnName)
        {
            // select table from schema
            if (table != null)
            {
                if (table.Columns.Count > 0)
                {
                    // perform LINQ query to get the table we want
                    var ret = (from DataColumn item in table.Columns
                               where item.ColumnName == columnName
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
    } // public class WrappedDataTable
} // namespace Boilerplate.Framework.Xml
