using CatenaryCAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatenaryCAD
{
    public interface IDependencies
    {
        IModel Parent { get; }
        IModel[] Childrens { get; }
        IModel[] Dependens { get; }
    }
}
