using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcondapackage.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Domain { get; set; }
        public string Password { get; set; }
        public string Agency { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedTime { get; set; }
        public string Token { get; set; }
    }
}
