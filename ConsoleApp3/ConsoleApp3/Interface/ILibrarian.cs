using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface ILibrarian
    {

        string AdminID
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        string Password
        {
            get;
            set;
        }
        
    }
}
