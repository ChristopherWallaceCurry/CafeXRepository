using System;

namespace CafeX
{
    /// <summary>
    /// Simple Menu Item class.
    /// </summary>
    public class MenuItem
    {
        /// <summary>
        /// <see cref="MenuItem"/> Constructor.
        /// </summary>
        /// <param name="name">The name of the <see cref="MenuItem"/>.</param>
        /// <param name="price">The price of the <see cref="MenuItem"/>.</param>
        public MenuItem(string name, double price)
        {
            // Salience checks.
            //
            // If no name was supplied.
            if (string.IsNullOrWhiteSpace(name))
            {
                // Raise this exception.
                throw new ArgumentNullException("name");
            }

            // If the price was a nonsense.
            if (price < 0.0)
            {
                // Raise this exception.
                throw new ArgumentOutOfRangeException("price", "A value of 0 or more is expected.");
            }

            // Set the member items.
            this.Name = name;
            this.Price = price;
        }

        /// <summary>
        /// The name of the menu item.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The price of the menu item.
        /// </summary>
        public double Price { get; private set; }
    }
}
