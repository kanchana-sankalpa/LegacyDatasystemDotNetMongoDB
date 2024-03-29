﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetcondapackage.Models
{
    public interface IDatabaseSettings2
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
    public class DatabaseSettings2 : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
