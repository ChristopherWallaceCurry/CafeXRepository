using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeX
{
    /// <summary>
    /// Simple set of tests to exercise the <see cref="Menu"/> class.
    /// <para>&#160;</para>
    /// Ordinarily I would add these tests to a test project but for brevity I have just added the in a simple class.
    /// </summary>
    public static class MenuTests
    {
        /// <summary>
        /// Verifies that the menu prices are as expected.
        /// </summary>
        public static void Test1_Verify_MenuItem_Prices()
        {
            if (Menu.CostOfMenuItem("Cola") != 0.5)
                throw new Exception("Invalid Cola value");

            if (Menu.CostOfMenuItem("Coffee") != 1)
                throw new Exception("Invalid Cola value");

            if (Menu.CostOfMenuItem("Cheese Sandwich") != 2)
                throw new Exception("Invalid Cola value");

            if (Menu.CostOfMenuItem("Steak Sandwich") != 4.5)
                throw new Exception("Invalid Cola value");
        }

        /// <summary>
        /// Verifies that unknown menu items result in the appropriate exception.
        /// </summary>
        public static void Test2_Check_Unknown_MenuItem()
        {
            try
            {
                // We expect this to fail.
                double price = Menu.CostOfMenuItem("UNKNOWN MENU ITEM");

                // If we reach here the test has failed.
                throw new Exception("TEST FAILED");
            }
            catch (Exception exception)
            {
                // If the exception is not as expected.
                if (!exception.Message.Contains("UNKNOWN MENU ITEM"))
                {
                    // Escalate.
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks that the result of a standard bill is as expected.
        /// </summary>
        public static void Test3_Check_Standard_Bill()
        {
            string issuesWithOrder = default(string);

            // Create the order.
            List<string> menuOrder = new List<string> { "Cola", "Coffee", "Cheese Sandwich" };

            // Calculate the cost.
            double calculatedCost = Menu.CalculateStandardBillCost(menuOrder, out issuesWithOrder);

            // If there were any issues with the order.
            if (!string.IsNullOrWhiteSpace(issuesWithOrder))
                throw new Exception($"Unexpected order issues: {issuesWithOrder}");

            // If the cost is not as expected.
            if (calculatedCost != 3.5)
                throw new Exception($"A cost of 3.5 was expected but a cost of {calculatedCost} was calculated.");
        }
    }
}
