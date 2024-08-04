using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    [TestClass]
    public class TrackingSmapleTest
    {
    }


    internal class Order : AggregateRoot<int>
}
