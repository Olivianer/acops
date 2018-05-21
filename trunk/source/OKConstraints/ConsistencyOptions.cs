using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Extensions.Logging;

namespace OKConstraints
{
    /// <summary>
    /// Die Klasse enthält Parameter, um die verwendeten Konsistenzalgorithmen zu bestimmen sowie zu welchen Zeitpunkten dies ausgeführt werden sollen.
    /// </summary>
    public class ConsistencyOptions : IXmlSerializable
    {
        /// <summary>
        /// Ein Dictionary Enthält zu jedem Konsistenzalgorithmus ein Flag, um festzulegen, ob der Algorithmus genutzt wird, oder nicht.
        /// Diese Dictionary sind wiederum eingebettet in einem Dictionary, wo der Schlüssel festlegt, zu welchem Zeitpunkt die Nutzung der Algorithmen erfolgt.
        /// </summary>
        protected Dictionary<ConsistencyCheckRegion, Dictionary<ConsistencyType, bool>> _ConsistencyMap;
        ILogger Logger { get; } =
            ApplicationLogging.CreateLogger<ConsistencyOptions>();

        /// <summary>
        /// Konstruktor für <see cref="ConsistencyOptions"/> class.
        /// </summary>
        public ConsistencyOptions()
        {
            SetDefaultValues();
        }

        #region Public Functions
        /// <summary>
        /// Setzt die Standardwerte.
        /// </summary>
        public void SetDefaultValues()
        {
            _ConsistencyMap = new Dictionary<ConsistencyCheckRegion,Dictionary<ConsistencyType, bool>>();   
      
            // Start
            Dictionary<ConsistencyType, bool> consistencyTypeMap = new Dictionary<ConsistencyType, bool>();
            consistencyTypeMap[ConsistencyType.Node] = true;
            consistencyTypeMap[ConsistencyType.Arc] = true;
            consistencyTypeMap[ConsistencyType.Bounds] = true;
            _ConsistencyMap[ConsistencyCheckRegion.Start] = consistencyTypeMap;

            // EachNodeGenerating
            consistencyTypeMap = new Dictionary<ConsistencyType, bool>();
            consistencyTypeMap[ConsistencyType.Node] = true;
            consistencyTypeMap[ConsistencyType.Arc] = false;
            consistencyTypeMap[ConsistencyType.Bounds] = false;
            _ConsistencyMap[ConsistencyCheckRegion.EachNodeGenerating] = consistencyTypeMap;

            // EachSolution
            consistencyTypeMap = new Dictionary<ConsistencyType, bool>();
            consistencyTypeMap[ConsistencyType.Node] = true;
            consistencyTypeMap[ConsistencyType.Arc] = true;
            consistencyTypeMap[ConsistencyType.Bounds] = false;
            _ConsistencyMap[ConsistencyCheckRegion.EachSolution] = consistencyTypeMap;
        }

        /// <summary>
        /// Gibt zurück, ob der Konsistenzalgorithmus genutzt wird oder nicht.
        /// </summary>
        /// <param name="region">Zeitpunkt, wo der Algorithmus ausgeführt werden könnte.</param>
        /// <param name="type">Bestimmt die Art des Konsistenzalgorithmuses.</param>
        /// <returns>true or false</returns>
        public bool GetCheckConsistencyActive(ConsistencyCheckRegion region, ConsistencyType type)
        {
            return _ConsistencyMap[region][type];
        }

        /// <summary>
        /// Legt fest, ob ein bestimmter Konsistenzalgorithmus genutzt wird oder nicht.
        /// </summary>
        /// <param name="region">Zeitpunkt, wo der Algorithmus ausgeführt werden könnte..</param>
        /// <param name="type">Bestimmt die Art des Konsistenzalgorithmuses.</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        public void SetCheckConsistencyActive(ConsistencyCheckRegion region, ConsistencyType type, bool active)
        {
            _ConsistencyMap[region][type] = active;
        }
        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            try
            {
                _ConsistencyMap.Clear();
                reader.Read();

                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if (reader.Name != "ConsistencyCheckRegion")
                    {
                        Logger.LogError("The 'ConsistencyCheckRegion' element is missing");
                    }
                    reader.MoveToNextAttribute();
                    if (reader.Name != "Region")
                    {
                        Logger.LogError("The 'Region' attribute is missing");
                    }
                    string region = reader.Value;
                    reader.Read();

                    Dictionary<ConsistencyType, bool> dictType = new Dictionary<ConsistencyType,bool>();

                    while (reader.NodeType != XmlNodeType.EndElement)
                    {
                        if (reader.Name != "Consistency")
                        {
                            Logger.LogError("The 'ConsistencyCheckRegion' element is missing");
                        }
                        reader.MoveToNextAttribute();
                        if (reader.Name != "Type")
                        {
                            Logger.LogError("The 'Type' attribute is missing");
                        }
                        string type = reader.Value;
                        reader.MoveToNextAttribute();
                        if (reader.Name != "Value")
                        {
                            Logger.LogError("The 'Value' attribute is missing");
                        }
                        string value = reader.Value;
                        ConsistencyType consType = (ConsistencyType) Enum.Parse(typeof(ConsistencyType), type, false);
                        dictType[consType] = Convert.ToBoolean(value);

                        reader.Read();
                    }

                    ConsistencyCheckRegion consRegion = (ConsistencyCheckRegion)Enum.Parse(typeof(ConsistencyCheckRegion), region, false);
                    _ConsistencyMap[consRegion] = dictType;  
                    reader.Read();
                }

                reader.Read();
            }
            catch (Exception ex)
            {
                Logger.LogCritical("ReadXml() throws an exception: ", ex);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            try
            {
                foreach (KeyValuePair<ConsistencyCheckRegion, Dictionary<ConsistencyType, bool>> pair in _ConsistencyMap)
                {
                    writer.WriteStartElement("ConsistencyCheckRegion");
                    writer.WriteAttributeString("Region", pair.Key.ToString());

                    Dictionary<ConsistencyType, bool> dictType = pair.Value;
                    if (dictType != null)
                    {
                        foreach (KeyValuePair<ConsistencyType, bool> innerPair in dictType)
                        {
                            writer.WriteStartElement("Consistency");
                            writer.WriteAttributeString("Type", innerPair.Key.ToString());
                            writer.WriteAttributeString("Value", innerPair.Value.ToString());
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();
                }
            }
            catch (Exception ex)
            {
                Logger.LogCritical("WriteXml() throws an exception: ", ex);
            }
        }

        #endregion
    }
}
