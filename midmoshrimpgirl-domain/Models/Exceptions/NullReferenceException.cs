using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midmoshrimpgirl_domain.Models.Exceptions;
public class NullReferenceException: Exception
{
    public NullReferenceException(string message) :base(message) { }
}
