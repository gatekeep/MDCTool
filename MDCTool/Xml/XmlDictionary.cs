/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace MDCTool.Xml
{
    /// <summary>
    /// Helper class used to contain the data in an <see cref="XmlDictionary{TKey, TValue}"/>
    /// </summary>
    /// <typeparam name="TKey">Type of the Key</typeparam>
    /// <typeparam name="TValue">Type of the Value</typeparam>
    public class DataItem<TKey, TValue>
    {
        /**
         * Fields
         */
        /// <summary>
        /// 
        /// </summary>
        public TKey Key;
        /// <summary>
        /// 
        /// </summary>
        public TValue Value;

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="DataItem{TKey, TValue}"/> class.
        /// </summary>
        public DataItem()
        {
            /* stub */
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataItem{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public DataItem(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
    } // public class DataItem<TKey, TValue>

    /// <summary>
    /// Represents a collection of keys and values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
    public class XmlDictionary<TKey, TValue> : IXmlSerializable
    {
        /**
         * Fields
         */
        /// <summary>
        /// Gets a collection containing the key-value pair items in the dictionary.
        /// </summary>
        public List<DataItem<TKey, TValue>> Items;

        /**
         * Properties
         */
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set</param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get { return Items.Find(x => (x.Key.Equals(key))).Value; }
            set { Items.Find(x => (x.Key.Equals(key))).Value = value; }
        }

        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="XmlDictionary{TKey, TValue}"/>
        /// </summary>
        public int Count
        {
            get { return Items.Count; }
        }

        /**
         * Operators
         */
        /// <summary>
        /// Converts a <see cref="XmlDictionary{TKey, TValue}"/> to a <see cref="XmlDictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="xmlDict"></param>
        public static implicit operator Dictionary<TKey, TValue>(XmlDictionary<TKey, TValue> xmlDict)
        {
            Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>(xmlDict.Count);

            foreach (DataItem<TKey, TValue> item in xmlDict.Items)
                dict.Add(item.Key, item.Value);

            return dict;
        }

        /// <summary>
        /// Converts a <see cref="Dictionary{TKey, TValue}"/> to a <see cref="XmlDictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="dict"></param>
        public static implicit operator XmlDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            XmlDictionary<TKey, TValue> xmlDict = new XmlDictionary<TKey, TValue>(dict.Count);

            foreach (KeyValuePair<TKey, TValue> kvp in dict)
                xmlDict.Add(kvp.Key, kvp.Value);

            return xmlDict;
        }

        /**
         * Methods
         */
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDictionary{TKey, TValue}"/> class.
        /// </summary>
        public XmlDictionary()
        {
            this.Items = new List<DataItem<TKey, TValue>>(16);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity"></param>
        public XmlDictionary(int capacity)
        {
            this.Items = new List<DataItem<TKey, TValue>>(capacity);
        }

        /// <summary>
        /// Removes all keys and values from the <see cref="XmlDictionary{TKey, TValue}"/>
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value)
        {
            DataItem<TKey, TValue> item = new DataItem<TKey, TValue>(key, value);
            Items.Add(item);
        }

        /// <summary>
        /// Removes the value with the specified key from the <see cref="XmlDictionary{TKey, TValue}"/>
        /// </summary>
        /// <param name="key"></param>
        public void Remove(TKey key)
        {
            DataItem<TKey, TValue> item = Items.Find(x => (x.Key.Equals(key)));
            Items.Remove(item);
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            string xmlString = reader.ReadInnerXml();

            XmlSerializer xs = new XmlSerializer(typeof(List<DataItem<TKey, TValue>>));
            StringReader strReader = new StringReader(xmlString);

            Items = (List<DataItem<TKey, TValue>>)xs.Deserialize(strReader);

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
            XmlWriterSettings xmlWSettings = new XmlWriterSettings();
            xmlWSettings.OmitXmlDeclaration = true;
            xmlWSettings.ConformanceLevel = ConformanceLevel.Auto;
            xmlWSettings.Indent = true;

            XmlWriter xmlWriter = XmlWriter.Create(strWriter, xmlWSettings);
            XmlSerializer serializer = new XmlSerializer(typeof(List<DataItem<TKey, TValue>>));

            serializer.Serialize(xmlWriter, Items, null);

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
    } // public class XmlDictionary<TKey, TValue> : IXmlSerializable
} // namespace Boilerplate.Framework.Xml
