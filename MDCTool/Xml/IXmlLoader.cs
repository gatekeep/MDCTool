/**
 * MDCTool
 * INTERNAL/PROPRIETARY/CONFIDENTIAL. Use is subject to license terms.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 * 
 * Author: Bryan Biedenkapp <gatekeep@jmp.cx>
 */
using System;

namespace MDCTool.Xml
{
    /// <summary>
    /// Interface that all objects that load XML data should derive from.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IXmlLoader<T>
    {
        /**
         * Methods
         */
        /// <summary>
        /// Loads XML data from a file.
        /// </summary>
        /// <param name="fileName"></param>
        T LoadXml(string fileName);

        /// <summary>
        /// Loads XML data from a <see cref="XmlResource"/>.
        /// </summary>
        /// <param name="resource"></param>
        T LoadXml(XmlResource resource);

        /// <summary>
        /// Saves XML data to a file.
        /// </summary>
        /// <param name="fileName"></param>
        void SaveXml(string fileName);

        /// <summary>
        /// Saves XML data to another <see cref="XmlResource"/>.
        /// </summary>
        /// <param name="resource"></param>
        void SaveXml(XmlResource resource);
    } // public interface IXmlLoader
} // namespace Boilerplate.Framework.Xml
