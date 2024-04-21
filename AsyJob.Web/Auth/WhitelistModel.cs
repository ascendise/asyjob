﻿using MongoDB.Bson.Serialization.Attributes;

namespace AsyJob.Web.Auth
{
    [BsonIgnoreExtraElements] //Ignores the ObjectId
    internal record WhitelistModel(string Email);
}
