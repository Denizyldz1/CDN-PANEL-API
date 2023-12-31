﻿using System.Text.Json.Serialization;

namespace AppouseProject.Core.Dtos
{
    public class BaseDto
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }
    }
}
