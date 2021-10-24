#if !NETSTANDARD1_3

namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    #endregion Usings

    /// <summary>
    /// Provides a generic collection that supports data binding and sorting.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    [Serializable]
    public sealed class SortableBindingList<T> : BindingList<T> where T : class
    {
        #region Fields

        /// <summary>
        /// Place holder variable for <see cref="IsSortedCore"/>.
        /// </summary>
        private bool isSorted;

        /// <summary>
        /// Place holder variable for <see cref="SortDirectionCore"/>.
        /// </summary>
        private ListSortDirection sortDirection;

        /// <summary>
        /// Place holder variable for <see cref="SortPropertyCore"/>.
        /// </summary>
        private PropertyDescriptor sortProperty;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        public SortableBindingList() : base(new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SortableBindingList{T}"/> class.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        public SortableBindingList(IEnumerable<T> enumeration) : base(new List<T>(enumeration))
        {
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the list is sorted.
        /// </summary>
        /// <value>
        /// True if the list is sorted; otherwise false.
        /// </value>
        protected override bool IsSortedCore
        {
            get { return this.isSorted; }
        }

        /// <summary>
        /// Gets the direction the list is sorted.
        /// </summary>
        /// <value>The sort direction core.</value>
        protected override ListSortDirection SortDirectionCore
        {
            get { return this.sortDirection; }
        }

        /// <summary>
        /// Gets the property descriptor that is used for sorting the list.
        /// </summary>
        /// <value>The <see cref="PropertyDescriptor"/> used for sorting the
        /// list.</value>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return this.sortProperty; }
        }

        /// <summary>
        /// Gets a value indicating whether the list supports searching.
        /// </summary>
        /// <value>
        /// True if the list supports searching; otherwise, false.
        /// </value>
        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the list supports sorting.
        /// </summary>
        /// <value>True if the list supports sorting; otherwise, false.</value>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        #endregion Properties

        #region Sort

        /// <summary>
        /// Sorts the collection based on the specified property name and
        /// direction.
        /// </summary>
        /// <param name="propertyName">The property name with in the class
        /// object to sort the data.</param>
        /// <param name="direction">One of the <see cref="ListSortDirection"/>
        /// values.</param>
        public void Sort(string propertyName, ListSortDirection direction)
        {
            if (propertyName.IsNullOrEmpty())
            {
                return;
            }

            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(typeof(T));
            if (pdc.Count < 1)
            {
                return;
            }

            PropertyDescriptor descriptor = pdc[propertyName];
            this.ApplySortCore(descriptor, direction);
        }

        /// <summary>
        /// Sorts the items in the list.
        /// </summary>
        /// <param name="prop">A <see cref="PropertyDescriptor"/> that
        /// specifies the property to sort on.</param>
        /// <param name="direction">One of the <see cref="ListSortDirection"/>
        /// values.</param>
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            this.sortDirection = direction;
            this.sortProperty = prop;

            var list = this.Items as List<T>;
            if (list == null)
            {
                return;
            }

            list.Sort(this.Compare);
            this.isSorted = true;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// Removes any sort applied with <see cref="ApplySortCore"/>.
        /// </summary>
        protected override void RemoveSortCore()
        {
            this.isSorted = false;
            this.sortProperty = null;
            this.sortDirection = base.SortDirectionCore;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is
        /// less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>Value condition Less than zero x is less than y. Zero x
        /// equals y. Greater than zero x is greater than y.</returns>
        private int Compare(T x, T y)
        {
            int result = this.OnComparison(x, y);

            // Invert sort order if descending.
            return this.sortDirection == ListSortDirection.Descending ? -result : result;
        }

        /// <summary>
        /// Called when [comparison].
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>Value condition Less than zero x is less than y. Zero x
        /// equals y. Greater than zero x is greater than y.</returns>
        private int OnComparison(T x, T y)
        {
            object objectX = x == null ? null : this.sortProperty.GetValue(x);
            object objectY = y == null ? null : this.sortProperty.GetValue(y);

            if (objectX == null)
            {
                // Nulls are equal.
                return objectY == null ? 0 : -1;
            }

            if (objectY == null)
            {
                // First has value, second doesn't have value.
                return 1;
            }

            if (objectX is IComparable comparable)
            {
                // Object implements IComparable for comparison.
                return comparable.CompareTo(objectY);
            }

            if (objectX.Equals(objectY))
            {
                // Both are the same.
                return 0;
            }

            // Not comparable, compare using ToString.
            return string.Compare(objectX.ToString(), objectY.ToString(), StringComparison.Ordinal);
        }

        #endregion Sort
    }
}

#endif