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
            _menuItems.Add("Cola", new MenuItem("Cola", 0.5, MenuItem.MenuItemCategory.Drinks));
            _menuItems.Add("Coffee", new MenuItem("Coffee", 1, MenuItem.MenuItemCategory.Drinks));
            _menuItems.Add("Cheese Sandwich", new MenuItem("Cheese Sandwich", 2, MenuItem.MenuItemCategory.ColdFood));
            _menuItems.Add("Steak Sandwich", new MenuItem("Steak Sandwich", 4.5, MenuItem.MenuItemCategory.HotFood));
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
        /// Gets the <see cref="MenuItem.MenuItemCategory"/> of the sought menu item.
        /// </summary>
        /// <param name="menuItemName">The sought menu item name.</param>
        /// <returns>A <see cref="MenuItem.MenuItemCategory"/> which is the category of the sought <paramref name="menuItemName"/> item.</returns>
        public static MenuItem.MenuItemCategory CategoryOfMenuItem(string menuItemName)
        {
            // Salience check.
            //
            // If the menu item is unavailable.
            if (!_menuItems.ContainsKey(menuItemName))
            {
                // Raise this exception.
                throw new ArgumentException($"Menu item '{menuItemName}' is not available on this menu.", "menuItemName");
            }

            // Return the category of the requested menu item.
            return _menuItems[menuItemName].Category;
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

            // Salience check.
            //
            // If no order information was supplied.
            if (null == menuOrder)
            {
                // Raise this exception.
                throw new ArgumentNullException("menuOrder");
            }

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

        /// <summary>
        /// Gets the overall <see cref="MenuItem.MenuItemCategory"/> for the supplied menu order.
        /// </summary>
        /// <param name="menuOrder">An <see cref="IList{string}"/> of the menu items on the order.</param>
        /// <param name="issuesWithOrder">[OUT] A record of any issues with the bill.</param>
        /// <returns>The overall <see cref="MenuItem.MenuItemCategory"/> of the order.</returns>
        public static MenuItem.MenuItemCategory OrderCategory(IList<string> menuOrder, out string issuesWithOrder)
        {
            issuesWithOrder = default(string);

            // Salience check.
            //
            // If no order information was supplied.
            if (null == menuOrder)
            {
                // Raise this exception.
                throw new ArgumentNullException("menuOrder");
            }

            StringBuilder orderIssues = new StringBuilder();

            // Default as a drinks order category.
            MenuItem.MenuItemCategory orderCategory = MenuItem.MenuItemCategory.Drinks;

            // For each menu item.
            foreach (string menuItemName in menuOrder)
            {
                try
                {
                    // Get the category of the menu item.
                    MenuItem.MenuItemCategory itemCategory = Menu.CategoryOfMenuItem(menuItemName);

                    // Check the category of this menu item.
                    switch (itemCategory)
                    {
                        // Cold food category.
                        case MenuItem.MenuItemCategory.ColdFood:

                            // If the current order category is drinks.
                            if (MenuItem.MenuItemCategory.Drinks == orderCategory)
                            {
                                // Upgrade the order category to cold food.
                                orderCategory = MenuItem.MenuItemCategory.ColdFood;
                            }
                            break;
                        
                        // Hot food category.
                        case MenuItem.MenuItemCategory.HotFood:

                            // The order category is hot food.
                            orderCategory = MenuItem.MenuItemCategory.HotFood;
                            break;
                    }
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

            // Return the order category.
            return orderCategory;
        }

        /// <summary>
        /// Calculates the service charge based on the supplied menu order.
        /// </summary>
        /// <param name="menuOrder">An <see cref="IList{string}"/> of the menu items on the order.</param>
        /// <param name="issuesWithOrder">[OUT] A record of any issues with the bill.</param>
        /// <returns>A <see cref="double"/> which is the service charge of the bill (rounded to two decimal places).</returns>
        public static double CalculateServiceCharge(IList<string> menuOrder, out string issuesWithOrder)
        {
            const double COLD_FOOD_PERCENTAGE = 0.1;
            const double HOT_FOOD_PERCENTAGE = 0.2;
            const double MAX_SERVICE_CHARGE = 20.0;

            issuesWithOrder = default(string);

            // Salience check.
            //
            // If no order information was supplied.
            if (null == menuOrder)
            {
                // Raise this exception.
                throw new ArgumentNullException("menuOrder");
            }

            // Assume no service charge.
            double serviceCharge = 0.0;

            // Get the overall categor for this order.
            MenuItem.MenuItemCategory orderCategory = OrderCategory(menuOrder, out issuesWithOrder);

            // If this is just a drinks order.
            if (MenuItem.MenuItemCategory.Drinks == orderCategory)
            {
                // There is no service charge.
                return 0.0;
            }

            // Check the order category to apply service charges.
            switch (orderCategory)
            {
                // Cold food.
                case MenuItem.MenuItemCategory.ColdFood:

                    // Calculate the cold food service charge.
                    serviceCharge = CalculateStandardBillCost(menuOrder, out issuesWithOrder) * COLD_FOOD_PERCENTAGE;
                    break;

                // Hot food.
                case MenuItem.MenuItemCategory.HotFood:

                    // Calculate the hot food service charge.
                    serviceCharge = CalculateStandardBillCost(menuOrder, out issuesWithOrder) * HOT_FOOD_PERCENTAGE;
                    break;
            }

            // Verify the service charge has not exceeded the maximum allowed.
            serviceCharge = serviceCharge > MAX_SERVICE_CHARGE ? MAX_SERVICE_CHARGE : serviceCharge;

            // Return the service charge (rounded to two decimal places).
            return Math.Round(serviceCharge, 2);
        }
    }
}
