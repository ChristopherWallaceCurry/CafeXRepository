using System;
using System.Collections.Generic;
using System.Text;

namespace CafeX
{
    /// <summary>
    /// Simple menu class.
    /// </summary>
    public static class Menu
    {
        // The collection of menu items.
        private static readonly Dictionary<string, MenuItem> _menuItems = new Dictionary<string, MenuItem>();

        /// <summary>
        /// Static <see cref="Menu"/> constructor.
        /// </summary>
        static Menu()
        {
            // Add the available items on the menu.
            _menuItems.Add("Cola", new MenuItem("Cola", 0.5));
            _menuItems.Add("Coffee", new MenuItem("Coffee", 1));
            _menuItems.Add("Cheese Sandwich", new MenuItem("Cheese Sandwich", 2));
            _menuItems.Add("Steak Sandwich", new MenuItem("Steak Sandwich", 4.5));
        }

        /// <summary>
        /// Gets the cost of the sought menu item.
        /// </summary>
        /// <param name="menuItemName">The sought menu item name.</param>
        /// <returns>A <see cref="double"/> which is the cost of the sought <paramref name="menuItemName"/> item.</returns>
        public static double CostOfMenuItem(string menuItemName)
        {
            // Salience check.
            //
            // If the menu item is unavailable.
            if (!_menuItems.ContainsKey(menuItemName))
            {
                // Raise this exception.
                throw new ArgumentException($"Menu item '{menuItemName}' is not available on this menu.", "menuItemName");
            }

            // Return the price of the requested menu item.
            return _menuItems[menuItemName].Price;
        }

        /// <summary>
        /// Calculates the standard bill cost based on the supplied menu order.
        /// </summary>
        /// <param name="menuOrder">An <see cref="IList{string}"/> of the menu items on the order.</param>
        /// <param name="issuesWithOrder">[OUT] A record of any issues with the bill.</param>
        /// <returns>A <see cref="double"/> which is the cost of the bill.</returns>
        public static double CalculateStandardBillCost(IList<string> menuOrder, out string issuesWithOrder)
        {
            issuesWithOrder = default(string);
            StringBuilder orderIssues = new StringBuilder();
            double billTotal = 0.0;

            // For each menu item.
            foreach (string menuItemName in menuOrder)
            {
                try
                {
                    // Add the cost of this menu item
                    billTotal += CostOfMenuItem(menuItemName);
                }
                catch (Exception exception)
                {
                    // Append this issue to the order issues.
                    orderIssues.AppendLine(exception.Message);
                }
            }

            // If there were any issues with the order.
            if (orderIssues.Length > 0)
            {
                // Set the order issues.
                issuesWithOrder = orderIssues.ToString();
            }

            // Return the bill total.
            return billTotal;
        }
    }
}
