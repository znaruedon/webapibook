using System;
using System.Collections.Generic;

namespace WebApiBook.Data.Contexts
{
    public partial class TbBook
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
    }
}
