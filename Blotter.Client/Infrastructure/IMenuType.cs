using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blotter.Client.Infrastructure
{
    public interface IMenuBuilder
    {
        List<MenuItem> Build();
    }
}