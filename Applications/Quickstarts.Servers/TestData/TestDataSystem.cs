/* ========================================================================
 * Copyright (c) 2005-2019 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using Opc.Ua;
using Opc.Ua.Server;
using System.Linq;

namespace TestData
{
    public interface ITestDataSystemCallback
    {
        void OnDataChange(
            BaseVariableState variable,
            object value,
            StatusCode statusCode,
            DateTime timestamp);

        void OnGenerateValues(BaseVariableState variable);

    }

    public interface ITestDataSystemValuesGenerator
    {
        StatusCode OnGenerateValues(ISystemContext context);
    }

    public class TestDataSystem
    {
        public TestDataSystem(ITestDataSystemCallback callback, NamespaceTable namespaceUris, StringTable serverUris)
        {
            m_callback = callback;
            m_minimumSamplingInterval = Int32.MaxValue;
            m_monitoredNodes = new Dictionary<uint, BaseVariableState>();
            m_samplingNodes = null;
            m_generator = new Opc.Ua.Test.DataGenerator(null);
            m_generator.NamespaceUris = namespaceUris;
            m_generator.ServerUris = serverUris;
            m_historyArchive = new HistoryArchive();
        }

        /// <summary>
        /// The number of nodes being monitored.
        /// </summary>
        public int MonitoredNodeCount
        {
            get
            {
                lock (m_lock)
                {
                    if (m_monitoredNodes == null)
                    {
                        return 0;
                    }

                    return m_monitoredNodes.Count;
                }
            }
        }

        /// <summary>
        /// Gets or sets the current system status.
        /// </summary>
        public StatusCode SystemStatus
        {
            get
            {
                lock (m_lock)
                {
                    return m_systemStatus;
                }
            }

            set
            {
                lock (m_lock)
                {
                    m_systemStatus = value;
                }
            }
        }

        /// <summary>
        /// Creates an archive for the variable.
        /// </summary>
        public void EnableHistoryArchiving(BaseVariableState variable)
        {
            if (variable == null)
            {
                return;
            }

            if (variable.ValueRank == ValueRanks.Scalar)
            {
                m_historyArchive.CreateRecord(variable.NodeId, TypeInfo.GetBuiltInType(variable.DataType));
            }
        }

        /// <summary>
        /// Returns the history file for the variable.
        /// </summary>
        public IHistoryDataSource GetHistoryFile(BaseVariableState variable)
        {
            if (variable == null)
            {
                return null;
            }

            return m_historyArchive.GetHistoryFile(variable.NodeId);
        }

        /// <summary>
        /// Returns a new value for the variable.
        /// </summary>
        public object ReadValue(BaseVariableState variable)
        {
            lock (m_lock)
            {
                switch (variable.NumericId)
                {
                    case TestData.Variables.ScalarValueObjectType_BooleanValue:
                    case TestData.Variables.UserScalarValueObjectType_BooleanValue:
                    {
                        return m_generator.GetRandom<bool>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_SByteValue:
                    case TestData.Variables.UserScalarValueObjectType_SByteValue:
                    {
                        return m_generator.GetRandom<sbyte>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_SByteValue:
                    {
                        return (sbyte)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                    }

                    case TestData.Variables.ScalarValueObjectType_ByteValue:
                    case TestData.Variables.UserScalarValueObjectType_ByteValue:
                    {
                        return m_generator.GetRandom<byte>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_ByteValue:
                    {
                        return (byte)((m_generator.GetRandom<uint>(false) % 201) + 50);
                    }

                    case TestData.Variables.ScalarValueObjectType_Int16Value:
                    case TestData.Variables.UserScalarValueObjectType_Int16Value:
                    {
                        return m_generator.GetRandom<short>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_Int16Value:
                    {
                        return (short)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                    }

                    case TestData.Variables.ScalarValueObjectType_UInt16Value:
                    case TestData.Variables.UserScalarValueObjectType_UInt16Value:
                    {
                        return m_generator.GetRandom<ushort>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_UInt16Value:
                    {
                        return (ushort)((m_generator.GetRandom<uint>(false) % 201) + 50);
                    }

                    case TestData.Variables.ScalarValueObjectType_Int32Value:
                    case TestData.Variables.UserScalarValueObjectType_Int32Value:
                    {
                        return m_generator.GetRandom<int>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_Int32Value:
                    case TestData.Variables.AnalogScalarValueObjectType_IntegerValue:
                    {
                        return (int)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                    }

                    case TestData.Variables.ScalarValueObjectType_UInt32Value:
                    case TestData.Variables.UserScalarValueObjectType_UInt32Value:
                    {
                        return m_generator.GetRandom<uint>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_UInt32Value:
                    case TestData.Variables.AnalogScalarValueObjectType_UIntegerValue:
                    {
                        return (uint)((m_generator.GetRandom<uint>(false) % 201) + 50);
                    }

                    case TestData.Variables.ScalarValueObjectType_Int64Value:
                    case TestData.Variables.UserScalarValueObjectType_Int64Value:
                    {
                        return m_generator.GetRandom<long>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_Int64Value:
                    {
                        return (long)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                    }

                    case TestData.Variables.ScalarValueObjectType_UInt64Value:
                    case TestData.Variables.UserScalarValueObjectType_UInt64Value:
                    {
                        return m_generator.GetRandom<ulong>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_UInt64Value:
                    {
                        return (ulong)((m_generator.GetRandom<uint>(false) % 201) + 50);
                    }

                    case TestData.Variables.ScalarValueObjectType_FloatValue:
                    case TestData.Variables.UserScalarValueObjectType_FloatValue:
                    {
                        return m_generator.GetRandom<float>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_FloatValue:
                    {
                        return (float)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                    }

                    case TestData.Variables.ScalarValueObjectType_DoubleValue:
                    case TestData.Variables.UserScalarValueObjectType_DoubleValue:
                    {
                        return m_generator.GetRandom<double>(false);
                    }

                    case TestData.Variables.AnalogScalarValueObjectType_DoubleValue:
                    case TestData.Variables.AnalogScalarValueObjectType_NumberValue:
                    {
                        return (double)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                    }

                    case TestData.Variables.ScalarValueObjectType_StringValue:
                    case TestData.Variables.UserScalarValueObjectType_StringValue:
                    {
                        return m_generator.GetRandom<string>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_DateTimeValue:
                    case TestData.Variables.UserScalarValueObjectType_DateTimeValue:
                    {
                        return m_generator.GetRandom<DateTime>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_GuidValue:
                    case TestData.Variables.UserScalarValueObjectType_GuidValue:
                    {
                        return m_generator.GetRandom<Guid>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_ByteStringValue:
                    case TestData.Variables.UserScalarValueObjectType_ByteStringValue:
                    {
                        return m_generator.GetRandom<byte[]>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_XmlElementValue:
                    case TestData.Variables.UserScalarValueObjectType_XmlElementValue:
                    {
                        return m_generator.GetRandom<XmlElement>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_NodeIdValue:
                    case TestData.Variables.UserScalarValueObjectType_NodeIdValue:
                    {
                        return m_generator.GetRandom<Opc.Ua.NodeId>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_ExpandedNodeIdValue:
                    case TestData.Variables.UserScalarValueObjectType_ExpandedNodeIdValue:
                    {
                        return m_generator.GetRandom<ExpandedNodeId>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_QualifiedNameValue:
                    case TestData.Variables.UserScalarValueObjectType_QualifiedNameValue:
                    {
                        return m_generator.GetRandom<QualifiedName>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_LocalizedTextValue:
                    case TestData.Variables.UserScalarValueObjectType_LocalizedTextValue:
                    {
                        return m_generator.GetRandom<LocalizedText>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_StatusCodeValue:
                    case TestData.Variables.UserScalarValueObjectType_StatusCodeValue:
                    {
                        return m_generator.GetRandom<StatusCode>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_VariantValue:
                    case TestData.Variables.UserScalarValueObjectType_VariantValue:
                    {
                        return m_generator.GetRandomVariant(false).Value;
                    }

                    case TestData.Variables.ScalarValueObjectType_StructureValue:
                    {
                        return GetRandomStructure();
                    }

                    case TestData.Variables.ScalarValueObjectType_EnumerationValue:
                    {
                        return m_generator.GetRandom<int>(false);
                    }

                    case TestData.Variables.ScalarValueObjectType_NumberValue:
                    {
                        return m_generator.GetRandom(BuiltInType.Number);
                    }

                    case TestData.Variables.ScalarValueObjectType_IntegerValue:
                    {
                        return m_generator.GetRandom(BuiltInType.Integer);
                    }

                    case TestData.Variables.ScalarValueObjectType_UIntegerValue:
                    {
                        return m_generator.GetRandom(BuiltInType.UInteger);
                    }

                    case TestData.Variables.Data_Static_Structure_VectorStructure:
                    case TestData.Variables.Data_Dynamic_Structure_VectorStructure:
                    case TestData.Variables.StructureValueObjectType_VectorStructure:
                    case TestData.Variables.ScalarValueObjectType_VectorValue:
                    {
                        return GetRandomVector();
                    }

                    case TestData.Variables.ArrayValueObjectType_VectorValue:
                    {
                        return GetRandomVectorArray();
                    }

                    case TestData.Variables.ArrayValueObjectType_BooleanValue:
                    case TestData.Variables.UserArrayValueObjectType_BooleanValue:
                    {
                        return m_generator.GetRandomArray<bool>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_SByteValue:
                    case TestData.Variables.UserArrayValueObjectType_SByteValue:
                    {
                        return m_generator.GetRandomArray<sbyte>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_SByteValue:
                    {
                        sbyte[] values = m_generator.GetRandomArray<sbyte>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (sbyte)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_ByteValue:
                    case TestData.Variables.UserArrayValueObjectType_ByteValue:
                    {
                        return m_generator.GetRandomArray<byte>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_ByteValue:
                    {
                        byte[] values = m_generator.GetRandomArray<byte>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (byte)((m_generator.GetRandom<uint>(false) % 201) + 50);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_Int16Value:
                    case TestData.Variables.UserArrayValueObjectType_Int16Value:
                    {
                        return m_generator.GetRandomArray<short>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_Int16Value:
                    {
                        short[] values = m_generator.GetRandomArray<short>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (short)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_UInt16Value:
                    case TestData.Variables.UserArrayValueObjectType_UInt16Value:
                    {
                        return m_generator.GetRandomArray<ushort>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_UInt16Value:
                    {
                        ushort[] values = m_generator.GetRandomArray<ushort>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (ushort)((m_generator.GetRandom<uint>(false) % 201) + 50);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_Int32Value:
                    case TestData.Variables.UserArrayValueObjectType_Int32Value:
                    {
                        return m_generator.GetRandomArray<int>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_Int32Value:
                    case TestData.Variables.AnalogArrayValueObjectType_IntegerValue:
                    {
                        int[] values = m_generator.GetRandomArray<int>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (int)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_UInt32Value:
                    case TestData.Variables.UserArrayValueObjectType_UInt32Value:
                    {
                        return m_generator.GetRandomArray<uint>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_UInt32Value:
                    case TestData.Variables.AnalogArrayValueObjectType_UIntegerValue:
                    {
                        uint[] values = m_generator.GetRandomArray<uint>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (uint)((m_generator.GetRandom<uint>(false) % 201) + 50);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_Int64Value:
                    case TestData.Variables.UserArrayValueObjectType_Int64Value:
                    {
                        return m_generator.GetRandomArray<long>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_Int64Value:
                    {
                        long[] values = m_generator.GetRandomArray<long>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (long)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_UInt64Value:
                    case TestData.Variables.UserArrayValueObjectType_UInt64Value:
                    {
                        return m_generator.GetRandomArray<ulong>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_UInt64Value:
                    {
                        ulong[] values = m_generator.GetRandomArray<ulong>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (ulong)((m_generator.GetRandom<uint>(false) % 201) + 50);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_FloatValue:
                    case TestData.Variables.UserArrayValueObjectType_FloatValue:
                    {
                        return m_generator.GetRandomArray<float>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_FloatValue:
                    {
                        float[] values = m_generator.GetRandomArray<float>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (float)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_DoubleValue:
                    case TestData.Variables.UserArrayValueObjectType_DoubleValue:
                    {
                        return m_generator.GetRandomArray<double>(false, 100, false);
                    }

                    case TestData.Variables.AnalogArrayValueObjectType_DoubleValue:
                    case TestData.Variables.AnalogArrayValueObjectType_NumberValue:
                    {
                        double[] values = m_generator.GetRandomArray<double>(false, 100, false);

                        for (int ii = 0; ii < values.Length; ii++)
                        {
                            values[ii] = (double)(((int)(m_generator.GetRandom<uint>(false) % 201)) - 100);
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_StringValue:
                    case TestData.Variables.UserArrayValueObjectType_StringValue:
                    {
                        return m_generator.GetRandomArray<string>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_DateTimeValue:
                    case TestData.Variables.UserArrayValueObjectType_DateTimeValue:
                    {
                        return m_generator.GetRandomArray<DateTime>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_GuidValue:
                    case TestData.Variables.UserArrayValueObjectType_GuidValue:
                    {
                        return m_generator.GetRandomArray<Guid>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_ByteStringValue:
                    case TestData.Variables.UserArrayValueObjectType_ByteStringValue:
                    {
                        return m_generator.GetRandomArray<byte[]>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_XmlElementValue:
                    case TestData.Variables.UserArrayValueObjectType_XmlElementValue:
                    {
                        return m_generator.GetRandomArray<XmlElement>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_NodeIdValue:
                    case TestData.Variables.UserArrayValueObjectType_NodeIdValue:
                    {
                        return m_generator.GetRandomArray<Opc.Ua.NodeId>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_ExpandedNodeIdValue:
                    case TestData.Variables.UserArrayValueObjectType_ExpandedNodeIdValue:
                    {
                        return m_generator.GetRandomArray<ExpandedNodeId>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_QualifiedNameValue:
                    case TestData.Variables.UserArrayValueObjectType_QualifiedNameValue:
                    {
                        return m_generator.GetRandomArray<QualifiedName>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_LocalizedTextValue:
                    case TestData.Variables.UserArrayValueObjectType_LocalizedTextValue:
                    {
                        return m_generator.GetRandomArray<LocalizedText>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_StatusCodeValue:
                    case TestData.Variables.UserArrayValueObjectType_StatusCodeValue:
                    {
                        return m_generator.GetRandomArray<StatusCode>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_VariantValue:
                    case TestData.Variables.UserArrayValueObjectType_VariantValue:
                    {
                        return m_generator.GetRandomArray<object>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_StructureValue:
                    {
                        ExtensionObject[] values = m_generator.GetRandomArray<ExtensionObject>(false, 10, false);

                        for (int ii = 0; values != null && ii < values.Length; ii++)
                        {
                            values[ii] = GetRandomStructure();
                        }

                        return values;
                    }

                    case TestData.Variables.ArrayValueObjectType_EnumerationValue:
                    {
                        return m_generator.GetRandomArray<int>(false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_NumberValue:
                    {
                        return m_generator.GetRandomArray(BuiltInType.Number, false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_IntegerValue:
                    {
                        return m_generator.GetRandomArray(BuiltInType.Integer, false, 100, false);
                    }

                    case TestData.Variables.ArrayValueObjectType_UIntegerValue:
                    {
                        return m_generator.GetRandomArray(BuiltInType.UInteger, false, 100, false);
                    }

                    case TestData.Variables.Data_Static_Structure_ScalarStructure:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure:
                    case TestData.Variables.StructureValueObjectType_ScalarStructure:
                        return GetRandomScalarStructureDataType();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_BooleanValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_BooleanValue:
                        return m_generator.GetRandomBoolean();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_SByteValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_SByteValue:
                        return m_generator.GetRandomSByte();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_ByteValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_ByteValue:
                        return m_generator.GetRandomByte();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_Int16Value:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_Int16Value:
                        return m_generator.GetRandomInt16();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_UInt16Value:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_UInt16Value:
                        return m_generator.GetRandomUInt16();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_Int32Value:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_Int32Value:
                        return m_generator.GetRandomInt32();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_UInt32Value:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_UInt32Value:
                        return m_generator.GetRandomUInt32();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_Int64Value:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_Int64Value:
                        return m_generator.GetRandomInt64();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_UInt64Value:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_UInt64Value:
                        return m_generator.GetRandomUInt64();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_FloatValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_FloatValue:
                        return m_generator.GetRandomFloat();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_DoubleValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_DoubleValue:
                        return m_generator.GetRandomDouble();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_StringValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_StringValue:
                        return m_generator.GetRandomString();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_DateTimeValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_DateTimeValue:
                        return m_generator.GetRandomDateTime();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_GuidValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_GuidValue:
                        return m_generator.GetRandomGuid();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_ByteStringValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_ByteStringValue:
                        return m_generator.GetRandomByteString();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_XmlElementValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_XmlElementValue:
                        return m_generator.GetRandomXmlElement();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_NodeIdValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_NodeIdValue:
                        return m_generator.GetRandomNodeId();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_ExpandedNodeIdValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_ExpandedNodeIdValue:
                        return m_generator.GetRandomExpandedNodeId();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_QualifiedNameValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_QualifiedNameValue:
                        return m_generator.GetRandomQualifiedName();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_LocalizedTextValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_LocalizedTextValue:
                        return m_generator.GetRandomLocalizedText();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_StatusCodeValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_StatusCodeValue:
                        return m_generator.GetRandomStatusCode();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_VariantValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_VariantValue:
                        return m_generator.GetRandomVariant();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_EnumerationValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_EnumerationValue:
                        return m_generator.GetRandomByte();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_StructureValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_StructureValue:
                        return GetRandomStructure();

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_NumberValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_NumberValue:
                        return new Variant(m_generator.GetRandomNumber());

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_IntegerValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_IntegerValue:
                        return new Variant(m_generator.GetRandomInteger());

                    case TestData.Variables.Data_Static_Structure_ScalarStructure_UIntegerValue:
                    case TestData.Variables.Data_Dynamic_Structure_ScalarStructure_UIntegerValue:
                        return new Variant(m_generator.GetRandomUInteger());
                }

                return null;
            }
        }

        /// <summary>
        /// Return random vector;
        /// </summary>
        public Vector GetRandomVector()
        {
            return new Vector() {
                X = (double)m_generator.GetRandom(BuiltInType.Double),
                Y = (double)m_generator.GetRandom(BuiltInType.Double),
                Z = (double)m_generator.GetRandom(BuiltInType.Double),
            };
        }

        private Vector[] GetRandomVectorArray()
        {
            int size = m_generator.GetRandomByte() % 8 + 1;
            Vector[] result = new Vector[size];
            for (int ii = 0; ii < size; ii++)
            {
                result[ii] = GetRandomVector();
            }
            return result;
        }

        /// <summary>
        /// Returns a random structure.
        /// </summary>
        private ExtensionObject GetRandomStructure()
        {
            if (m_generator.GetRandomBoolean())
            {
                ScalarStructureDataType value = GetRandomScalarStructureDataType();
                return new ExtensionObject(value.TypeId, value);
            }
            else
            {
                ArrayValueDataType value = GetRandomArrayValueDataType();
                return new ExtensionObject(value.TypeId, value);
            }
        }

        public ScalarStructureDataType GetRandomScalarStructureDataType()
        {
            ScalarStructureDataType value = new ScalarStructureDataType {
                BooleanValue = m_generator.GetRandom<bool>(false),
                SByteValue = m_generator.GetRandom<sbyte>(false),
                ByteValue = m_generator.GetRandom<byte>(false),
                Int16Value = m_generator.GetRandom<short>(false),
                UInt16Value = m_generator.GetRandom<ushort>(false),
                Int32Value = m_generator.GetRandom<int>(false),
                UInt32Value = m_generator.GetRandom<uint>(false),
                Int64Value = m_generator.GetRandom<long>(false),
                UInt64Value = m_generator.GetRandom<ulong>(false),
                FloatValue = m_generator.GetRandom<float>(false),
                DoubleValue = m_generator.GetRandom<double>(false),
                StringValue = m_generator.GetRandom<string>(false),
                DateTimeValue = m_generator.GetRandom<DateTime>(false),
                GuidValue = m_generator.GetRandom<Uuid>(false),
                ByteStringValue = m_generator.GetRandom<byte[]>(false),
                XmlElementValue = m_generator.GetRandom<XmlElement>(false),
                NodeIdValue = m_generator.GetRandom<Opc.Ua.NodeId>(false),
                ExpandedNodeIdValue = m_generator.GetRandom<ExpandedNodeId>(false),
                QualifiedNameValue = m_generator.GetRandom<QualifiedName>(false),
                LocalizedTextValue = m_generator.GetRandom<LocalizedText>(false),
                StatusCodeValue = m_generator.GetRandom<StatusCode>(false),
                VariantValue = m_generator.GetRandomVariant(false),
                IntegerValue = new Variant(m_generator.GetRandomInteger()),
                UIntegerValue = new Variant(m_generator.GetRandomUInteger()),
                NumberValue = new Variant(m_generator.GetRandomNumber())
            };

            return value;
        }

        public ArrayValueDataType GetRandomArrayValueDataType()
        {
            ArrayValueDataType value = new ArrayValueDataType {
                BooleanValue = m_generator.GetRandomArray<bool>(false, 10, false),
                SByteValue = m_generator.GetRandomArray<sbyte>(false, 10, false),
                ByteValue = m_generator.GetRandomArray<byte>(false, 10, false),
                Int16Value = m_generator.GetRandomArray<short>(false, 10, false),
                UInt16Value = m_generator.GetRandomArray<ushort>(false, 10, false),
                Int32Value = m_generator.GetRandomArray<int>(false, 10, false),
                UInt32Value = m_generator.GetRandomArray<uint>(false, 10, false),
                Int64Value = m_generator.GetRandomArray<long>(false, 10, false),
                UInt64Value = m_generator.GetRandomArray<ulong>(false, 10, false),
                FloatValue = m_generator.GetRandomArray<float>(false, 10, false),
                DoubleValue = m_generator.GetRandomArray<double>(false, 10, false),
                StringValue = m_generator.GetRandomArray<string>(false, 10, false),
                DateTimeValue = m_generator.GetRandomArray<DateTime>(false, 10, false),
                GuidValue = m_generator.GetRandomArray<Uuid>(false, 10, false),
                ByteStringValue = m_generator.GetRandomArray<byte[]>(false, 10, false),
                XmlElementValue = m_generator.GetRandomArray<XmlElement>(false, 10, false),
                NodeIdValue = m_generator.GetRandomArray<Opc.Ua.NodeId>(false, 10, false),
                ExpandedNodeIdValue = m_generator.GetRandomArray<ExpandedNodeId>(false, 10, false),
                QualifiedNameValue = m_generator.GetRandomArray<QualifiedName>(false, 10, false),
                LocalizedTextValue = m_generator.GetRandomArray<LocalizedText>(false, 10, false),
                StatusCodeValue = m_generator.GetRandomArray<StatusCode>(false, 10, false),
            };

            object[] values = m_generator.GetRandomArray<object>(false, 10, false);

            for (int ii = 0; values != null && ii < values.Length; ii++)
            {
                value.VariantValue.Add(new Variant(values[ii]));
            }

            return value;
        }

        public void StartMonitoringValue(uint monitoredItemId, double samplingInterval, BaseVariableState variable)
        {
            lock (m_lock)
            {
                if (m_monitoredNodes == null)
                {
                    m_monitoredNodes = new Dictionary<uint, BaseVariableState>();
                }

                m_monitoredNodes[monitoredItemId] = variable;
                m_samplingNodes = null;

                SetSamplingInterval(samplingInterval);
            }
        }

        public void SetSamplingInterval(double samplingInterval)
        {
            lock (m_lock)
            {
                if (samplingInterval < 0)
                {
                    // m_samplingEvent.Set();
                    m_minimumSamplingInterval = Int32.MaxValue;

                    if (m_timer != null)
                    {
                        m_timer.Dispose();
                        m_timer = null;
                    }

                    return;
                }

                if (m_minimumSamplingInterval > samplingInterval)
                {
                    m_minimumSamplingInterval = (int)samplingInterval;

                    if (m_minimumSamplingInterval < 100)
                    {
                        m_minimumSamplingInterval = 100;
                    }

                    if (m_timer != null)
                    {
                        m_timer.Dispose();
                        m_timer = null;
                    }

                    m_timer = new Timer(DoSample, null, m_minimumSamplingInterval, m_minimumSamplingInterval);
                }
            }
        }

        void DoSample(object state)
        {
            Utils.LogTrace("DoSample HiRes={0:ss.ffff} Now={1:ss.ffff}", HiResClock.UtcNow, DateTime.UtcNow);

            var samples = new Queue<Sample>();
            var generateValues = new List<BaseVariableState>();

            lock (m_lock)
            {
                if (m_monitoredNodes == null)
                {
                    return;
                }

                if (m_samplingNodes == null)
                {
                    m_samplingNodes = m_monitoredNodes.Values.Distinct(new NodeStateComparer()).Cast<BaseVariableState>().ToList();
                }

                foreach (BaseVariableState variable in m_samplingNodes)
                {
                    if (variable is ITestDataSystemValuesGenerator)
                    {
                        generateValues.Add(variable);
                    }
                    else if (variable.Parent is ITestDataSystemValuesGenerator)
                    {
                        generateValues.Add(variable.Parent as BaseVariableState);
                    }
                    else
                    {
                        Sample sample = new Sample();

                        sample.Variable = variable;
                        sample.Value = ReadValue(sample.Variable);
                        sample.StatusCode = StatusCodes.Good;
                        sample.Timestamp = DateTime.UtcNow;

                        samples.Enqueue(sample);
                    }
                }
            }

            while (samples.Count > 0)
            {
                Sample sample = samples.Dequeue();

                m_callback.OnDataChange(
                    sample.Variable,
                    sample.Value,
                    sample.StatusCode,
                    sample.Timestamp);
            }

            foreach (var generateValue in generateValues)
            {
                m_callback.OnGenerateValues(generateValue);
            }
        }

        public void StopMonitoringValue(uint monitoredItemId)
        {
            lock (m_lock)
            {
                if (m_monitoredNodes == null)
                {
                    return;
                }

                m_monitoredNodes.Remove(monitoredItemId);
                m_samplingNodes = null;

                if (m_monitoredNodes.Count == 0)
                {
                    SetSamplingInterval(-1);
                }
            }
        }

        private class Sample
        {
            public BaseVariableState Variable;
            public object Value;
            public StatusCode StatusCode;
            public DateTime Timestamp;
        }

        #region Private Fields
        private object m_lock = new object();
        private ITestDataSystemCallback m_callback;
        private Opc.Ua.Test.DataGenerator m_generator;
        private int m_minimumSamplingInterval;
        private Dictionary<uint, BaseVariableState> m_monitoredNodes;
        private IList<BaseVariableState> m_samplingNodes;
        private Timer m_timer;
        private StatusCode m_systemStatus;
        private HistoryArchive m_historyArchive;
        #endregion
    }
}
