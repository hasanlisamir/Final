﻿using DataAccess.Models;

namespace DataAccess.Dtos
{
    public class ModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int BrandId { get; set; }
       // public string BrandName { get; set; }
    }
}
