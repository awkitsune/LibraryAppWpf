﻿using LibraryAppWpf.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAppWpf
{
    public static class Static
    {
        public static User CurrentUser { get; set; }

    }
}
