using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityFunctions
{
    [Information(Description = "This class contains basic math functionality")]
    public class BasicMathFunctions
    {
        [Information(Description = "This method divides number1 by number2")]
        public double DivideOperation(double number1, double number2)
        {
            return number1 / number2;
        }

        [Information(Description = "This method multiplies number1 by number2")]
        public double MultiplyOperation(double number1, double number2)
        {
            return number1 * number2;
        }
    }
}
