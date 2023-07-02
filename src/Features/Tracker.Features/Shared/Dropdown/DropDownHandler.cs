using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Features.Shared.Dropdown
{
    public abstract class DropDown
    {
        public abstract Task<List<Itemlist>> GetDropDownList(string Type);
        public abstract Task<List<Itemlist>> GetDropDownListbyId(string Type,string id);
    }
    internal class DropDownHandler
    {
    }
}
