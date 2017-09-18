using System;
using System.IO;
using System.Xml.Serialization;

namespace SharpHelper.Util {
    public static class XmlSerializationHelper {
        public static string ToXmlString(this object input) =>
            input.ToXmlString(input.GetType());

        public static string ToXmlString(this object input, Type type) {
            using (var writer = new StringWriter()) {
                new XmlSerializer(type).Serialize(writer, input);
                return writer.ToString();
            }
        }

        public static void ToXmlFile(this object objectToSerialize, string filePath) =>
            objectToSerialize.ToXmlFile(filePath, objectToSerialize.GetType());

        public static void ToXmlFile(this object objectToSerialize, string filePath, Type type) {
            using (TextWriter writer = new StreamWriter(filePath))
                new XmlSerializer(type).Serialize(writer, objectToSerialize);
        }

        public static T FromXmlString<T>(string objectData) =>
            (T) FromXmlString(objectData, typeof(T));

        public static object FromXmlString(string objectData, Type type) {
            var serializer = new XmlSerializer(type);
            using (TextReader reader = new StringReader(objectData))
                return serializer.Deserialize(reader);
        }

        public static T FromXmlFile<T>(string filePath) =>
            (T) FromXmlFile(filePath, typeof(T));

        public static object FromXmlFile(string filePath, Type type) {
            var serializer = new XmlSerializer(type);
            using (var reader = new StreamReader(filePath))
                return serializer.Deserialize(reader);
        }
    }
}
