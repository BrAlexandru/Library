using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public interface IAccount
    {
        string Username
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
