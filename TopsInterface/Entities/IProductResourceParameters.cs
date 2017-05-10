using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TopsInterface.Entities
{
    public interface IProductResourceParameters
    {
        int Page { get; set; }
        int PageSize { get; set; }
        string SearchText { get; set; }
        int ApoClass { get; set; }
    }
}
