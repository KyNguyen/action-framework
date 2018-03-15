using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionFramework
{
    public static class Assert
    {
        public static void IsTrue(bool expression, string errorMessage = null)
        {
            if (!expression)
            {
                throw new TestException(errorMessage);
            }
        }

        public static void AreEqual(object expected, object actual, string errorMessage= null)
        {
            if (expected.Equals(actual))
            {
                throw new TestException(errorMessage);

            }
        }
    }
}
