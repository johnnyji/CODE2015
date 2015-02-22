using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TemplateApp.DAO
{
    [Serializable]
    public class EditablePair
    {
        public ObjectId Id { get; set; }
        public string KeyName { get; set; }
        private object _realValue;
        private string _textValue;
        private int? _valueType;

        [BsonIgnore]
        public MongoDB.Bson.BsonType MongoType
        {
            get { return (BsonType) ValueType.GetValueOrDefault(); }
        }

        public string Value
        {
            get { return _textValue; }
            set
            {
                _textValue = value;

                if (MongoType == BsonType.EndOfDocument)
                    return;

                _realValue = ConvertToRealValue(value, MongoType);
            }
        }

        private object ConvertToRealValue(string value, BsonType type)
        {
            var bsonStr = new BsonString(value);
            switch (type)
            {
                case BsonType.Double:
                    return bsonStr.ToDouble();
                case BsonType.String:
                    return value;
                case BsonType.Document:
                    return bsonStr.ToBsonDocument();
                case BsonType.Binary:
                    return Enumerable.Range(0, value.Length)
                        .Where(x => x%2 == 0)
                        .Select(x => Convert.ToByte(value.Substring(x, 2), 16))
                        .ToArray();
                case BsonType.ObjectId:
                    return new ObjectId(value);
                case BsonType.Boolean:
                    return bsonStr.ToBoolean();
                case BsonType.DateTime:
                    return DateTime.Parse(value);
                case BsonType.Undefined:
                case BsonType.Array:
                case BsonType.Null:
                case BsonType.RegularExpression:
                case BsonType.JavaScript:
                case BsonType.Symbol:
                case BsonType.JavaScriptWithScope:
                case BsonType.MinKey:
                case BsonType.MaxKey:
                default:
                    throw new InvalidOperationException(MongoType.ToString());
                case BsonType.Int32:
                    return bsonStr.ToInt32();
                case BsonType.Timestamp:
                    return TimeSpan.Parse(value);
                case BsonType.Int64:
                    return bsonStr.ToInt64();

            }
        }

        [BsonIgnore]
        public object ValueObject
        {
            get { return _realValue; }
            set
            {
                if (value == null)
                {
                    ValueType = (int) BsonType.String;
                    _textValue = null;
                    return;
                }

                var res = BsonTypeMapper.MapToBsonValue(value);
                ValueType = (int) res.BsonType;

                if (MongoType == BsonType.Binary)
                {
                    _textValue = ((byte[]) value).Select(a => a.ToString("X2")).Aggregate((a, b) => a + b);
                }
                else
                {
                    _textValue = value.ToString();
                }

            }
        }

        public int? ValueType
        {
            get { return _valueType; }
            set
            {
                _valueType = value;
                if (_textValue == null)
                {
                    return;
                }

                _realValue = ConvertToRealValue(_textValue, MongoType);
            }
        }
    }
}