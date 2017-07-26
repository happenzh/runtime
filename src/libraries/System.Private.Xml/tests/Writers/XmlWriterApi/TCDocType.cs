﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using OLEDB.Test.ModuleCore;
using XmlCoreTest.Common;
using Xunit;

namespace System.Xml.Tests
{
    //[TestCase(Name = "WriteDocType")]
    public class TCDocType
    {
        //[Variation(id = 1, Desc = "Sanity test", Pri = 1)]
        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter)]
        public void docType_1(XmlWriterUtils utils)
        {
            using (XmlWriter w = utils.CreateWriter())
            {
                w.WriteDocType("ROOT", "publicid", "sysid", "<!ENTITY e 'abc'>");
                w.WriteStartElement("ROOT");
                w.WriteEndElement();
            }

            string exp = utils.IsIndent() ?
                "<!DOCTYPE ROOT PUBLIC \"publicid\" \"sysid\"[<!ENTITY e 'abc'>]>" + Environment.NewLine + "<ROOT />" :
                "<!DOCTYPE ROOT PUBLIC \"publicid\" \"sysid\"[<!ENTITY e 'abc'>]><ROOT />";
            Assert.True(utils.CompareString(exp));
        }

        //[Variation(id = 2, Desc = "WriteDocType pubid = null and sysid = null", Pri = 1)]
        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter)]
        public void docType_2(XmlWriterUtils utils)
        {
            using (XmlWriter w = utils.CreateWriter())
            {
                w.WriteDocType("test", null, null, "<!ENTITY e 'abc'>");
                w.WriteStartElement("Root");
                w.WriteEndElement();
            }
            string exp = utils.IsIndent() ?
                "<!DOCTYPE test [<!ENTITY e 'abc'>]>" + Environment.NewLine + "<Root />" :
                "<!DOCTYPE test [<!ENTITY e 'abc'>]><Root />";
            Assert.True(utils.CompareString(exp));
        }

        //[Variation(id = 3, Desc = "Call WriteDocType twice", Pri = 1)]
        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter)]
        public void docType_3(XmlWriterUtils utils)
        {
            using (XmlWriter w = utils.CreateWriter())
            {
                try
                {
                    w.WriteDocType("doc1", null, null, "<!ENTITY e 'abc'>");
                    w.WriteDocType("doc2", null, null, "<!ENTITY f 'abc'>");
                }
                catch (InvalidOperationException e)
                {
                    CError.WriteLineIgnore(e.ToString());
                    CError.Compare(w.WriteState, WriteState.Error, "WriteState should be Error");
                    return;
                }
            }
            CError.WriteLine("Did not throw exception");
            Assert.True(false);
        }

        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter, "String.Empty")]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter, "null")]
        public void docType_4(XmlWriterUtils utils, string param)
        {
            String docName = "";
            if (param == "String.Empty")
                docName = String.Empty;
            else if (param == "null")
                docName = null;
            using (XmlWriter w = utils.CreateWriter())
            {
                try
                {
                    w.WriteDocType(docName, null, null, "test1");
                }
                catch (ArgumentException e)
                {
                    CError.WriteLineIgnore(e.ToString());
                    CError.Compare(w.WriteState, (utils.WriterType == WriterType.CharCheckingWriter) ? WriteState.Start : WriteState.Error, "WriteState should be Error");
                    return;
                }
                catch (NullReferenceException e)
                {
                    CError.WriteLineIgnore(e.ToString());
                    CError.Compare(w.WriteState, (utils.WriterType == WriterType.CharCheckingWriter) ? WriteState.Start : WriteState.Error, "WriteState should be Error");
                    return;
                }
            }
            CError.WriteLine("Did not throw exception");
            Assert.True(false);
        }

        //[Variation(id = 6, Desc = "WriteDocType with DocType end tag in the value", Pri = 1)]
        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter)]
        public void docType_5(XmlWriterUtils utils)
        {
            using (XmlWriter w = utils.CreateWriter())
            {
                String docName = "Root";
                String docValue = "]>";
                w.WriteDocType(docName, null, null, docValue);
                w.WriteStartElement("Root");
                w.WriteEndElement();
            }
            string exp = utils.IsIndent() ? "<!DOCTYPE Root []>]>" + Environment.NewLine + "<Root />" : "<!DOCTYPE Root []>]><Root />";
            Assert.True(utils.CompareString(exp));
        }

        //[Variation(id = 7, Desc = "Call WriteDocType in the root element", Pri = 1)]
        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter)]
        public void docType_6(XmlWriterUtils utils)
        {
            using (XmlWriter w = utils.CreateWriter())
            {
                try
                {
                    w.WriteStartElement("Root");
                    w.WriteDocType("doc1", null, null, "test1");
                    w.WriteEndElement();
                }
                catch (InvalidOperationException e)
                {
                    CError.WriteLineIgnore(e.ToString());
                    CError.Compare(w.WriteState, WriteState.Error, "WriteState should be Error");
                    return;
                }
            }
            CError.WriteLine("Did not throw exception");
            Assert.True(false);
        }

        //[Variation(id = 8, Desc = "Call WriteDocType following root element", Pri = 1)]
        [Theory]
        [XmlWriterInlineData(TestCaseUtilsImplementation.XmlFactoryWriter)]
        public void docType_7(XmlWriterUtils utils)
        {
            using (XmlWriter w = utils.CreateWriter())
            {
                try
                {
                    w.WriteStartElement("Root");
                    w.WriteEndElement();
                    w.WriteDocType("doc1", null, null, "test1");
                }
                catch (InvalidOperationException e)
                {
                    CError.WriteLineIgnore(e.ToString());
                    CError.Compare(w.WriteState, WriteState.Error, "WriteState should be Error");
                    return;
                }
            }
            CError.WriteLine("Did not throw exception");
            Assert.True(false);
        }
    }
}
