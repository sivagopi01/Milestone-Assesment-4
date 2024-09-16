using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace chatapplication.Models
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }  // Optional, if you need to use the Id field

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("Password")]
        public string Password { get; set; }
    }
}